using UnityEngine;
using System.Collections;
using System.IO;
using System;

public static class EquipTable
{
    const int tableSize = 200;
    static ArmorProperties[] armorPropTable = new ArmorProperties[tableSize];
    static WeaponProperties[] weaponPropTable = new WeaponProperties[tableSize];

    static EquipTable()
    {
        //护甲表
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/GameData/" + TextResources.armorTable, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB18030")))
            {
                string input;
                int count = 0;
                while ((input = sr.ReadLine()) != null && count < tableSize)
                {
                    if (count >= 1) //表格长度限制,且第一行为标题栏
                    {
                        armorPropTable[count] = ParseArmor(input);
                    }
                    count++;
                }
                sr.Close();
            }
            fs.Close();
        }

        //武器表
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/GameData/" + TextResources.weaponTable, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB18030")))
            {
                string input;
                int count = 0;
                while ((input = sr.ReadLine()) != null && count < tableSize)
                {
                    if (count >= 1) //表格长度限制,且第一行为标题栏
                    {
                        weaponPropTable[count] = ParseWeapon(input);
                    }
                    count++;
                }
                sr.Close();
            }
            fs.Close();
        }
    }

    static public ArmorProperties GetArmorProp(int id)
    {
        if (id >= armorPropTable.Length)
            return null;
        return armorPropTable[id];
    }

    static public WeaponProperties GetWeaponProp(int id)
    {
        if (id >= weaponPropTable.Length)
            return null;
        return weaponPropTable[id];
    }


    enum ArmorTableIndex
    {
        Id,
        Type,
        Hp,
        MinAtk,
        MaxAtk,
        Def,
        Spd,
        Jmp,
        AtkSpd,
        CrtChance,
        CrtRate,
        RCR,
    }
    static ArmorProperties ParseArmor(string str)
    {
        ArmorProperties prop = new ArmorProperties();
        string[] cell = null;
        cell = str.Split(',');
        try
        {
            prop.armorType = (ArmorType)Int32.Parse(cell[(int)ArmorTableIndex.Type]);
        }
        catch
        {
            return null;
        }
        //hp
        try{
            prop.hpBonus = Int32.Parse(cell[(int)ArmorTableIndex.Hp]);
        }
        catch
        {
            prop.hpBonus = 0;
        }
        //min atk
        try
        {
            prop.minAtkBonus = Int32.Parse(cell[(int)ArmorTableIndex.MinAtk]);
        }
        catch
        {
            prop.minAtkBonus = 0;
        }
        //max atk
        try
        {
            prop.maxAtkBonus = Int32.Parse(cell[(int)ArmorTableIndex.MaxAtk]);
        }
        catch
        {
            prop.maxAtkBonus = 0;
        }
        //def
        try
        {
            prop.defBonus = Int32.Parse(cell[(int)ArmorTableIndex.Def]);
        }
        catch
        {
            prop.defBonus = 0;
        }
        //spd
        try
        {
            prop.spdScaleBonus = float.Parse(cell[(int)ArmorTableIndex.Spd]) / 100.0f;
        }
        catch
        {
            prop.spdScaleBonus = 0;
        }
        //jmp
        try
        {
            prop.jmpScaleBonus = float.Parse(cell[(int)ArmorTableIndex.Jmp]) / 100.0f;
        }
        catch
        {
            prop.jmpScaleBonus = 0;
        }
        //atk spd
        try
        {
            prop.atkSpdBonus = float.Parse(cell[(int)ArmorTableIndex.AtkSpd]) / 100.0f;
        }
        catch
        {
            prop.atkSpdBonus = 0;
        }
        //critical chance
        try
        {
            prop.crtlChanceBonus = float.Parse(cell[(int)ArmorTableIndex.CrtChance]) / 100.0f;
        }
        catch
        {
            prop.crtlChanceBonus = 0;
        }
        //critical rate
        try
        {
            prop.crtlRateBonus = float.Parse(cell[(int)ArmorTableIndex.CrtRate]) / 100.0f;
        }
        catch
        {
            prop.crtlRateBonus = 0;
        }
        //rcr
        try
        {
            prop.rcrBonus = float.Parse(cell[(int)ArmorTableIndex.RCR]) / 100.0f;
        }
        catch
        {
            prop.rcrBonus = 0;
        }

        return prop;
    }

    enum WeaponTableIndex
    {
        Id,
        Type,
        MinAtk,
        MaxAtk,
        Interval,
        CrtlChance,
        CtrlRate,
        RCR,
        AmmoCap,
    }
    static WeaponProperties ParseWeapon(string str)
    {
        WeaponProperties prop = new WeaponProperties();

        string[] cell = null;
        cell = str.Split(',');
        try
        {
            prop.gunType = (GunType)Int32.Parse(cell[(int)WeaponTableIndex.Type]);
        }
        catch
        {
            return null;
        }
        //min atk
        try
        {
            prop.minAtkBonus = Int32.Parse(cell[(int)WeaponTableIndex.MinAtk]);
        }
        catch
        {
            return null;
        }
        //max atk
        try
        {
            prop.maxAtkBonus = Int32.Parse(cell[(int)WeaponTableIndex.MaxAtk]);
        }
        catch
        {
            return null;
        }
        //interval
        try
        {
            prop.atkInterval = float.Parse(cell[(int)WeaponTableIndex.Interval]);
        }
        catch
        {
            return null;
        }
        //crtl chance
        try
        {
            prop.crtlChanceBonus = float.Parse(cell[(int)WeaponTableIndex.CrtlChance]) / 100.0f;
        }
        catch
        {
            prop.crtlChanceBonus = 0; ;
        }
        //crtl rate
        try
        {
            prop.crtlRateBonus = float.Parse(cell[(int)WeaponTableIndex.CtrlRate]) / 100.0f;
        }
        catch
        {
            prop.crtlRateBonus = 0; ;
        }
        //ammo
        try
        {
            prop.ammoCapacity = UInt32.Parse(cell[(int)WeaponTableIndex.AmmoCap]);
        }
        catch
        {
            return null;
        }
        //RCR
        try
        {
            prop.rcrBonus = float.Parse(cell[(int)WeaponTableIndex.RCR]) / 100.0f;
        }
        catch
        {
            prop.rcrBonus = 0;
        }
        return prop;
    }
}
