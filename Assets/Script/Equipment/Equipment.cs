using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;

//***********************************************
//                  护甲
//***********************************************
public enum ArmorType
{
    Helmet, //头盔
    Armor, //护甲
    Boot, //鞋子
    Glove, //手套
    Accessory, //饰品0
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
public enum GunType
{
    Normal,
    Projectile,

}
//武器属性
public class WeaponProperties
{
    public GunType gunType;

    public int minAtkBonus; //攻击
    public int maxAtkBonus;
    public float atkInterval; //攻击间隔
    public float crtlChanceBonus; //暴击几率
    public float crtlRateBonus; //暴击倍数
    public uint ammoCapacity; //弹药容量
    public float rcrBonus; //资源消耗减少
    public float recoil; //后坐力
    public float beatBack; //击退

    public string appearance;
}

[Serializable]
public class PlayerEquipment : ISerializable
{
    public PlayerEquipment(EntityProperties bindProp)
    {
        bindProperties = bindProp;
        ArmorInit();
        WeaponInit();
    }
    EntityProperties bindProperties = null;
    ////////////////////////////////////////////////////////
    //******************装备栏*****************************
    ////////////////////////////////////////////////////
    Item[] _armors = null;
    public static readonly ArmorType[] armorTypes = {ArmorType.Helmet, ArmorType.Armor, ArmorType.Boot, ArmorType.Glove,
                        ArmorType.Accessory, ArmorType.Accessory, ArmorType.Accessory };
    public Item[] Armors { get { return this._armors; } }
    void ArmorInit()
    {
        _armors = new Item[armorTypes.Length];
    }

    //根据类型穿装备,饰品就放在第一个格子里
    public bool PutOnArmor(Item armor, out Item preArmor)
    {
        preArmor = null;
        if (armor != null && armor.Type.IsArmor)
        {
            ArmorProperties armorProp = EquipTable.GetArmorProp(armor.Type.armorId);
            for (int i = 0; i < armorTypes.Length; i++)
            {
                if (armorProp.armorType == armorTypes[i])
                {
                    PutOnArmor(armor, i, out preArmor);
                    return true;
                }
            }
        }
        return false;
    }

    //穿上装备,如果之前格子里有装备,会移除掉
    public bool PutOnArmor(Item armor, int idx, out Item preArmor)
    {
        preArmor = null;
        if (armor == null || !armor.Type.IsArmor)
            return false;
        ArmorProperties armorProp = EquipTable.GetArmorProp(armor.Type.armorId);
        if (armorProp != null && armorProp.armorType == armorTypes[idx])
        {  //存在装备配置,且类型相同
            preArmor = Armors[idx];
            Armors[idx] = armor; //加入已装备护甲列表

            if (preArmor != null)
            {
                ArmorProperties preProp = EquipTable.GetArmorProp(preArmor.Type.armorId);
                if (preProp != null)
                    RemoveArmorProp(bindProperties, preProp);
            }
            AddArmorProp(bindProperties, armorProp);
            RaiseEquipChanged();
        }
        return true;
    }

    //脱下装备
    public Item TakeOffArmor(int idx)
    {
        Item preArmor = Armors[idx];
        Armors[idx] = null; //从已装备列表中移除

        //修改属性
        if (preArmor != null)
        {
            ArmorProperties preProp = EquipTable.GetArmorProp(preArmor.Type.armorId);
            if (preProp != null)
                RemoveArmorProp(bindProperties, preProp);
        }
        RaiseEquipChanged();
        return preArmor;
    }

    //加上装备属性
    void AddArmorProp(EntityProperties playerProp, ArmorProperties armorProp)
    {
        if (playerProp != null && armorProp != null)
        {
            playerProp.maxHp += armorProp.hpBonus; //生命上限
            if (playerProp.hp > playerProp.maxHp)
                playerProp.hp = playerProp.maxHp;
            playerProp.minAttack += armorProp.minAtkBonus; //最小攻击
            playerProp.maxAttack += armorProp.maxAtkBonus; //最大攻击
            playerProp.defense += armorProp.defBonus; //防御
            playerProp.speedScale += armorProp.spdScaleBonus; //速度
            playerProp.jumpScale += armorProp.jmpScaleBonus; //跳跃
            playerProp.atkSpeed += armorProp.atkSpdBonus; //攻速
            playerProp.criticalChance += armorProp.crtlChanceBonus; //暴击几率
            playerProp.criticalRate += armorProp.crtlRateBonus; //暴击伤害
            playerProp.rcr += armorProp.rcrBonus; //rcr

        }
    }

    //移除装备属性
    void RemoveArmorProp(EntityProperties playerProp, ArmorProperties armorProp)
    {
        if (playerProp != null && armorProp != null)
        {
            //减掉装备加成
            playerProp.maxHp -= armorProp.hpBonus; //生命上限
            if (playerProp.hp > playerProp.maxHp)
                playerProp.hp = playerProp.maxHp;

            playerProp.minAttack -= armorProp.minAtkBonus; //最小攻击
            playerProp.maxAttack -= armorProp.maxAtkBonus; //最大攻击
            playerProp.defense -= armorProp.defBonus; //防御
            playerProp.speedScale -= armorProp.spdScaleBonus; //速度
            playerProp.jumpScale -= armorProp.jmpScaleBonus; //跳跃

            playerProp.atkSpeed -= armorProp.atkSpdBonus; //攻速
            playerProp.criticalChance -= armorProp.crtlChanceBonus; //暴击几率
            playerProp.criticalRate -= armorProp.crtlRateBonus; //暴击伤害
            playerProp.rcr -= armorProp.rcrBonus; //rcr

        }
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

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="idx"></param>
    /// <param name="preWeapon"></param>
    /// <returns></returns>
    public bool PutOnWeapon(Item weapon, int idx, out Item preWeapon)
    {
        preWeapon = null;
        if (idx < 0 || idx >= weaponAmount || weapon == null || !weapon.Type.IsWeapon)
            return false;

        WeaponProperties weaponProp = EquipTable.GetWeaponProp(weapon.Type.weaponId);
        if (weaponProp != null)
        {
            //记下原来的武器
            if (_weapons[idx] != null)
                preWeapon = _weapons[idx];

            //装备新武器
            _weapons[idx] = weapon;

            if (idx == curWeaponIdx) //如果切换的是当前使用武器,则要计算属性
            {
                if (preWeapon != null)
                {//之前格子里有武器,则移除属性
                    WeaponProperties preProp = EquipTable.GetWeaponProp(preWeapon.Type.weaponId);
                    if (preProp != null)
                        RemoveWeaponProp(bindProperties, preProp);
                }
                //加上新武器的属性
                AddWeaponProp(bindProperties, weaponProp);
            }

            RaiseEquipChanged();
        }
        return true;
    }

    /// <summary>
    /// 取下武器
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public Item TakeOffWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return null;

        Item preWeapon = Weapons[idx]; //之前装备的武器

        Weapons[idx] = null; //从已装武器列表中移除
        if (idx == curWeaponIdx && preWeapon != null)
        {
            WeaponProperties weaponProp = EquipTable.GetWeaponProp(preWeapon.Type.weaponId);
            if (weaponProp != null)
                RemoveWeaponProp(bindProperties, weaponProp);
        }

        RaiseEquipChanged();
        return preWeapon;
    }

    /// <summary>
    /// 切换武器
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    bool SwitchWeapon(int idx)
    {
        if (idx < 0 || idx >= weaponAmount)
            return false;

        //移除当前使用武器的属性
        if (_weapons[curWeaponIdx] != null)
        {
            WeaponProperties weaponProp = EquipTable.GetWeaponProp(_weapons[curWeaponIdx].Type.weaponId);
            if (weaponProp != null)
                RemoveWeaponProp(bindProperties, weaponProp);
        }

        //添加要使用的武器的属性
        if (Weapons[idx] != null)
        {
            WeaponProperties weaponProp = EquipTable.GetWeaponProp(_weapons[idx].Type.weaponId);
            if (weaponProp != null)
                AddWeaponProp(bindProperties, weaponProp);
        }

        curWeaponIdx = idx;

        return true;
    }

    void AddWeaponProp(EntityProperties playerProp, WeaponProperties weaponProp)
    {
        if (playerProp != null && weaponProp != null)
        {
            playerProp.minAttack += weaponProp.minAtkBonus; //最小攻击
            playerProp.maxAttack += weaponProp.maxAtkBonus; //最大攻击
            playerProp.atkInterval = weaponProp.atkInterval; //攻击间隔
            playerProp.recoil = weaponProp.recoil; //后坐力
            playerProp.knockBack = weaponProp.beatBack;//击退
            playerProp.criticalChance += weaponProp.crtlChanceBonus; //暴击几率
            playerProp.criticalRate += weaponProp.crtlRateBonus; //暴击伤害
            playerProp.rcr += weaponProp.rcrBonus; //rcr
        }
    }

    void RemoveWeaponProp(EntityProperties playerProp, WeaponProperties weaponProp)
    {
        if (playerProp != null && weaponProp != null)
        {
            playerProp.minAttack -= weaponProp.minAtkBonus; //最小攻击
            playerProp.maxAttack -= weaponProp.maxAtkBonus; //最大攻击
            playerProp.atkInterval = 0.5f; //攻击间隔
            playerProp.recoil = 0; //后坐力
            playerProp.knockBack = 0; //击退
            playerProp.criticalChance -= weaponProp.crtlChanceBonus; //暴击几率
            playerProp.criticalRate -= weaponProp.crtlRateBonus; //暴击伤害
            playerProp.rcr -= weaponProp.rcrBonus; //rcr
        }
    }



    //显示某一把武器
    void ShowWeapon(string spriteName)
    {

    }

    //重新计算数值
    public void RecalcProperties()
    {
        if (bindProperties != null)
        {
            bindProperties.Reset();
            foreach (Item armor in Armors)
            {
                if (armor != null) {
                    ArmorProperties armorProp = EquipTable.GetArmorProp(armor.Type.armorId);
                    AddArmorProp(bindProperties, armorProp);
                }
            }
            Item curWeapon = CurWeapon;
            if(curWeapon != null)
            {
                WeaponProperties weaponProp = EquipTable.GetWeaponProp(curWeapon.Type.weaponId);
                AddWeaponProp(bindProperties, weaponProp);
            }
        }
    }

    //装备改变事件
    public delegate void EquipChanged();
    public event EquipChanged EquipChangedEvent = null;
    void RaiseEquipChanged()
    {
        if (this.EquipChangedEvent != null)
            EquipChangedEvent();
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("armor", this._armors, typeof(Item[]));
        info.AddValue("weapon", this._weapons, typeof(Item[]));
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public PlayerEquipment(SerializationInfo info, StreamingContext context)
    {
        this._armors = (Item[])info.GetValue("armor", typeof(Item[]));
        this._weapons = (Item[])info.GetValue("weapon", typeof(Item[]));
    }

    public void BindProperties(EntityProperties prop)
    {
        this.bindProperties = prop;
    }
}

