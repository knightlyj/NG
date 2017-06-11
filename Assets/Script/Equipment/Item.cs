using UnityEngine;
using System.Collections;
using System;

public enum ItemQuality
{
    White,
    Green,
    Blue,
    Purple,
    Red,
    Golden,
}

public enum ItemId
{
    Wood,
    Iron,
    Copper,
    Gold,
    TestGun,
    TestHealthPotion,
    Max,
}

public struct RawMaterial
{
    public ItemId id;
    public uint amount;
}

[Serializable]
public class ItemType : UnityEngine.Object
{
    public ItemId id; //id
    public string itemName; //名字
    public string icon; //图标
    public ItemQuality quality; //品质,决定物品名的颜色
    public string comment; //注释
    public int typeBit; //识别材料,消耗品等

    const int materialOffset = 0;
    const int consumableOffset = 1;
    const int armorOffset = 2;
    const int weaponOffset = 3;
    const int canCraftOffset = 4;

    public bool IsMaterial { get { return (typeBit & materialOffset) != 0; } }
    public bool IsConsumable { get { return (typeBit & consumableOffset) != 0; } }
    public bool IsArmor { get { return (typeBit & armorOffset) != 0; } }
    public bool IsWeapon { get { return (typeBit & weaponOffset) != 0; } }

    public bool CanCraft { get { return (typeBit & canCraftOffset) != 0; } }
    public bool CanStack {get { return IsMaterial || IsConsumable; } }

    public RawMaterial[] rawMats = null;

    public ItemType()
    {
        this.id = ItemId.Wood;
        this.typeBit = 0;
        quality = ItemQuality.White;
        comment = "";
        itemName = "";
        icon = "";
    }

    public ItemType(ItemId id, int typeBit)
    {
        this.id = id;
        this.typeBit = typeBit;
        quality = ItemQuality.White;
        comment = "";
        itemName = "";
        icon = "";
    }

    //public override bool Equals(object obj)
    //{
    //    return base.Equals(obj);
    //}

    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}
}



public class Item {
    ItemType _type; //这里保存物品的种类信息
    public ItemType Type { get { return _type; } }
    
    public uint amount = 0; //为消耗品或材料时,可以堆叠
    //public string icon = null;

    public Item(ItemType type, uint a)
    {
        this._type = type;
        this.amount = a;

    }
    //public Item(ItemType type, uint a)
    //{
    //    this._type = type;
    //    this.amount = a;
    //}
}
