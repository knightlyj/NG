using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(ItemTypeTable))]
class ItemTypeTableEditor : Editor
{
    ItemTypeTable table;
    void OnEnable()
    {
        table = target as ItemTypeTable;
    }

    string[] typeBit = { "material", "consumable", "armor", "weapon" };
    public override void OnInspectorGUI()
    {
        Array a = Enum.GetValues(typeof(ItemId));
        for (int i = 0; i < table.table.Length; i++)
        {
            EditorGUILayout.LabelField(a.GetValue(i).ToString());
            table.table[i].name = EditorGUILayout.TextField("\t名字", table.table[i].name);
            table.table[i].quality = (ItemQuality)EditorGUILayout.EnumPopup("\t品质", table.table[i].quality);
            table.table[i].comment = EditorGUILayout.TextField("\t注释", table.table[i].comment);
            table.table[i].typeBit = EditorGUILayout.MaskField("\t种类", table.table[i].typeBit, typeBit);
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
}
