using UnityEngine;
using System.Collections;

public partial struct ItemType
{
    //木头
    public static ItemType Wood
    {
        get
        {
            ItemType i = new ItemType(Type.Wood, 1 << materialOffset);
            return i;
        }
    }

    //铁
    public static ItemType Iron
    {
        get
        {
            ItemType i = new ItemType(Type.Iron, 1 << materialOffset);
            return i;
        }
    }

    //铜
    public static ItemType Copper
    {
        get
        {
            ItemType i = new ItemType(Type.Copper, 1 << materialOffset);
            return i;
        }
    }

    //金
    public static ItemType Gold
    {
        get
        {
            ItemType i = new ItemType(Type.Gold, 1 << materialOffset);
            return i;
        }
    }

    //测试枪
    public static ItemType TestGun
    {
        get
        {
            ItemType i = new ItemType(Type.TestGun, 1 << weaponOffset);
            return i;
        }
    }

    //测试血瓶
    public static ItemType TestHealthPotion
    {
        get
        {
            ItemType i = new ItemType(Type.TestHealthPotion, 1 << consumableOffset);
            return i;
        }
    }
}