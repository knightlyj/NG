using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using CraftList = System.Collections.Generic.List<CraftFormula>;

public static class ItemTypeTable
{
    //物品表
    const int tableSize = 200;
    static ItemType[] itemTable = null; //索引0为null,从1开始

    //合成表
    public static List<string> className = new List<string>();
    public static List<string> classIcon = new List<string>();
    public static CraftList[] craftFormulas = null;
    
    static ItemTypeTable()
    {
        Debug.Log("item table init------------");

        //初始化合成表
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/GameData/" + TextResources.craftClassTable, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB18030")))
            {
                string input;
                int count = 0;
                while ((input = sr.ReadLine()) != null)
                {
                    if (count >= 1)
                    {
                        string[] cell = null;
                        cell = input.Split(',');
                        if (cell.Length < 3)
                        { //到结尾了
                            break;
                        }
                        else
                        { //解析数据
                            if (cell[1] != null && !cell[1].Equals(""))
                            {
                                className.Add(cell[1]); //分类名
                                //分类图标
                                if (cell[2] != null && !cell[2].Equals(""))
                                {
                                    classIcon.Add(cell[2]);
                                }
                                else
                                {
                                    classIcon.Add(TextResources.defaultCraftClassIcon);
                                }
                            }
                        }
                    }
                    count++;
                }
                sr.Close();
            }

            fs.Close();
        }
        craftFormulas = new CraftList[className.Count];
        for (int i = 0; i < craftFormulas.Length; i++)
        {
            craftFormulas[i] = new CraftList();
        }

        //初始化物品表
        itemTable = new ItemType[tableSize];
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/GameData/" + TextResources.itemTable, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB18030")))
            {
                string input;
                int count = 0;
                while ((input = sr.ReadLine()) != null && count < tableSize)
                {
                    string[] cell = null;
                    cell = input.Split(',');
                    if (cell.Length >= 13 && count >= 1) //表格长度限制,且第一行为标题栏
                    {
                        itemTable[count] = new ItemType();
                        itemTable[count].id = count;
                        ParseItem(itemTable[count], cell);
                    }
                    count++;
                }
                sr.Close();
            }
            fs.Close();
        }
    }

    public static ItemType GetItemType(int id)
    {
        if (id < 0 || id >= itemTable.Length)
            return null;
        ItemType type = itemTable[id];
        return type;
    }

    public enum ItemCsvIndex
    {
        Id,
        Name,
        Quality,
        Comment,
        Icon,
        WeaponId,
        ArmorId,
        ConsumId,
        Price,
        CraftStart,
    }

    public static void ParseItem(ItemType type, string[] data)
    {
        //name
        type.itemName = data[(int)ItemCsvIndex.Name];
        //品质
        if (data[(int)ItemCsvIndex.Quality].Equals("白"))
        {
            type.quality = ItemQuality.White;
        }
        else if (data[(int)ItemCsvIndex.Quality].Equals("绿"))
        {
            type.quality = ItemQuality.Green;
        }
        else if (data[(int)ItemCsvIndex.Quality].Equals("蓝"))
        {
            type.quality = ItemQuality.Blue;
        }
        else if (data[(int)ItemCsvIndex.Quality].Equals("紫"))
        {
            type.quality = ItemQuality.Purple;
        }
        else if (data[(int)ItemCsvIndex.Quality].Equals("红"))
        {
            type.quality = ItemQuality.Red;
        }
        else if (data[(int)ItemCsvIndex.Quality].Equals("金"))
        {
            type.quality = ItemQuality.Golden;
        }
        //注释
        type.comment = data[(int)ItemCsvIndex.Comment];
        //图标
        type.icon = data[(int)ItemCsvIndex.Icon];
        //武器id
        if (data[(int)ItemCsvIndex.WeaponId].Length <= 0)
        {
            type.weaponId = 0;
        }
        else
        {
            type.weaponId = Int32.Parse(data[(int)ItemCsvIndex.WeaponId]);
        }
        //护甲id
        if (data[(int)ItemCsvIndex.ArmorId].Length <= 0)
        {
            type.armorId = 0;
        }
        else
        {
            type.armorId = Int32.Parse(data[(int)ItemCsvIndex.ArmorId]);
        }

        //消耗品id
        if (data[(int)ItemCsvIndex.ConsumId].Length <= 0)
        {
            type.consumId = 0;
        }
        else
        {
            type.consumId = Int32.Parse(data[(int)ItemCsvIndex.ConsumId]);
        }
        //价格
        if (data[(int)ItemCsvIndex.Price].Length <= 0)
        {
            type.originalPrice = 1;
        }
        else
        {
            type.originalPrice = Int32.Parse(data[(int)ItemCsvIndex.Price]);
        }
        //合成分类
        int craftClass = className.Count;
        if (data[(int)ItemCsvIndex.CraftStart-1] != null && !data[(int)ItemCsvIndex.CraftStart-1].Equals(""))
        {
            craftClass = Int32.Parse(data[(int)ItemCsvIndex.CraftStart-1]);
        }
        craftClass--; //偏移量从1开始,需要减去1
        //合成公式解析
        CraftFormula formual = new CraftFormula();
        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++) //最多6种材料
        {   //表格偏移量从x开始
            string strMat = data[i + (int)ItemCsvIndex.CraftStart];
            if (strMat != null && !strMat.Equals(""))
            {
                string[] matData = strMat.Split('#');
                if (matData.Length == 2)
                {
                    RawMaterial rawMat = new RawMaterial();
                    try
                    {   //新增一种材料,添加到公式中
                        rawMat.id = Int32.Parse(matData[0]);
                        rawMat.amount = UInt32.Parse(matData[1]);
                        formual.AddMaterial(rawMat);
                    }
                    catch
                    {   //这里应该是数据格式不对,放弃这种材料
                        rawMat = null;
                    }
                }
            }
        }
        //产出物品的id和数量
        formual.outputId = type.id;
        if (data[(int)ItemCsvIndex.CraftStart + 6] != null && !data[(int)ItemCsvIndex.CraftStart + 6].Equals(""))
        {   //解析合成公式的产出数量
            try
            {
                formual.outputAmount = UInt32.Parse(data[(int)ItemCsvIndex.CraftStart + 6]);
            }
            catch
            {
                formual.outputAmount = 0;
            }
        }
        if (formual.matCount > 0 && formual.outputAmount > 0)
        {   //这个合成公式有材料且有产出,添加到合成表中
            if (craftClass < craftFormulas.Length) //合成分类索引不超出范围
            {
                craftFormulas[craftClass].Add(formual);
            }
            else
            {
                Debug.LogError("" + type.id + " craft class error");
            }
        }
    }

}
