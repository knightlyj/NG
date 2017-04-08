using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Package))]
class PackageEditor : Editor
{
    Package pack;
    Transform slotPrefab;
    void OnEnable()
    {
        pack = target as Package;
        slotPrefab = pack.slotPrefab;
    }
    public override void OnInspectorGUI()
    {
        slotPrefab = EditorGUILayout.ObjectField("slot prefab", slotPrefab, typeof(Transform), false) as Transform;

        int slotSize = EditorGUILayout.IntField("slot size", pack.slotSize);
        if (slotSize < 20)
            slotSize = 40;
        pack.slotSize = slotSize;

        int slotGap = EditorGUILayout.IntField("slot gap", pack.slotGap);
        if (slotGap < 5)
            slotGap = 5;
        pack.slotGap = slotGap;

        int toSide = EditorGUILayout.IntField("to side", pack.toSide);
        if (toSide < 5)
            toSide = 5;
        pack.toSide = toSide;
                
        //slot
        if (GUILayout.Button("生成package"))
        {
            //物品栏面板
            Vector2 panelLeftTop = new Vector2(20, 20);
            float panelWidth = 10 * slotSize + 9 * slotGap + toSide * 2;
            float panelHeight = 4 * slotSize + 3 * slotGap + toSide * 2;
            RectTransform rect = pack.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(panelLeftTop.x, Screen.height - panelLeftTop.y - panelHeight);
            rect.offsetMax = rect.offsetMin + new Vector2(panelWidth, panelHeight);

            //Debug.Log(pack.transform.childCount);
            List<GameObject> slotList = new List<GameObject>();
            for(int i = 0; i < pack.transform.childCount; i++)
            {
                slotList.Add(pack.transform.GetChild(i).gameObject);
            }
            foreach (GameObject go in slotList)
                GameObject.DestroyImmediate(go);
            slotList.Clear();


            for (int i = 0; i < Player.packSize; i++)
            {
                RectTransform slotRect = GameObject.Instantiate(slotPrefab, pack.transform) as RectTransform;
                int row = i / 10;
                int col = i % 10;
                slotRect.anchorMin = Vector2.zero;
                slotRect.anchorMax = Vector2.zero;
                slotRect.offsetMin = new Vector2(toSide + col * (slotSize + slotGap), panelHeight - (toSide + slotSize + row * (slotSize + slotGap)));
                slotRect.offsetMax = slotRect.offsetMin + new Vector2(slotSize, slotSize);
                slotRect.name = "Slot" + i;
            }
        }
    }
}
