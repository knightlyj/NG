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

public class RawMaterial
{
    public int id;
    public uint amount;
}

[Serializable]
public class ItemType// : UnityEngine.Object
{
    public int id; //id
    public string itemName; //名字,要与资源文件夹的名字一致
    public ItemQuality quality; //品质,决定物品名的颜色
    public string comment; //注释
    public int equipId = 0; //装备id,为0时表示不是装备
    public int consumId = 0; //消耗品id,为0时表示不是消耗品

    //public bool IsMaterial { get { return (typeBit & materialOffset) != 0; } }
    public bool IsConsumable { get { return consumId != 0; } }
    public bool IsEquipment { get { return equipId != 0; } }

    public bool CanCraft { get { return false; } }
    public bool CanStack { get { return equipId == 0; } }

    public const int maxRawMatSorts = 6; //最多6种材料
    public RawMaterial[] rawMats = new RawMaterial[maxRawMatSorts]; //按表格填写位置存放
    public int craftOutput = 0;

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



public class Item
{
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
