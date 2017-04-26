using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(Package))]
class PackageEditor : Editor
{
    Package pack;
    Transform trashPrefab;
    void OnEnable()
    {
        pack = target as Package;
    }
    public override void OnInspectorGUI()
    {
        pack.slotPrefab = EditorGUILayout.ObjectField("slot prefab", pack.slotPrefab, typeof(Transform), false) as Transform;
        pack.trashPrefab = EditorGUILayout.ObjectField("trash prefab", pack.trashPrefab, typeof(Transform), false) as Transform;

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
            float panelHeight = 5 * slotSize + 4 * slotGap + toSide * 2;
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

            //40格背包
            for (int i = 0; i < Player.packSize; i++)
            {
                RectTransform slotRect = GameObject.Instantiate(pack.slotPrefab, pack.transform) as RectTransform;
                int row = i / 10;
                int col = i % 10;
                slotRect.anchorMin = Vector2.zero;
                slotRect.anchorMax = Vector2.zero;
                slotRect.offsetMin = new Vector2(toSide + col * (slotSize + slotGap), panelHeight - (toSide + slotSize + row * (slotSize + slotGap)));
                slotRect.offsetMax = slotRect.offsetMin + new Vector2(slotSize, slotSize);
                slotRect.name = "Slot" + i;

                Text txtSlot = slotRect.FindChild("Amount").GetComponent<Text>();
                txtSlot.fontSize = (int)(slotSize / 40.0f * 15.0f);
            }

            //垃圾箱
            RectTransform trashRect = GameObject.Instantiate(pack.trashPrefab, pack.transform) as RectTransform;
            trashRect.anchorMin = Vector2.zero;
            trashRect.anchorMax = Vector2.zero;
            trashRect.offsetMin = new Vector2(toSide + 9 * (slotSize + slotGap), panelHeight - (toSide + slotSize + 4 * (slotSize + slotGap)));
            trashRect.offsetMax = trashRect.offsetMin + new Vector2(slotSize, slotSize);
            trashRect.name = "Trash";
            Text txtTrash = trashRect.FindChild("Amount").GetComponent<Text>();
            txtTrash.fontSize = (int)(slotSize / 40.0f * 15.0f);

            //鼠标拖拽物品
            RectTransform mouseRect = pack.transform.parent.FindChild("MouseItem") as RectTransform;
            mouseRect.anchorMin = Vector2.zero;
            mouseRect.anchorMax = Vector2.zero;
            mouseRect.offsetMin = Vector2.zero;
            mouseRect.offsetMax = new Vector2(slotSize, slotSize);
            mouseRect.name = "MouseItem";
            Text txtMouse = trashRect.FindChild("Amount").GetComponent<Text>();
            txtTrash.fontSize = (int)(slotSize / 40.0f * 17.0f);
        }
    }
}
