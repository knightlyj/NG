using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public partial class Player : Entity
{
    //属性改变事件
    public delegate void PropChanged(Player p);
    public event PropChanged PropChangedEvent = null;

    //物品栏
    public const int packSize = 4 * 10;
    Item[] _package = null;
    public Item[] Package { get { return this._package; } }
    public Item mouseItem = null; //鼠标上拖拽时有一个格子
    public Item trashItem = null; //垃圾箱有一个格子
    //---------------------初始化
    void PackageInit()
    {
        if(isLocal)
            _package = new Item[packSize];

        ArmorInit();
        WeaponInit();
    }

    //-----------------捡东西的逻辑,先看是否可以叠加,不能叠加则放入新的空位
    public bool PackPickUpItem(Item item)
    {
        if (item.Type.IsMaterail || item.Type.IsConsumable)
        {
            for(int i = 0; i < packSize; i++)
            {
                if(Package[i].Type == item.Type)
                {
                    Package[i].amount += item.amount;
                    return true;
                }
            }
        }

        //到这里,要么没找到堆叠物品,要么不可以堆叠,则找个空位放入
        for (int i = 0; i < packSize; i++)
        {
            if(Package[i] == null)
            {
                Package[i] = item;
                return true;
            }
        }
       
        return false; //背包满了
    }

    //把物品放入指定格子,如果之前这个格子有物品,则在输出参数preItem中范围
    public bool PackPutInItem(Item item, int idx, out Item preItem)
    {
        preItem = null;
        if (idx < 0 || idx >= packSize)
            return false;

        if(Package[idx] != null)
        {
            preItem = Package[idx];
        }
        Package[idx] = item;

        return true;
    }

    //根据索引移除物品 
    public bool PackRemoveItem(int idx)
    {
        if (idx < 0 || idx >= packSize)
            return false;

        Package[idx] = null;

        return true;
    }

    //--------------根据item id和数量移除物品,材料和消耗品,才可以用这个接口
    public bool PackageRemoveAmount(ItemType type, uint amount)
    {
        int[] idcs;
        uint total = PackageItemAmount(type, out idcs);
        if (total < amount) //物品不够
            return false;

        //物品数量足够,挨个减去移除量
        uint a = amount;
        foreach(int idx in idcs)
        {
            if(idx >= 0 && a > 0)
            {
                if(Package[idx].amount <= a)
                {
                    a -= Package[idx].amount; //这个格子里数量不够或者刚好够,则删除这个格子里的物品,并减少相应的总数量
                    Package[idx] = null;
                }
                else
                {
                    Package[idx].amount -= a; //这个格子里的数量已经大于剩下要扣除的数量了,减去就行
                }
            } 
            else if(idx < 0 && a > 0)
            {   //之前有判断数量是足够的,不应该运行到这里,报错,并假装已扣除足够数量,继续运行
                Debug.LogError("PackageRemoveAmount >> amount error");
                return true;
            }
            else //这里只有 a <= 0的情况了,说明已扣除足够数量,跳出循环即可
            {
                break;
            }
        }

        return true;    
    }

    public bool PackageItemEnough(ItemType type, uint amount)
    {
        int[] idcs;
        uint total = PackageItemAmount(type, out idcs);
        return total >= amount;
    }

    //获取物品数量,会输出所有物品的索引
    public uint PackageItemAmount(ItemType type, out int[] indices)
    {
        int[] idcs = PackageFindIdcs(type);
        indices = idcs;
        if (idcs[0] < 0) //没找到,则是0个
            return 0;

        uint a = 0;
        for (int i = 0; i < idcs.Length; i++)
        {
            if (idcs[i] < 0)
                break;
            a += Package[idcs[i]].amount;
        }

        return a;
    }

    //根据索引移除物品
    public bool PackageRemoveIdx(int idx)
    {
        if (idx < 0 || idx >= packSize)
            return false;

        Package[idx] = null;
        return true;   
    }


    //-------------------根据type找到物品的全部索引
    public int[] PackageFindIdcs(ItemType type)
    {
        int[] found = new int[packSize];
        for (int i = 0; i < packSize; i++)
            found[i] = -1;
        int cnt = 0;
        for (int i = 0; i < packSize; i++)
        {
            if(Package[i].Type == type)
            {
                found[cnt++] = i;
            }
        }
        return found;
    }

    //-----------------根据索引找到物品信息
    public Item PackageFindItem(int idx)
    {
        if (idx < 0 || idx >= packSize)
            return null;

        return Package[idx];
    }

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

    void RaisePropChanged()
    {
        if (this.PropChangedEvent != null)
            PropChangedEvent(this);
    }
}
