using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using CraftList = System.Collections.Generic.List<CraftFormula>;

public class ItemTypeTable
{
    //物品表
    const int tableSize = 100;
    static ItemType[] itemTable = null;

    //合成表
    public static readonly string[] className = { "材料", "消耗品", "装备", "其他", "1", "2", "3", "4", "5", "6" };
    public static readonly string[] classIcon = { "wood", "potion", "pistol", "anvil", "wood", "potion", "pistol", "anvil", "wood", "potion" };
    public static CraftList[] craftFormulas = null;

    static bool initialized = false;
    static ItemTypeTable()
    {
        if (initialized)
            return;
        Debug.Log("item table init------------");
        itemTable = new ItemType[tableSize];  //初始化物品表
        //初始化合成表
        craftFormulas = new CraftList[className.Length];
        for(int i = 0; i < craftFormulas.Length; i++)
        {
            craftFormulas[i] = new CraftList();
        }
        using (FileStream fs = new FileStream(Application.streamingAssetsPath + "/GameData/item.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                        ParseData(itemTable[count], cell);
                    }
                    count++;
                }
                sr.Close();
            }
            fs.Close();
        }
        initialized = true;
    }

    public static void Init(string path)
    {

    }

    public static ItemType GetItemType(ItemId id)
    {
        ItemType type = null;
        type = itemTable[(int)id];
        return type;
    }

    const int craftOffset = 8;
    public static void ParseData(ItemType type, string[] data)
    {
        //id
        type.id = Int32.Parse(data[0]);
        //name
        type.itemName = data[1];
        //品质
        if (data[2].Equals("白"))
        {
            type.quality = ItemQuality.White;
        }
        else if (data[2].Equals("绿"))
        {
            type.quality = ItemQuality.Green;
        }
        else if (data[2].Equals("蓝"))
        {
            type.quality = ItemQuality.Blue;
        }
        else if (data[2].Equals("紫"))
        {
            type.quality = ItemQuality.Purple;
        }
        else if (data[2].Equals("红"))
        {
            type.quality = ItemQuality.Red;
        }
        else if (data[2].Equals("金"))
        {
            type.quality = ItemQuality.Golden;
        }
        //注释
        type.comment = data[3];
        //图标
        type.icon = data[4];
        //装备id
        if (data[5].Length <= 0)
        {
            type.equipId = 0;
        }
        else
        {
            type.equipId = Int32.Parse(data[5]);
        }
        //消耗品id
        if (data[6].Length <= 0)
        {
            type.consumId = 0;
        }
        else
        {
            type.consumId = Int32.Parse(data[6]);
        }
        //合成分类
        int craftClass = className.Length;
        if(data[7] != null && !data[7].Equals(""))
        {
            craftClass = Int32.Parse(data[7]);
        }
        craftClass--; //偏移量从1开始,需要减去1
        //合成公式解析
        CraftFormula formual = new CraftFormula();
        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++) //最多6种材料
        {   //表格偏移量从x开始
            string strMat = data[i + craftOffset];
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
        if (data[craftOffset+6] != null && !data[craftOffset+6].Equals(""))
        {   //解析合成公式的产出数量
            try
            {
                formual.outputAmount = UInt32.Parse(data[craftOffset + 6]);
            }
            catch
            {
                formual.outputAmount = 0;
            }
        }
        if(formual.matCount > 0 && formual.outputAmount > 0) 
        {   //这个合成公式有材料且有产出,添加到合成表中
            craftFormulas[craftClass].Add(formual);
        }
    }

    
}
