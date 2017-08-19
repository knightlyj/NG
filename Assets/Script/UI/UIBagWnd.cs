using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class UIBagWnd : MonoBehaviour
{
    //布局用参数
    public Transform packSlotPrefab;
    public int slotToSide;
    public int slotGap;
    public int slotSize;

    //背包物品格子
    UIItemSlot[] packSlots = null;
    UIItemSlot trashSlot = null;

    //金钱显示
    Text money = null;

    //awake会在setactive后立即调用,初始化写在这里
    void Awake()
    {
        Transform trPack = transform.FindChild("Bg");
        //物品栏 8行5列
        packSlots = new UIItemSlot[LocalPlayer.itemPackSize];
        for (int i = 0; i < LocalPlayer.itemPackSize; i++)
        {
            packSlots[i] = trPack.FindChild("Slot" + i).GetComponent<UIItemSlot>();
            packSlots[i].index = i;
            packSlots[i].MouseDownEvent += this.OnItemMouseDown;
        }

        trashSlot = trPack.FindChild("Trash").GetComponent<UIItemSlot>();
        trashSlot.MouseDownEvent += this.OnTrashMouseDown;

        money = trPack.FindChild("MoneyBg").FindChild("Money").GetComponent<Text>();

        
        //订阅本地玩家创建事件
        EventManager.AddListener(EventId.LocalPlayerCreate, this.OnLocalPlayerCreate);
        //订阅本地玩家销毁事件
        EventManager.AddListener(EventId.LocalPlayerDestroy, this.OnLocalPlayerDestroy);
        //读取存档事件,背包数据会改变
        EventManager.AddListener(EventId.LocalPlayerLoad, this.OnLocalPlayerLoad);
    }

    // Use this for initialization
    void Start()
    {
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
            BindBag(localPlayer.bag);
    }

    void OnDestroy()
    {
        //物品栏 8行5列
        foreach (UIItemSlot slot in packSlots)
        {
            slot.MouseDownEvent -= this.OnItemMouseDown;
        }

        trashSlot.MouseDownEvent -= this.OnTrashMouseDown;

        //退订本地玩家创建事件
        EventManager.RemoveListener(EventId.LocalPlayerCreate, this.OnLocalPlayerCreate);
        //退订本地玩家销毁事件
        EventManager.RemoveListener(EventId.LocalPlayerDestroy, this.OnLocalPlayerDestroy);
        //退订读取存档事件,背包数据会改变
        EventManager.RemoveListener(EventId.LocalPlayerLoad, this.OnLocalPlayerLoad);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (bindBag != null)
        {
            //获取背包
            money.text = bindBag.money.ToString(); //金钱数量
            trashSlot.SetItemInfo(bindBag.trash.item);
        }
    }

    //点下物品格
    void OnItemMouseDown(UIItemSlot slot, PointerEventData eventData)
    {
        if (bindBag != null)
        {
            //获取背包内容
            ItemPackage itemPack = bindBag.itemPack;
            if (eventData.button == PointerEventData.InputButton.Left)
            { // 左键
                if (Input.GetKey(KeyCode.LeftShift)) //按住shift,移动到垃圾箱
                {
                    Item itemInSlot = null; //原来格子里的物品
                    itemInSlot = itemPack.TakeItem(slot.index);
                    if (itemInSlot != null) //如果有物品,放入垃圾箱
                    {
                        bindBag.trash.PutItem(itemInSlot);
                    }
                }
                else
                {   //没有按shift
                    MouseItem mouseItem = bindBag.mouseItem;
                    if (mouseItem.hasItem) //鼠标拖拽有物品,放入格子
                    {
                        Item itemInSlot = null; //原来格子里的物品
                        itemPack.PutInItem(mouseItem.TakeItem(), slot.index, out itemInSlot);
                        //可以叠加或原来没有物品,则itemInSlot为null,不论是否null,放入鼠标即可
                        mouseItem.PutItem(itemInSlot);
                    }
                    else
                    { //鼠标没有物品,取出原来格子里的物品,放入鼠标即可
                        mouseItem.PutItem(itemPack.TakeItem(slot.index));
                    }
                }
            }
            else
            {   //右键,使用
                Item itemInSlot = null; //原来格子里的物品
                itemInSlot = itemPack.TakeItem(slot.index); //拿出来
                if (itemInSlot.Type.IsArmor)
                {   //护甲,装备上,饰品就放在第一个格子里
                    LocalPlayer localPlayer = Helper.FindLocalPlayer();
                    if (localPlayer != null)
                    {
                        Item preArmor = null;
                        localPlayer.equipment.PutOnArmor(itemInSlot, out preArmor); //穿上护甲
                        itemPack.PutInItem(preArmor, slot.index, out itemInSlot); //脱下的护甲放入这个格子里
                    }
                }
            }
            Helper.ShowTips(itemPack.content[slot.index]);
        }

        Helper.MoveWndToFront(transform);
    }

    void OnTrashMouseDown(UIItemSlot slot, PointerEventData eventData)
    {
        if (bindBag != null)
        {
            //获取背包内容
            Trash trash = bindBag.trash;
            MouseItem mouseItem = bindBag.mouseItem;
            if (mouseItem.hasItem) //鼠标拖拽有物品,放入格子
            {
                trash.PutItem(mouseItem.TakeItem());
            }
            else
            {
                mouseItem.PutItem(trash.TakeItem());
            }
            Helper.ShowTips(trash.item);
        }

        Helper.MoveWndToFront(transform);
    }

    //更新背包
    void UpdateItemPack()
    {
        //显示所有物品
        for (int i = 0; i < LocalPlayer.itemPackSize; i++)
        {
            packSlots[i].SetItemInfo(bindBag.itemPack.content[i]);
        }
        //Debug.Log("update pack");
    }

    //背包物品有变化时的回调
    void OnPackChanged()
    {
        UpdateItemPack();
    }

    //背包关闭时的回调
    void OnPackClosed()
    {
        UnbindBag();
    }

    PlayerBag bindBag = null;
    void BindBag(PlayerBag bag)
    {
        //先解绑
        UnbindBag();
        if (bag == null)
            return;
        //绑定到新的package
        bindBag = bag;
        //暂时没考虑背包关闭的问题
        bindBag.itemPack.PackChangedEvent += this.OnPackChanged; //item包改变

        this.UpdateItemPack();
    }

    void UnbindBag()
    {
        if (bindBag != null)
        {
            bindBag.itemPack.PackChangedEvent -= this.OnPackChanged;
            bindBag = null;
        }
    }

    void OnLocalPlayerCreate(System.Object sender)
    {
        LocalPlayer localPlayer = sender as LocalPlayer;
        if (localPlayer != null)
        {
            BindBag(localPlayer.bag);
        }
    }

    void OnLocalPlayerDestroy(System.Object sender)
    {
        UnbindBag();
    }

    void OnLocalPlayerLoad(System.Object sender)
    {
        LocalPlayer localPlayer = sender as LocalPlayer;
        if (localPlayer != null)
        {
            BindBag(localPlayer.bag);
        }
    }
}
