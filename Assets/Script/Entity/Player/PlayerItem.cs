using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public partial class Player : Entity
{
    public const int itemPackSize = 40;
    PlayerPackage _playerPack = new PlayerPackage(itemPackSize);
    public PlayerPackage playerPack { get { return this._playerPack; } }

    ////////////////////////////////////////////////////////
    //******************装备栏*****************************
    ////////////////////////////////////////////////////
    Armor[] _armors = null;
    public Armor[] Armors { get { return this._armors; } }
    void ArmorInit()
    {
        Array a = Enum.GetValues(typeof(ArmorType));
        _armors = new Armor[a.Length];
    }

    Armor GetArmor(ArmorType type)
    {
        return Armors[(int)type];
    }

    //穿上装备,如果之前格子里有装备,会移除掉
    bool PutOnArmor(Armor a)
    {
        int idx = (int)a.Properties.armorType;
        if(Armors[idx] != null) //原来如果有装备,则移除属性
        {
            RemoveArmorProp(Armors[idx], this.Properties);
        }
        AddArmorProp(a, this.Properties); //加上新装备的属性

        Armors[idx] = a; //加入已装备护甲列表

        ShowArmor(a.Properties.armorType, a.Properties.sprite); //显示新装备

        return true;
    }

    //脱下装备
    bool PutOffArmor(ArmorType type)
    {
        int idx = (int)type;
        if(Armors[idx] != null) 
        {   //移除属性
            RemoveArmorProp(Armors[idx], this.Properties);
        }
        Armors[idx] = null; //从已装备列表中移除

        ShowArmor(type, "default"); //显示默认装备

        return true;
    }
    
    void AddArmorProp(Armor a, EntityProperties prop)
    {
        prop.maxHp += a.Properties.hpBonus; //生命上限
        prop.minAttack += a.Properties.minAtkBonus; //最小攻击
        prop.maxAttack += a.Properties.maxAtkBonus; //最大攻击
        prop.defense += a.Properties.defBonus; //防御
        prop.speedScale += a.Properties.spdScaleBonus; //速度
        prop.jumpScale += a.Properties.jmpScaleBonus; //跳跃
        prop.atkSpeed += a.Properties.atkSpdBonus; //攻速
        prop.criticalChance += a.Properties.crtlChanceBonus; //暴击几率
        prop.criticalRate += a.Properties.crtlRateBonus; //暴击伤害

        RaisePropChanged();
    }

    void RemoveArmorProp(Armor a, EntityProperties prop)
    {
        //减掉装备加成
        prop.maxHp -= a.Properties.hpBonus; //生命上限
        if (prop.hp > prop.maxHp)
            prop.hp = prop.maxHp;

        prop.minAttack -= a.Properties.minAtkBonus; //最小攻击
        prop.maxAttack -= a.Properties.maxAtkBonus; //最大攻击
        prop.defense -= a.Properties.defBonus; //防御
        prop.speedScale -= a.Properties.spdScaleBonus; //速度
        prop.jumpScale -= a.Properties.jmpScaleBonus; //跳跃

        prop.atkSpeed -= a.Properties.atkSpdBonus; //攻速
        prop.criticalChance -= a.Properties.crtlChanceBonus; //暴击几率
        prop.criticalRate -= a.Properties.crtlRateBonus; //暴击伤害

        RaisePropChanged();
    }

    void ShowArmor(ArmorType type, string sprite)
    {

    }

    ////////////////////////////////////////////////////////
    //******************武器栏*****************************
    //由于可以切换武器,武器属性不直接加入,使用的时候再计算
    ////////////////////////////////////////////////////
    public const int weaponAmount = 4;
    Weapon[] _weapons = null;
    public Weapon[] Weapons { get { return this._weapons; } }
    int curWeaponIdx = 0;
    public Weapon CurWeapon { get { return Weapons[curWeaponIdx]; } }

    void WeaponInit()
    {
        _weapons = new Weapon[weaponAmount];
    }

    bool PutOnWeapon(Weapon w, int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return false;

        if(idx == curWeaponIdx) //更换的当前使用武器
        {
            if(Weapons[idx] != null)  //如果原来格子有武器,则移除附加属性
                RemoveWeaponProp(Weapons[idx], this.Properties);

            AddWeaponProp(w, this.Properties); //加上新武器的属性
            ShowWeapon(w.Properties.sprite); //显示新武器的外观
        }

        Weapons[idx] = w; //存入已装备武器列表

        return true;
    }

    bool PutOffWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return false;

        if(idx == curWeaponIdx)
        { //如果是当前装备,则移除属性,并显示默认武器
            if (Weapons[idx] != null) 
                RemoveWeaponProp(Weapons[idx], this.Properties);

            ShowWeapon("default");
        }

        Weapons[idx] = null; //从已装武器列表中移除

        return true;
    }

    void AddWeaponProp(Weapon w, EntityProperties prop)
    {
        prop.minAttack += w.Properties.minAtkBonus; //最小攻击
        prop.maxAttack += w.Properties.maxAtkBonus; //最大攻击
        prop.atkInterval = w.Properties.atkInterval; //攻击间隔
        prop.criticalChance += w.Properties.crtlChanceBonus; //暴击几率
        prop.criticalRate += w.Properties.crtlRateBonus; //暴击伤害

        RaisePropChanged();
    }

    void RemoveWeaponProp(Weapon w, EntityProperties prop)
    {
        prop.minAttack += w.Properties.minAtkBonus; //最小攻击
        prop.maxAttack += w.Properties.maxAtkBonus; //最大攻击
        prop.atkInterval = w.Properties.atkInterval; //攻击间隔
        prop.criticalChance += w.Properties.crtlChanceBonus; //暴击几率
        prop.criticalRate += w.Properties.crtlRateBonus; //暴击伤害

        RaisePropChanged();
    }

    bool SwitchWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return false;

        if (Weapons[curWeaponIdx] != null)
            RemoveWeaponProp(Weapons[curWeaponIdx], this.Properties);

        if (Weapons[idx] != null)
            AddWeaponProp(Weapons[idx], this.Properties);

        curWeaponIdx = idx;

        return true;
    }

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
        foreach (Armor a in Armors)
        {
            newPorp.maxHp += a.Properties.hpBonus; //生命上限
            newPorp.minAttack += a.Properties.minAtkBonus; //最小攻击
            newPorp.maxAttack += a.Properties.maxAtkBonus; //最大攻击
            newPorp.defense += a.Properties.defBonus; //防御
            newPorp.speedScale += a.Properties.spdScaleBonus; //速度
            newPorp.jumpScale += a.Properties.jmpScaleBonus; //跳跃
            newPorp.atkSpeed += a.Properties.atkSpdBonus; //攻速
            newPorp.criticalChance += a.Properties.crtlChanceBonus; //暴击几率
            newPorp.criticalRate += a.Properties.crtlRateBonus; //暴击伤害
        }
        return newPorp;
    }

    //属性改变事件
    public delegate void PropChanged(Player p);
    public event PropChanged PropChangedEvent = null;
    void RaisePropChanged()
    {
        if (this.PropChangedEvent != null)
            PropChangedEvent(this);
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
