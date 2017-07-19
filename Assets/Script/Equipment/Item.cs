using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;

public enum ItemQuality
{
    White,
    Green,
    Blue,
    Purple,
    Red,
    Golden,
}

public class ItemType// : UnityEngine.Object
{
    public int id; //id
    public string itemName; //名字,要与资源文件夹的名字一致
    public ItemQuality quality; //品质,决定物品名的颜色
    public string comment; //注释
    public string icon; //图标 
    public int weaponId = 0; //装备id
    public int armorId = 0;//护甲id
    public int consumId = 0; //消耗品id,为0时表示不是消耗品
    public int originalPrice = 0; //原始价格,不考虑装备加成等

    //public bool IsMaterial { get { return (typeBit & materialOffset) != 0; } }
    public bool IsConsumable { get { return consumId != 0; } }
    public bool IsWeapon { get { return weaponId != 0; } }
    public bool IsArmor { get { return armorId != 0; } }
    public bool IsEquipment { get { return IsWeapon || IsArmor; } }

    //public bool CanCraft { get { return false; } }
    public bool CanStack { get { return !IsEquipment; } }

    public CraftFormula craftItem = null;

    public ItemType()
    {
        this.id = 0;// ItemId.Wood;
        quality = ItemQuality.White;
        comment = "";
        itemName = "";
    }

    public ItemType(int id)
    {
        this.id = id;
        quality = ItemQuality.White;
        comment = "";
        itemName = "";
    }
}

public class RawMaterial
{
    public int id;
    public uint amount;
}

public class CraftFormula
{
    public const int maxRawMatSorts = 6; //最多6种材料
    public int matCount = 0; //实际所需材料种类
    public RawMaterial[] rawMats = new RawMaterial[maxRawMatSorts]; //材料表
    public int outputId = -1;
    public uint outputAmount = 0;

    public void AddMaterial(RawMaterial mat)
    {
        if (matCount >= maxRawMatSorts)
            return;
        rawMats[matCount++] = mat;
    }
}

[Serializable]
public class Item : ISerializable
{
    ItemType _type; //这里保存物品的种类信息
    public ItemType Type { get { return _type; } }

    public int id = 0;
    public uint amount = 0; //为消耗品或材料时,可以堆叠,做装备处理时,不考虑amout项
    //public string icon = null;

    public Item(ItemType type, uint a)
    {
        this.id = type.id;
        this._type = type;
        this.amount = a;
    }

    public Item(int id, uint a)
    {
        this.id = id;
        this._type = ItemTypeTable.GetItemType(id);
        this.amount = a;
    }

    public bool valid { get { return this._type != null; } }

    public int buyPrice
    {
        get
        {
            if (Type.CanStack) //可以叠加,不是装备,按数量算
            {
                return Type.originalPrice * (int)this.amount;
            }
            else
            {   //装备,不按数量算
                return Type.originalPrice;
            }
        }
    }
    public int sellPrice
    {
        get
        {
            if (Type.CanStack)//可以叠加,不是装备,按数量算
            {
                return (int)(Type.originalPrice * this.amount * GameSetting.SellPriceRate);
            }
            else
            {
                return (int)(Type.originalPrice * GameSetting.SellPriceRate);
            }
        }
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("id", this.id, typeof(int));
        info.AddValue("amount", this.amount, typeof(uint));
    }

    public Item(SerializationInfo info, StreamingContext context)
    {
        this.id = (int)info.GetValue("id", typeof(int));
        this.amount = (uint)info.GetValue("amount", typeof(uint));
        this._type = ItemTypeTable.GetItemType(id);
    }

}


public static class ItemGenerator
{
    //生成物品的档次
    public enum GenItemLvl
    {
        Normal,

    }

    static Item GenItem(int id, uint amount, GenItemLvl level = GenItemLvl.Normal)
    {
        Item item = null;
        ItemType type = ItemTypeTable.GetItemType(id);
        if (type != null)
        {
            if (type.weaponId != 0)
            {
                item = new Item(type, amount);
            }
        }
        return item;
    }
}
