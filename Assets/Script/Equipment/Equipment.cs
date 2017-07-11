using UnityEngine;
using System.Collections;
using System;

//***********************************************
//                  护甲
//***********************************************
public enum ArmorType
{
    Helmet, //头盔
    Armor, //护甲
    Glove, //手套
    Boot, //鞋子
    Accessory0, //饰品0
    Accessory1, //饰品1
    Accessory2, //饰品2
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
    public float rcrBonus = 0;

    public string appearance;
}

//***********************************************
//                  武器
//***********************************************
//武器属性
public class WeaponProperties
{
    public int minAtkBonus; //攻击
    public int maxAtkBonus;
    public float atkInterval; //攻击间隔
    public float crtlChanceBonus; //暴击几率
    public float crtlRateBonus; //暴击倍数
    public uint ammoCapacity; //弹药容量
    public float rcrBonus; //资源消耗减少

    public string appearance;
}

public class PlayerEquipment
{
    public PlayerEquipment()
    {
        ArmorInit();
        WeaponInit();
    }
    ////////////////////////////////////////////////////////
    //******************装备栏*****************************
    ////////////////////////////////////////////////////
    Item[] _armors = null;
    public Item[] Armors { get { return this._armors; } }
    void ArmorInit()
    {
        Array a = Enum.GetValues(typeof(ArmorType));
        _armors = new Item[a.Length];
    }

    //穿上装备,如果之前格子里有装备,会移除掉
    public bool PutOnArmor(Item armor, out Item preArmor)
    {
        preArmor = null;
        //int idx = (int)armor.Properties.armorType;

        //preArmor = Armors[idx];
        //Armors[idx] = armor; //加入已装备护甲列表

        return true;
    }

    //脱下装备
    public Item TakeOffArmor(ArmorType type)
    {
        int idx = (int)type;
        Item a = Armors[idx];
        Armors[idx] = null; //从已装备列表中移除

        return a;
    }

    void AddArmorProp(Item a)
    {
        //prop.maxHp += a.Properties.hpBonus; //生命上限
        //prop.minAttack += a.Properties.minAtkBonus; //最小攻击
        //prop.maxAttack += a.Properties.maxAtkBonus; //最大攻击
        //prop.defense += a.Properties.defBonus; //防御
        //prop.speedScale += a.Properties.spdScaleBonus; //速度
        //prop.jumpScale += a.Properties.jmpScaleBonus; //跳跃
        //prop.atkSpeed += a.Properties.atkSpdBonus; //攻速
        //prop.criticalChance += a.Properties.crtlChanceBonus; //暴击几率
        //prop.criticalRate += a.Properties.crtlRateBonus; //暴击伤害

        RaiseEquipChanged();
    }

    void RemoveArmorProp(Item a)
    {
        //减掉装备加成
        //prop.maxHp -= a.Properties.hpBonus; //生命上限
        //if (prop.hp > prop.maxHp)
        //    prop.hp = prop.maxHp;

        //prop.minAttack -= a.Properties.minAtkBonus; //最小攻击
        //prop.maxAttack -= a.Properties.maxAtkBonus; //最大攻击
        //prop.defense -= a.Properties.defBonus; //防御
        //prop.speedScale -= a.Properties.spdScaleBonus; //速度
        //prop.jumpScale -= a.Properties.jmpScaleBonus; //跳跃

        //prop.atkSpeed -= a.Properties.atkSpdBonus; //攻速
        //prop.criticalChance -= a.Properties.crtlChanceBonus; //暴击几率
        //prop.criticalRate -= a.Properties.crtlRateBonus; //暴击伤害

        RaiseEquipChanged();
    }

    void ShowArmor(ArmorType type, string sprite)
    {

    }

    ////////////////////////////////////////////////////////
    //******************武器栏*****************************
    //由于可以切换武器,武器属性不直接加入,使用的时候再计算
    ////////////////////////////////////////////////////
    public const int weaponAmount = 4;
    Item[] _weapons = null;
    public Item[] Weapons { get { return this._weapons; } }
    int curWeaponIdx = 0;
    public Item CurWeapon { get { return Weapons[curWeaponIdx]; } }

    void WeaponInit()
    {
        _weapons = new Item[weaponAmount];
    }

    public bool PutOnWeapon(Item weapon, int idx, out Item preWeapon)
    {
        preWeapon = null;
        if (idx < 0 || idx >= weaponAmount || weapon == null)
            return false;

        //记下原来的武器
        if (_weapons[idx] != null)
            preWeapon = _weapons[idx];

        //装备新武器
        _weapons[idx] = weapon;

        RaiseEquipChanged();
        return true;
    }

    public Item TakeOffWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return null;

        Item w = Weapons[idx]; //取下武器

        Weapons[idx] = null; //从已装武器列表中移除

        RaiseEquipChanged();
        return w;
    }

    void AddWeaponProp(Item w)
    {
        //prop.minAttack += w.Properties.minAtkBonus; //最小攻击
        //prop.maxAttack += w.Properties.maxAtkBonus; //最大攻击
        //prop.atkInterval = w.Properties.atkInterval; //攻击间隔
        //prop.criticalChance += w.Properties.crtlChanceBonus; //暴击几率
        //prop.criticalRate += w.Properties.crtlRateBonus; //暴击伤害

        RaiseEquipChanged();
    }

    void RemoveWeaponProp(Item w)
    {
        //prop.minAttack += w.Properties.minAtkBonus; //最小攻击
        //prop.maxAttack += w.Properties.maxAtkBonus; //最大攻击
        //prop.atkInterval = w.Properties.atkInterval; //攻击间隔
        //prop.criticalChance += w.Properties.crtlChanceBonus; //暴击几率
        //prop.criticalRate += w.Properties.crtlRateBonus; //暴击伤害

        RaiseEquipChanged();
    }

    //bool SwitchWeapon(int idx)
    //{
    //    if (idx < 0 || idx >= weaponAmount)
    //        return false;

    //    if (Weapons[curWeaponIdx] != null)
    //        RemoveWeaponProp(Weapons[curWeaponIdx], this.Properties);

    //    if (Weapons[idx] != null)
    //        AddWeaponProp(Weapons[idx], this.Properties);

    //    curWeaponIdx = idx;

    //    return true;
    //}

    //显示某一把武器
    void ShowWeapon(string spriteName)
    {

    }

    //根据装备计算属性
    EntityProperties CalcProperties()
    {
        if (Armors == null)
        {
            Debug.LogError("Player.CalcProperties >> Armors is null");
            return null;
        }
        EntityProperties newPorp = new EntityProperties();
        newPorp.Reset();
        foreach (Item a in Armors)
        {
            //newPorp.maxHp += a.Properties.hpBonus; //生命上限
            //newPorp.minAttack += a.Properties.minAtkBonus; //最小攻击
            //newPorp.maxAttack += a.Properties.maxAtkBonus; //最大攻击
            //newPorp.defense += a.Properties.defBonus; //防御
            //newPorp.speedScale += a.Properties.spdScaleBonus; //速度
            //newPorp.jumpScale += a.Properties.jmpScaleBonus; //跳跃
            //newPorp.atkSpeed += a.Properties.atkSpdBonus; //攻速
            //newPorp.criticalChance += a.Properties.crtlChanceBonus; //暴击几率
            //newPorp.criticalRate += a.Properties.crtlRateBonus; //暴击伤害
        }
        return newPorp;
    }

    //装备改变事件
    public delegate void EquipChanged();
    public event EquipChanged EquipChangedEvent = null;
    void RaiseEquipChanged()
    {
        if (this.EquipChangedEvent != null)
            EquipChangedEvent();
    }
}

