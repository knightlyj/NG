using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIEquipWnd : MonoBehaviour
{
    UIItemSlot[] weaponSlots = new UIItemSlot[PlayerEquipment.weaponAmount];
    UIItemSlot[] armorSlots;

    void Awake()
    {
        for (int i = 0; i < PlayerEquipment.weaponAmount; i++)
        {
            weaponSlots[i] = transform.FindChild("Bg").FindChild("Weapon" + i).GetComponent<UIItemSlot>();
            weaponSlots[i].index = i;
            weaponSlots[i].MouseDownEvent += this.OnWeaponSlotClick;
        }

        Array a = Enum.GetValues(typeof(ArmorType));
        armorSlots = new UIItemSlot[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            armorSlots[i] = transform.FindChild("Bg").FindChild(Enum.GetName(typeof(ArmorType), i)).GetComponent<UIItemSlot>();
            armorSlots[i].index = i;
            armorSlots[i].MouseDownEvent += this.OnArmorSlotClick;
        }

        
    }

    // Use this for initialization
    void Start()
    {
        Player localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
            BindEquipment(localPlayer.playerEquipment);
        //订阅本地玩家创建事件
        EventManager.AddListener(EventId.LocalPlayerCreate, this.OnLocalPlayerCreate);
        //订阅本地玩家销毁事件
        EventManager.AddListener(EventId.LocalPlayerDestroy, this.OnLocalPlayerDestroy);
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    //武器格子点击回调
    void OnWeaponSlotClick(UIItemSlot slot, PointerEventData eventData)
    {
        Player localPlayer = Helper.FindLocalPlayer();
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
        }
    }

    //护甲格子点击回调
    void OnArmorSlotClick(UIItemSlot slot, PointerEventData eventData)
    {
        Player localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null && bindEquipment != null)
        {
            MouseItem mouseItem = localPlayer.bag.mouseItem;
            if (mouseItem.hasItem)
            { //鼠标有物品,则尝试装备
                if (mouseItem.item.Type.IsArmor)
                {
                    Item preArmor = null;
                    bindEquipment.PutOnArmor(mouseItem.TakeItem(), out preArmor); //穿上护甲
                    mouseItem.PutItem(preArmor); //鼠标放入之前格子里的护甲
                }
                else
                { //鼠标上的不是护甲,什么都不用做

                }
            }
            else
            {   //鼠标没物品,尝试取下武器,如果没武器,则会得到null
                mouseItem.PutItem(bindEquipment.TakeOffArmor((ArmorType)slot.index));
            }
        }
    }

    //更新装备窗口
    void UpateEquip()
    {
        //显示武器
        for (int i = 0; i < PlayerEquipment.weaponAmount; i++)
        {
            weaponSlots[i].SetItemInfo(bindEquipment.Weapons[i]);
        }
    }

    //装备改变事件
    void OnEquipChanged()
    {
        UpateEquip();
    }

    PlayerEquipment bindEquipment = null;
    void BindEquipment(PlayerEquipment equip)
    {
        UnbindEquipment();

        bindEquipment = equip;
        bindEquipment.EquipChangedEvent += this.OnEquipChanged;
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
        Player localPlayer = sender as Player;
        if (localPlayer != null)
        {
            BindEquipment(localPlayer.playerEquipment);
        }
    }

    void OnLocalPlayerDestroy(System.Object sender)
    {
        UnbindEquipment();
    }
}
