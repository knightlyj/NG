using UnityEngine;
using System.Collections;

public partial struct ItemType
{
    public enum Type
    {
        Nothing,
        Wood,
        Iron,
        Copper,
        Gold,
        TestGun,
        TestHealthPotion,
    }

    readonly Type type; //id
    readonly int typeBit; //识别材料,消耗品等

    const int materialOffset = 0;
    const int consumableOffset = 1;
    const int armorOffset = 2;
    const int weaponOffset = 3;

    public bool IsMaterail { get { return (typeBit & materialOffset) != 0; } }
    public bool IsConsumable { get { return (typeBit & consumableOffset) != 0; } }
    public bool IsArmor { get { return (typeBit & armorOffset) != 0; } }
    public bool IsWeapon { get { return (typeBit & weaponOffset) != 0; } }

    ItemType(Type id, int type)
    {
        this.type = id;
        this.typeBit = type;
    }

    //public override bool Equals(object obj)
    //{
    //    return obj is ItemType && this == (ItemType)obj;
    //}

    //public override int GetHashCode()
    //{
    //    return (this.typeBit << 5) + (int)type;
    //}

    static public bool operator == (ItemType t1, ItemType t2)
    {
        return t1.type == t2.type;
    }

    static public bool operator !=(ItemType t1, ItemType t2)
    {
        return t1.type != t2.type;
    }

    
}

public class Item {


    ItemType _type;
    public ItemType Type { get { return _type; } }
    
    public uint amount = 0; //为消耗品或材料时,可以堆叠
    public string icon = null;

    public Item(ItemType type, uint a)
    {
        this._type = type;
        this.amount = a;
    }
}
