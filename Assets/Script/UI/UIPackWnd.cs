using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class UIPackWnd : MonoBehaviour
{
    public Transform packSlotPrefab;


    //背包物品格子
    UIItemSlot[] packSlots = null;
    UIItemSlot trashSlot = null;

    //金钱显示
    Text money = null;

    //slot
    public int slotToSide;
    public int slotGap;
    public int slotSize;

    public int width, height;

    void Awake()
    {
        Transform trPack = transform.FindChild("Bg");
        //物品栏 8行5列
        packSlots = new UIItemSlot[Player.itemPackSize];
        for (int i = 0; i < Player.itemPackSize; i++)
        {
            packSlots[i] = trPack.FindChild("Slot" + i).GetComponent<UIItemSlot>();
            packSlots[i].index = i;
            packSlots[i].MouseDownEvent += this.OnItemMouseDown;
        }

        trashSlot = trPack.FindChild("Trash").GetComponent<UIItemSlot>();
        trashSlot.MouseDownEvent += this.OnTrashMouseDown;

        money = trPack.FindChild("MoneyBg").FindChild("Moeny").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        this.BindPackage(Helper.playerPackage);

    }

    void OnDestroy()
    {
        Transform trPack = transform.FindChild("Bg");
        //物品栏 8行5列
        foreach (UIItemSlot slot in packSlots)
        {
            slot.MouseDownEvent -= this.OnItemMouseDown;
        }

        trashSlot.MouseDownEvent -= this.OnTrashMouseDown;
    }

    Player localPlayer = null;
    // Update is called once per frame
    void Update()
    {
        //if (localPlayer == null) 
        //{//还没绑定到本地玩家,则寻找
        //    localPlayer = Helper.FindLocalPlayer();
        //    if (localPlayer != null)
        //    {
        //        UpdatePack();
        //        localPlayer.PackChangedEvent += this.OnPackChanged;
        //        localPlayer.EntityDestroyEvent += this.OnPlayerDestroyed;
        //    }
        //}
        //else
        //{ //如果玩家有拖拽物品,则跟随鼠标
        //    if (localPlayer.mouseItem != null)
        //    {
        //        this.mouseSlot.transform.position = Input.mousePosition; //鼠标位置就是屏幕位置
        //    }
        //}



        //Player localPlayer = Helper.FindLocalPlayer();
        //if (localPlayer != null)
        {
            //获取背包
            //PlayerPackage playerPack = localPlayer.playerPack;
            money.text = bindPack.money.ToString(); //金钱数量
            trashSlot.SetItemInfo(bindPack.trash.item);
        }
    }

    void OnItemMouseDown(UIItemSlot slot)
    {
        if (bindPack != null)
        {
            //获取背包内容
            ItemPackage itemPack = bindPack.itemPack;
            MouseItem mouseItem = bindPack.mouseItem;
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

        Helper.MoveWndToFront(transform);
    }

    void OnTrashMouseDown(UIItemSlot slot)
    {
        if (bindPack != null)
        {
            //获取背包内容
            ItemPackage pack = bindPack.itemPack;
            Trash trash = bindPack.trash;
            MouseItem mouseItem = bindPack.mouseItem;
            if (mouseItem.hasItem) //鼠标拖拽有物品,放入格子
            {
                trash.PutItem(mouseItem.TakeItem());
            }
            else
            {
                mouseItem.PutItem(trash.TakeItem());
            }
        }

        Helper.MoveWndToFront(transform);
    }

    //更新背包
    void UpdatePack()
    {
        //显示所有物品
        for (int i = 0; i < Player.itemPackSize; i++)
        {
            packSlots[i].SetItemInfo(bindPack.itemPack.content[i]);
        }
        //Debug.Log("update pack");
    }

    //背包物品有变化时的回调
    void OnPackChanged()
    {
        UpdatePack();
    }

    //背包关闭时的回调
    void OnPackClosed()
    {
        UnBindPackage();
    }

    PlayerPackage bindPack = null;
    void BindPackage(PlayerPackage pack)
    {
        //先解绑
        UnBindPackage();
        if (pack == null)
            return;
        //绑定到新的package
        bindPack = pack;
        //暂时没考虑背包关闭的问题
        bindPack.itemPack.PackChangedEvent += this.OnPackChanged; //item包改变
        bindPack.PlayerPackClosedEvent += this.OnPackClosed;

        this.UpdatePack();
    }

    void UnBindPackage()
    {
        if (bindPack != null)
        {
            bindPack.itemPack.PackChangedEvent -= this.OnPackChanged;
            bindPack.PlayerPackClosedEvent -= this.OnPackClosed;
            bindPack = null;
        }
    }
}
