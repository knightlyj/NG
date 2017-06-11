using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.IO;

[CustomEditor(typeof(ItemTypeTable))]
class ItemTypeTableEditor : Editor
{
    ItemTypeTable table;
    void OnEnable()
    {
        table = target as ItemTypeTable;
        showAttr = new bool[table.table.Length];
    }

    string[] typeBit = { "material", "consumable", "armor", "weapon", "可制造"};
    bool[] showAttr;  
    public override void OnInspectorGUI()
    {
        Array a = Enum.GetValues(typeof(ItemId));
        for (int i = 0; i < table.table.Length; i++)
        {
            showAttr[i] = EditorGUILayout.Foldout(showAttr[i], a.GetValue(i).ToString());
            if(showAttr[i])
            {
                if(table.table[i] == null)
                {
                    Debug.Log("null 1");
                    break; 
                }
                if (table.table[i].itemName == null)
                {
                    Debug.Log("null 2");
                    break;
                }
                table.table[i].itemName = EditorGUILayout.TextField("名字", table.table[i].itemName);
                table.table[i].icon = EditorGUILayout.TextField("图标", table.table[i].icon);
                table.table[i].quality = (ItemQuality)EditorGUILayout.EnumPopup("品质", table.table[i].quality);
                table.table[i].comment = EditorGUILayout.TextField("注释", table.table[i].comment);
                table.table[i].typeBit = EditorGUILayout.MaskField("种类", table.table[i].typeBit, typeBit);
                if (table.table[i].CanCraft)
                {   //可合成的,显示合成表

                }
                else
                {
                    table.table[i].rawMats = null;
                }
            }
        }

        //if (GUILayout.Button("test"))
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Debug.Log(table.table[i].name);
        //        Debug.Log(table.table[i].typeBit);
        //    }
        //}
    }

    [MenuItem("Assets/Create/Item Table")]
    static void CreateTable()
    {
        // 实例化类  Bullet
        ScriptableObject itemTalbe = ScriptableObject.CreateInstance<ItemTypeTable>();

        // 如果实例化 Bullet 类为空，返回
        if (!itemTalbe)
        {
            Debug.LogWarning("itemTalbe not found");
            return;
        }

        // 自定义资源保存路径
        string path = Application.dataPath;
        Debug.Log(path);
        //如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //将类名 Bullet 转换为字符串
        //拼接保存自定义资源（.asset） 路径
        path = string.Format("Assets/GameData/{0}.asset", (typeof(ItemTypeTable).ToString()));
        //Debug.Log(path);
        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(itemTalbe, path);
    }
}

//string name = EditorGUILayout.TextField("名字", table.table[i].name);
//                if (!name.Equals(table.table[i].name))
//                {
//                    Undo.RecordObject(table.table[i], "name changed");
//                    table.table[i].name = name;
//                }

//                string icon = EditorGUILayout.TextField("图标", table.table[i].icon);
//                if (!icon.Equals(table.table[i].icon))
//                {
//                    Undo.RecordObject(table.table[i], "icon changed");
//                    table.table[i].icon = icon;
//                }

//                ItemQuality quality = (ItemQuality)EditorGUILayout.EnumPopup("品质", table.table[i].quality);
//                if (quality != table.table[i].quality)
//                {
//                    Undo.RecordObject(table.table[i], "quality changed");
//                    table.table[i].quality = quality;
//                }

//                string comment = EditorGUILayout.TextField("注释", table.table[i].comment);
//                if (!comment.Equals(table.table[i].comment))
//                {
//                    Undo.RecordObject(table.table[i], "comment changed");
//                    table.table[i].comment = comment;
//                }

//                int typeBit = EditorGUILayout.MaskField("种类", table.table[i].typeBit, typeBitOption);
//                if (typeBit != table.table[i].typeBit)
//                {
//                    Undo.RecordObject(table.table[i], "type bit changed");
//                    table.table[i].typeBit = typeBit;
//                }

//                if (table.table[i].CanCraft)
//                {   //可合成的,显示合成表

//                }
//                else
//                {
//                    table.table[i].rawMats = null;
//                }