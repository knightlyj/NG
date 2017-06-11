using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

[CustomEditor(typeof(PlayerInfo))]
class PlayerInfoEditor : Editor
{
    PlayerInfo info;
    void OnEnable()
    {
        info = target as PlayerInfo;
    }

    public override void OnInspectorGUI()
    {
        int toLeft = EditorGUILayout.IntField("to left", info.toLeft);
        if (toLeft < 20)
            toLeft = 20;
        info.toLeft = toLeft;

        int toTop = EditorGUILayout.IntField("to top", info.toTop);
        if (toTop < 20)
            toTop = 20;
        info.toTop = toTop;

        int panelGap = EditorGUILayout.IntField("panel gap", info.panelGap);
        if (panelGap < 20)
            panelGap = 20;
        info.panelGap = panelGap;

        //界面的布局
        if (GUILayout.Button("layout"))
        {
            Layout();
        }

    }

    //各个界面的布局
    void Layout()
    {
        int toLeft = info.toLeft;
        int toTop = info.toTop;
        int panelGap = info.panelGap;

        //装备栏
        EquipPanel equip = info.transform.FindChild("EquipPanel").GetComponent<EquipPanel>();
        RectTransform trEquip = equip.transform as RectTransform;
        trEquip.anchorMin = new Vector2(0, 1);
        trEquip.anchorMax = new Vector2(0, 1);
        trEquip.offsetMin = new Vector2(toLeft, -toTop - equip.height);
        trEquip.offsetMax = new Vector2(toLeft + equip.width, -toTop);

        Debug.Log("equip " + equip.width + ", " + equip.height);

        //背包栏
        PackPanel pack = info.transform.FindChild("PackPanel").GetComponent<PackPanel>();
        RectTransform trPack = pack.transform as RectTransform;
        trPack.anchorMin = new Vector2(0, 1);
        trPack.anchorMax = new Vector2(0, 1);
        trPack.offsetMin = trEquip.offsetMin + new Vector2(0, -panelGap - pack.height);
        trPack.offsetMax = trPack.offsetMin + new Vector2(pack.width, pack.height);

        Debug.Log("pack " + pack.width + ", " + pack.height);
    }
}
