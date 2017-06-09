using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ScrollPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public Transform CellPrefab;
    Scrollbar bar;
    RectTransform rectList;
    // Use this for initialization
    void Start () {
        RectTransform rect = transform as RectTransform;
        bar = transform.FindChild("Scrollbar").GetComponent<Scrollbar>();
        bar.onValueChanged.AddListener(OnScrollChanged);
        rectList = transform.FindChild("ListView") as RectTransform;
        rectList.offsetMin = new Vector2(0, -10);
        rectList.offsetMax = new Vector2(rect.offsetMax.x - rect.offsetMin.x - 15, 0);
        

        List<Item> test = new List<Item>();
        Spawner sp = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        for (int i = 0; i < 20; i++)
        {
            test.Add(new Item(sp.table.GetItemType(ItemId.Gold), 1));
        }
        SetList(test);
    }
	
	// Update is called once per frame
	void Update () {
        if (mouseIn && filled)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if(wheel != 0 && bar.size < 1)
            {
                bar.value += bar.size * -wheel * 2;
                FitScrollBar();
            }
        }
	}

    bool mouseIn = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIn = false;
    }

    public delegate void CellSelectEvent(ItemCell cell);
    public event CellSelectEvent OnCellSelected;
    ItemCell selCell = null;
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransform rect = transform as RectTransform;
        Vector2 localPos = eventData.position - new Vector2(0, Screen.height) - rect.anchoredPosition;
        Debug.Log(localPos);
        if (localPos.x <= rectList.offsetMax.x)
        {   //在list区域内
            int idx = (int)((rectList.offsetMax.y - localPos.y) / cellHeight);
            ItemCell newSelCell = cellList[idx].GetComponent<ItemCell>();
            newSelCell.Seleted(true);
            if (selCell != null)
            {
                selCell.Seleted(false);
            }
            selCell = newSelCell;
            if(OnCellSelected != null)
            {
                OnCellSelected(selCell);
            }
        }
    }

    void OnScrollChanged(float rate)
    {
        FitScrollBar();
    }

    void FitScrollBar()
    {
        RectTransform rect = transform as RectTransform;
        float panelHeight = rect.offsetMax.y - rect.offsetMin.y;
        if (Helper.FloatEqual(bar.value, 1))
        {   //最低处比panel底部要高,直接移动到底部
            rectList.offsetMin = new Vector2(rectList.offsetMin.x, -panelHeight);
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, rectList.offsetMin.y + cellHeight * cellList.Count);
            bar.value = 1;
        }
        else if (Helper.FloatEqual(bar.value, 0))
        {   //最高处比panel顶部要低,直接移动到顶部
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, 0);
            rectList.offsetMin = new Vector2(0, -cellHeight * cellList.Count);
            bar.value = 0;
        }
        else
        {
            float topValue = bar.value * (1 - bar.size);
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, topValue * panelHeight);
            rectList.offsetMin = new Vector2(0, rectList.offsetMax.y - cellHeight * cellList.Count);
        }
    }

    bool filled = false;
    List<Transform> cellList = new List<Transform>();
    float cellHeight;
    public void SetList(List<Item> list)
    {
        ClearList();
        
        RectTransform rect = transform as RectTransform;
        float width = rectList.offsetMax.x - rectList.offsetMin.x; 
        float minY = 0;
        foreach (Item t in list)
        {
            RectTransform rectCell = GameObject.Instantiate(CellPrefab, rectList) as RectTransform;
            cellList.Add(rectCell);
            //设置内容
            ItemCell cell = rectCell.GetComponent<ItemCell>();
            cell.SetContent(t);
            //摆放位置
            cellHeight = cell.GetHeight();
            minY -= cellHeight;
            rectCell.anchorMax = new Vector2(0, 1);
            rectCell.anchorMin = new Vector2(0, 1);
            rectCell.offsetMin = new Vector2(0, minY);
            rectCell.offsetMax = new Vector2(width, minY + cellHeight);
            filled = true; //标记有填充内容
        }
        
        rectList.offsetMin = new Vector2(rectList.offsetMin.x, minY);
        //计算bar的size
        float listHeight = rectList.offsetMax.y - rectList.offsetMin.y;
        float panelHeight = rect.offsetMax.y - rect.offsetMin.y;
        float scrollSize = panelHeight / listHeight;
        if (scrollSize >= 1)
            bar.size = 1;
        else
            bar.size = scrollSize;
    }
    
    void ClearList()
    {
        while(cellList.Count > 0){
            Destroy(cellList[0]);
            cellList.RemoveAt(0);
        }
        filled = false;
    }


}
