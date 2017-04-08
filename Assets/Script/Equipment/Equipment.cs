using UnityEngine;
using System.Collections;

//***********************************************
//                  护甲
//***********************************************
public enum ArmorType
{
    Nothing,
    Armor,
    Helmet,
}
//护甲属性
public class ArmorProperties
{
    //装备类型
    public ArmorType armorType;

    public int hpBonus = 0;
    public int minAtkBonus = 0;
    public int maxAtkBonus = 0;
    public int defBonus = 0;
    public float spdScaleBonus = 0;
    public float jmpScaleBonus = 0;
    public float atkSpdBonus = 0;
    public float crtlChanceBonus = 0;
    public float crtlRateBonus = 0;
    public float cdrBonus = 0;
    public float rcrBonus = 0;

    public string sprite;
}

public class Armor : Item {
    ArmorProperties _properties = new ArmorProperties();
    public ArmorProperties Properties { get { return this._properties; } }
    
    public Armor(ItemType type) : base(type, 1)
    {

    }
}

//***********************************************
//                  武器
//***********************************************
//public enum GunType
//{
//    Pistol, //手枪
//    AutomaticRifle, //自动步枪
//    SniperRifle, //狙击步枪
//    Laucher, //发射器
//}
//武器属性
public struct WeaponProperties
{
    public int minAtkBonus; //攻击
    public int maxAtkBonus;
    public float atkInterval; //攻击间隔
    public float crtlChanceBonus; //暴击几率
    public float crtlRateBonus; //暴击倍数
    public float rcrBonus; //资源消耗减少

    public uint maxAmmo; //最大弹药
    public uint curAmmor; //当前弹药

    public string sprite;
}

public class Weapon : Item
{
    WeaponProperties _properties = new WeaponProperties();
    public WeaponProperties Properties { get { return this._properties; } }

    public Weapon(ItemType type) : base(type, 1)
    {

    }
}
