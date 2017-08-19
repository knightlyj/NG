using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

[CustomEditor(typeof(UIStoreWnd))]
public class StoreWndEditor : Editor
{
    UIStoreWnd store;
    void OnEnable()
    {
        store = target as UIStoreWnd;
    }

    public override void OnInspectorGUI()
    {
        store.slotPrefab = EditorGUILayout.ObjectField("commodity", store.slotPrefab, typeof(Transform), false) as Transform;
        //背包格子太多,用程序生成
        if (GUILayout.Button("生成slot"))
        {
            RmSlots();
            GenSlots();
        }

        int slotSize = EditorGUILayout.IntField("slot size", store.slotSize);
        if (slotSize < 20)
            slotSize = 20;
        store.slotSize = slotSize;

        int slotGap = EditorGUILayout.IntField("slot gap", store.slotGap);
        if (slotGap < 1)
            slotGap = 1;
        store.slotGap = slotGap;

        int toSide = EditorGUILayout.IntField("to side", store.slotToSide);
        if (toSide < 1)
            toSide = 1;
        store.slotToSide = toSide;
        if (GUILayout.Button("layout"))
        {
            LayoutSlot();
        }
    }

    //生成所用格子
    void GenSlots()
    {
        Transform trPack = store.transform.FindChild("Bg");
        if (store.slotPrefab == null)
        {
            Debug.LogError("slot prefab 未设置");
            return;
        }
        //40格商品
        for (int i = 0; i < LocalPlayer.itemPackSize; i++)
        {
            RectTransform slotRect = GameObject.Instantiate(store.slotPrefab, trPack) as RectTransform;
            slotRect.name = "Slot" + i;
        }
    }


    //删除原来的格子
    void RmSlots()
    {
        Transform trPack = store.transform.FindChild("Bg");
        //Debug.Log(pack.transform.childCount);
        List<GameObject> slotList = new List<GameObject>();
        for (int i = 0; i < trPack.childCount; i++)
        {
            Transform tr = trPack.GetChild(i);
            if (tr.name.StartsWith("Slot"))
            {
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
        Transform trPack = store.transform.FindChild("Bg");
        int slotSize = store.slotSize;
        int slotGap = store.slotGap;
        int toSide = store.slotToSide;

        for (int i = 0; i < LocalPlayer.itemPackSize; i++)
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
    }
}
