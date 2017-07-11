using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(UIBagWnd))]
class BagWndEditor : Editor
{
    UIBagWnd bag;
    void OnEnable()
    {
        bag = target as UIBagWnd;
    }
    public override void OnInspectorGUI()
    {
        bag.packSlotPrefab = EditorGUILayout.ObjectField("pack slot", bag.packSlotPrefab, typeof(Transform), false) as Transform;

        //背包格子太多,用程序生成
        if (GUILayout.Button("生成背包slot"))
        {
            RmPackSlots();
            GenPackSlots();
        }

        int slotSize = EditorGUILayout.IntField("slot size", bag.slotSize);
        if (slotSize < 20)
            slotSize = 20;
        bag.slotSize = slotSize;

        int slotGap = EditorGUILayout.IntField("slot gap", bag.slotGap);
        if (slotGap < 1)
            slotGap = 1;
        bag.slotGap = slotGap;

        int toSide = EditorGUILayout.IntField("to side", bag.slotToSide);
        if (toSide < 1)
            toSide = 1;
        bag.slotToSide = toSide;
        if (GUILayout.Button("layout"))
        {
            LayoutSlot();
        }
    }

    //生成背包所用格子
    void GenPackSlots()
    {
        Transform trPack = bag.transform.FindChild("Bg");
        if (bag.packSlotPrefab == null)
        {
            Debug.LogError("slot prefab 未设置");
            return;
        }
        //40格背包
        for (int i = 0; i < Player.itemPackSize; i++)
        {
            RectTransform slotRect = GameObject.Instantiate(bag.packSlotPrefab, trPack) as RectTransform;
            slotRect.name = "Slot" + i;
        }

        //垃圾箱
        RectTransform trashRect = GameObject.Instantiate(bag.packSlotPrefab, trPack) as RectTransform;
        trashRect.name = "Trash";
        Image trashImg = trashRect.GetComponent<Image>();
    }


    //删除原来的格子
    void RmPackSlots()
    {
        Transform trPack = bag.transform.FindChild("Bg");
        //Debug.Log(pack.transform.childCount);
        List<GameObject> slotList = new List<GameObject>();
        for (int i = 0; i < trPack.childCount; i++)
        {
            Transform tr = trPack.GetChild(i);
            if (tr.name.StartsWith("Slot") || tr.name.Equals("Trash")) {
                slotList.Add(tr.gameObject);
            }
        }
        foreach (GameObject go in slotList)
            GameObject.DestroyImmediate(go);
        slotList.Clear();
    }

    const int rowAmout = 10;
    //格子布局
    void LayoutSlot()
    {
        Transform trPack = bag.transform.FindChild("Bg");
        int slotSize = bag.slotSize;
        int slotGap = bag.slotGap;
        int toSide = bag.slotToSide;

        for (int i = 0; i < Player.itemPackSize; i++)
        {
            RectTransform rect = trPack.FindChild("Slot" + i) as RectTransform;
            if (rect == null)
            {
                Debug.LogError("no slot");
                return;
            }
            int row = i / rowAmout, col = i % rowAmout;
            //Vector2 halfSize = new Vector2(slotSize / 2, slotSize / 2);
            Vector2 min = new Vector2(toSide + col * (slotSize + slotGap), toSide + row * (slotSize + slotGap) + slotSize);
            min.y *= -1;
            Vector2 max = min + new Vector2(slotSize, slotSize);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.offsetMin = min;
            rect.offsetMax = max;
        }

        int trashRow = Player.itemPackSize / rowAmout;
        int trashCol = (Player.itemPackSize - 1) % rowAmout;
        RectTransform trashRect = trPack.FindChild("Trash") as RectTransform;
        trashRect.anchorMin = new Vector2(0, 1);
        trashRect.anchorMax = new Vector2(0, 1);
        trashRect.offsetMin = new Vector2(toSide + trashCol * (slotSize + slotGap), -(toSide + trashRow * (slotSize + slotGap) + slotSize));
        trashRect.offsetMax = trashRect.offsetMin + new Vector2(slotSize, slotSize);
    }
}
