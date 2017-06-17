using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ItemTypeTable
{
    const int tableSize = 100;
    static ItemType[] table = new ItemType[tableSize];
    static bool initialized = false;
    static ItemTypeTable()
    {
        if (initialized)
            return;
        Debug.Log("init------------");
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
                        table[count] = new ItemType();
                        ParseData(table[count], cell);
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
        type = table[(int)id];
        return type;
    }

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
        //装备id
        if (data[4].Length <= 0)
        {
            type.equipId = 0;
        }
        else
        {
            type.equipId = Int32.Parse(data[4]);
        }
        //消耗品id
        if (data[5].Length <= 0)
        {
            type.consumId = 0;
        }
        else
        {
            type.consumId = Int32.Parse(data[5]);
        }
        //合成表解析
        int matCount = 0;
        for (int i = 0; i < ItemType.maxRawMatSorts; i++) //最多6种材料
        {   //表格偏移量从6开始
            string strMat = data[i + 6];
            if (strMat != null && !strMat.Equals(""))
            {
                //type.rawMats[i] =
                string[] matData = strMat.Split('#');
                if (matData.Length == 2)
                {
                    type.rawMats[i] = new RawMaterial();
                    try
                    {
                        type.rawMats[i].id = Int32.Parse(matData[0]);
                        type.rawMats[i].amount = UInt32.Parse(matData[1]);
                        matCount++;
                    }
                    catch
                    {   //这里应该是数据格式不对
                        type.rawMats[i] = null;
                    }
                }
            }
        }
        if (data[12] != null && !data[12].Equals(""))
        {
            try
            {
                type.craftOutput = Int32.Parse(data[12]);
            }
            catch
            {
                type.craftOutput = 0;
            }
        }
        if (matCount != 0 && type.craftOutput == 0)
        {
            Debug.LogError("item table error: id=" + type.id);
        }
    }
}
