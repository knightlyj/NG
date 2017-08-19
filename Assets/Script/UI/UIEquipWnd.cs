using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIEquipWnd : MonoBehaviour
{
    UIItemSlot[] weaponSlots = new UIItemSlot[PlayerEquipment.weaponAmount];
    UIItemSlot[] armorSlots;
    readonly string[] armorSlotName = { "Helmet", "Armor", "Boot", "Glove", "Accessory0", "Accessory1", "Accessory2" };
    void Awake()
    {
        for (int i = 0; i < PlayerEquipment.weaponAmount; i++)
        {
            weaponSlots[i] = transform.FindChild("Bg").FindChild("Weapon" + i).GetComponent<UIItemSlot>();
            weaponSlots[i].index = i;
            weaponSlots[i].MouseDownEvent += this.OnWeaponSlotClick;
        }
        
        armorSlots = new UIItemSlot[armorSlotName.Length];
        for (int i = 0; i < armorSlotName.Length; i++)
        {
            armorSlots[i] = transform.FindChild("Bg").FindChild(armorSlotName[i]).GetComponent<UIItemSlot>();
            armorSlots[i].index = i;
            armorSlots[i].MouseDownEvent += this.OnArmorSlotClick;
        }
       
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
            BindEquipment(localPlayer.equipment);
    }

    void OnDestroy()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].MouseDownEvent -= this.OnWeaponSlotClick;
        }

        for (int i = 0; i < armorSlots.Length; i++)
        {
            armorSlots[i].MouseDownEvent -= this.OnArmorSlotClick;
        }

        UnbindEquipment();

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

    }

    //武器格子点击回调
    void OnWeaponSlotClick(UIItemSlot slot, PointerEventData eventData)
    {
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null && bindEquipment != null)
        {
            MouseItem mouseItem = localPlayer.bag.mouseItem;
            if (mouseItem.hasItem)
            { //鼠标有物品,则尝试装备
                if (mouseItem.item.Type.IsWeapon)
                {
                    Item preWeapon = null;
                    bindEquipment.PutOnWeapon(mouseItem.TakeItem(), slot.index, out preWeapon);
                    mouseItem.PutItem(preWeapon); //鼠标放入之前格子里的武器
                }
                else
                { //鼠标上的不是武器,什么都不用做

                }
            }
            else
            {   //鼠标没物品,尝试取下武器,如果没武器,则会得到null
                mouseItem.PutItem(bindEquipment.TakeOffWeapon(slot.index));
            }
            Helper.ShowTips(bindEquipment.Weapons[slot.index]);
        }
        Helper.MoveWndToFront(transform);
    }

    //护甲格子点击回调
    void OnArmorSlotClick(UIItemSlot slot, PointerEventData eventData)
    {
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null && bindEquipment != null)
        {
            MouseItem mouseItem = localPlayer.bag.mouseItem;
            if (mouseItem.hasItem)
            { //鼠标有物品,则尝试装备
                if (mouseItem.item.Type.IsArmor)
                {
                    ArmorProperties armorProp = EquipTable.GetArmorProp(mouseItem.item.Type.armorId);
                    if (armorProp != null && armorProp.armorType == PlayerEquipment.armorTypes[slot.index])
                    { //护甲有效,护甲类型匹配,则装备
                        Item preArmor = null;
                        bindEquipment.PutOnArmor(mouseItem.TakeItem(), slot.index, out preArmor); //穿上护甲
                        mouseItem.PutItem(preArmor); //鼠标放入之前格子里的护甲
                    }
                }
                else
                { //鼠标上的不是护甲,什么都不用做

                }
            }
            else
            {   //鼠标没物品,尝试取下武器,如果没武器,则会得到null
                mouseItem.PutItem(bindEquipment.TakeOffArmor(slot.index));
            }
            Helper.ShowTips(bindEquipment.Armors[slot.index]);
        }
        Helper.MoveWndToFront(transform);
    }

    //更新装备窗口
    void UpdateEquip()
    {
        //显示武器
        for (int i = 0; i < PlayerEquipment.weaponAmount; i++)
        {
            weaponSlots[i].SetItemInfo(bindEquipment.Weapons[i]);
        }
        //显示护甲
        for(int i = 0; i < armorSlots.Length; i++)
        {
            armorSlots[i].SetItemInfo(bindEquipment.Armors[i]);
        }
    }

    //装备改变事件
    void OnEquipChanged()
    {
        UpdateEquip();
    }

    PlayerEquipment bindEquipment = null;
    void BindEquipment(PlayerEquipment equip)
    {
        UnbindEquipment();

        bindEquipment = equip;
        bindEquipment.EquipChangedEvent += this.OnEquipChanged;
        this.UpdateEquip();
    }

    void UnbindEquipment()
    {
        if (bindEquipment != null)
        {
            bindEquipment.EquipChangedEvent -= this.OnEquipChanged;
            bindEquipment = null;
        }
    }

    void OnLocalPlayerCreate(System.Object sender)
    {
        LocalPlayer localPlayer = sender as LocalPlayer;
        if (localPlayer != null)
        {
            BindEquipment(localPlayer.equipment);
        }
    }

    void OnLocalPlayerDestroy(System.Object sender)
    {
        UnbindEquipment();
    }

    void OnLocalPlayerLoad(System.Object sender)
    {
        LocalPlayer localPlayer = sender as LocalPlayer;
        if (localPlayer != null)
        {
            BindEquipment(localPlayer.equipment);
        }
    }
}
