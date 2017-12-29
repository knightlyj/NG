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
    public PlayerEquipment(Player bindPlayer)
    {
        this.bindPlayer = bindPlayer;
        ArmorInit();
        WeaponInit();
    }
    Player bindPlayer = null;
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
                    RemoveArmorProp(bindPlayer, preProp);
            }
            AddArmorProp(bindPlayer, armorProp);
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
                RemoveArmorProp(bindPlayer, preProp);
        }
        RaiseEquipChanged();
        return preArmor;
    }

    //加上装备属性
    void AddArmorProp(Player player, ArmorProperties armorProp)
    {
        if (player != null && armorProp != null)
        {
            player.maxHp += armorProp.hpBonus; //生命上限
            if (player.hp > player.maxHp)
                player.hp = player.maxHp;
            player.minAttack += armorProp.minAtkBonus; //最小攻击
            player.maxAttack += armorProp.maxAtkBonus; //最大攻击
            player.defense += armorProp.defBonus; //防御
            player.speedScale += armorProp.spdScaleBonus; //速度
            player.jumpScale += armorProp.jmpScaleBonus; //跳跃
            player.atkSpeed += armorProp.atkSpdBonus; //攻速
            player.criticalChance += armorProp.crtlChanceBonus; //暴击几率
            player.criticalRate += armorProp.crtlRateBonus; //暴击伤害
            player.rcr += armorProp.rcrBonus; //rcr

        }
    }

    //移除装备属性
    void RemoveArmorProp(Player player, ArmorProperties armorProp)
    {
        if (player != null && armorProp != null)
        {
            //减掉装备加成
            player.maxHp -= armorProp.hpBonus; //生命上限
            if (player.hp > player.maxHp)
                player.hp = player.maxHp;

            player.minAttack -= armorProp.minAtkBonus; //最小攻击
            player.maxAttack -= armorProp.maxAtkBonus; //最大攻击
            player.defense -= armorProp.defBonus; //防御
            player.speedScale -= armorProp.spdScaleBonus; //速度
            player.jumpScale -= armorProp.jmpScaleBonus; //跳跃

            player.atkSpeed -= armorProp.atkSpdBonus; //攻速
            player.criticalChance -= armorProp.crtlChanceBonus; //暴击几率
            player.criticalRate -= armorProp.crtlRateBonus; //暴击伤害
            player.rcr -= armorProp.rcrBonus; //rcr

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
                        RemoveWeaponProp(bindPlayer, preProp);
                }
                //加上新武器的属性
                AddWeaponProp(bindPlayer, weaponProp);
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
                RemoveWeaponProp(bindPlayer, weaponProp);
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
                RemoveWeaponProp(bindPlayer, weaponProp);
        }

        //添加要使用的武器的属性
        if (Weapons[idx] != null)
        {
            WeaponProperties weaponProp = EquipTable.GetWeaponProp(_weapons[idx].Type.weaponId);
            if (weaponProp != null)
                AddWeaponProp(bindPlayer, weaponProp);
        }

        curWeaponIdx = idx;

        return true;
    }

    void AddWeaponProp(Player player, WeaponProperties weaponProp)
    {
        if (player != null && weaponProp != null)
        {
            player.minAttack += weaponProp.minAtkBonus; //最小攻击
            player.maxAttack += weaponProp.maxAtkBonus; //最大攻击
            player.atkInterval = weaponProp.atkInterval; //攻击间隔
            player.recoil = weaponProp.recoil; //后坐力
            player.knockBack = weaponProp.beatBack;//击退
            player.criticalChance += weaponProp.crtlChanceBonus; //暴击几率
            player.criticalRate += weaponProp.crtlRateBonus; //暴击伤害
            player.rcr += weaponProp.rcrBonus; //rcr
        }
    }

    void RemoveWeaponProp(Player player, WeaponProperties weaponProp)
    {
        if (player != null && weaponProp != null)
        {
            player.minAttack -= weaponProp.minAtkBonus; //最小攻击
            player.maxAttack -= weaponProp.maxAtkBonus; //最大攻击
            player.atkInterval = 0.5f; //攻击间隔
            player.recoil = 0; //后坐力
            player.knockBack = 0; //击退
            player.criticalChance -= weaponProp.crtlChanceBonus; //暴击几率
            player.criticalRate -= weaponProp.crtlRateBonus; //暴击伤害
            player.rcr -= weaponProp.rcrBonus; //rcr
        }
    }



    //显示某一把武器
    void ShowWeapon(string spriteName)
    {

    }

    //重新计算数值
    public void RecalcProperties()
    {
        if (bindPlayer != null)
        {
            bindPlayer.ResetProperties();
            foreach (Item armor in Armors)
            {
                if (armor != null)
                {
                    ArmorProperties armorProp = EquipTable.GetArmorProp(armor.Type.armorId);
                    AddArmorProp(bindPlayer, armorProp);
                }
            }
            Item curWeapon = CurWeapon;
            if (curWeapon != null)
            {
                WeaponProperties weaponProp = EquipTable.GetWeaponProp(curWeapon.Type.weaponId);
                AddWeaponProp(bindPlayer, weaponProp);
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

    public void BindPlayer(Player player)
    {
        this.bindPlayer = player;
    }
}

