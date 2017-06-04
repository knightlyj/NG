using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EquipPanel : MonoBehaviour {

    public Transform equipSlotPrefab;
    //slot
    public int slotToSide;
    public int slotGap;
    public int slotSize;

    public int width, height;

    //武器格子
    ItemSlot[] weaponSlots = null;

    //护甲格子
    ItemSlot[] armorSlots = null;

    // Use this for initialization
    void Start () {
        weaponSlots = new ItemSlot[4];
        Array a = Enum.GetValues(typeof(ArmorType));
        armorSlots = new ItemSlot[a.Length];

        for(int i = 0; i < 4; i++)
        {
            weaponSlots[i] = transform.FindChild("Weapon" + i).GetComponent<ItemSlot>();
        }

        for (int i = 0; i < a.Length; i++)
        {
            armorSlots[i] = transform.FindChild(((ArmorType)a.GetValue(i)).ToString()).GetComponent<ItemSlot>();
        }
    }

    Player localPlayer = null;
    // Update is called once per frame
    void Update () {
        if (localPlayer == null)
        {//还没绑定到本地玩家,则寻找
            localPlayer = Helper.FindLocalPlayer();
            if (localPlayer != null)
            {
                UpdateEquip();
                localPlayer.EquipChangedEvent += this.OnEquipChanged;
                localPlayer.EntityDestroyEvent += this.OnPlayerDestroyed;
            }
        }
    }

    void UpdateEquip()
    {
        if(localPlayer != null)
        {
            //更新武器栏
            for(int i = 0; i < 4; i++)
            {
                weaponSlots[i].SetItemInfo(localPlayer.Weapons[i]);
            }

            //更新护甲栏
            for(int i = 0; i < armorSlots.Length; i++)
            {
                armorSlots[i].SetItemInfo(localPlayer.Armors[i]);
            }
        }
        else
        {
            Debug.LogError("EquipPanel.UpdateEquip >> local player is null");
        }
    }

    //背包物品有变化时的回调
    void OnEquipChanged()
    {
        UpdateEquip();
    }

    //玩家销毁时的回调
    void OnPlayerDestroyed()
    {
        localPlayer.EntityDestroyEvent -= this.OnPlayerDestroyed;
        localPlayer.EquipChangedEvent -= this.OnEquipChanged;
        localPlayer = null;
    }
}
