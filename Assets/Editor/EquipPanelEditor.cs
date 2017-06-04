using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(EquipPanel))]
class EquipPanelEditor : Editor
{
    EquipPanel equip;
    void OnEnable()
    {
        equip = target as EquipPanel;
    }

    public override void OnInspectorGUI()
    {
        equip.equipSlotPrefab = EditorGUILayout.ObjectField("pack slot", equip.equipSlotPrefab, typeof(Transform), false) as Transform;

        if (GUILayout.Button("clear"))
        {
            RmElement();
        }

        if (GUILayout.Button("生成界面"))
        {
            GenElement();
        }

        int slotSize = EditorGUILayout.IntField("slot size", equip.slotSize);
        if (slotSize < 20)
            slotSize = 20;
        equip.slotSize = slotSize;

        int slotGap = EditorGUILayout.IntField("slot gap", equip.slotGap);
        if (slotGap < 1)
            slotGap = 1;
        equip.slotGap = slotGap;

        int toSide = EditorGUILayout.IntField("to side", equip.slotToSide);
        if (toSide < 1)
            toSide = 1;
        equip.slotToSide = toSide;
        //界面的布局
        if (GUILayout.Button("layout"))
        {
            Layout();
        }
    }

    void GenElement()
    {
        Transform trEquip = equip.transform;
        //武器栏
        for (int i = 0; i < 4; i++)
        {
            RectTransform weaponRect = GameObject.Instantiate(equip.equipSlotPrefab, trEquip) as RectTransform;
            weaponRect.name = "Weapon" + i;
        }

        //道具栏,暂时不做

        //护甲栏
        Array a = Enum.GetValues(typeof(ArmorType));
        for (int i = 0; i < a.Length; i++)
        {
            RectTransform armorRect = GameObject.Instantiate(equip.equipSlotPrefab, trEquip) as RectTransform;
            armorRect.name = ((ArmorType)a.GetValue(i)).ToString();
        }
    }

    void RmElement()
    {
        Transform trEquip = equip.transform;
        List<GameObject> elementList = new List<GameObject>();
        for (int i = 0; i < trEquip.childCount; i++)
        {
            elementList.Add(trEquip.GetChild(i).gameObject);
        }
        foreach (GameObject go in elementList)
            GameObject.DestroyImmediate(go);
        elementList.Clear();
    }

    //界面的布局
    void Layout()
    {
        Transform trEquip = equip.transform;
        int slotSize = equip.slotSize;
        int slotGap = equip.slotGap;
        int toSide = equip.slotToSide;
        //摆武器格子
        for (int i = 0; i < 4; i++)
        {
            RectTransform weaponRect = trEquip.FindChild("Weapon" + i) as RectTransform;
            Vector2 min = new Vector2(toSide + i * (slotSize + slotGap), toSide + slotSize);
            min.y *= -1; //左上角为原点
            Vector2 max = min + new Vector2(slotSize, slotSize);
            weaponRect.anchorMin = new Vector2(0, 1);
            weaponRect.anchorMax = new Vector2(0, 1);
            weaponRect.offsetMin = min;
            weaponRect.offsetMax = max;
        }

        //摆护甲格子
        Array a = Enum.GetValues(typeof(ArmorType));
        for (int i = 0; i < a.Length; i++)
        {
            RectTransform armorRect = trEquip.FindChild(((ArmorType)a.GetValue(i)).ToString()) as RectTransform;
            Vector2 min = new Vector2(toSide + i * (slotSize + slotGap), toSide + 1 * (slotSize + slotGap) + slotSize);
            min.y *= -1;
            Vector2 max = min + new Vector2(slotSize, slotSize);
            armorRect.anchorMin = new Vector2(0, 1);
            armorRect.anchorMax = new Vector2(0, 1);
            armorRect.offsetMin = min;
            armorRect.offsetMax = max;
        }

        equip.width = 8 * slotSize + 7 * slotGap + 2 * toSide;
        equip.height = 2 * slotSize + slotGap + 2 * toSide;
    }
}
