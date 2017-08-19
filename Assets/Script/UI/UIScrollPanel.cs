using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIScrollPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    Scrollbar bar;
    RectTransform rectList;
    float listToSide = 5;
    float scrollWidth = 15;
    
    void Awake()
    {
        RectTransform rect = transform as RectTransform;
        bar = transform.FindChild("Scrollbar").GetComponent<Scrollbar>();
        bar.onValueChanged.AddListener(OnScrollChanged);
        rectList = transform.FindChild("ListView") as RectTransform;
        rectList.offsetMin = new Vector2(listToSide, -10);
        rectList.offsetMax = new Vector2(rect.rect.xMax - rect.rect.xMin - listToSide - scrollWidth, 0);
    }

    // Use this for initialization
    void Start () {

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

    public delegate void CellSelectEvent(ListCell cell);
    public event CellSelectEvent OnCellSelected;
    ListCell selCell = null; //记录已选中的cell
    //click
    public void OnPointerDown(PointerEventData eventData)
    {
        Helper.MoveWndToFront(this.transform);//窗口移动到最前面

        RectTransform rect = transform as RectTransform;
        Vector3[] corners = new Vector3[4]; //顺序为 左下 左上 右上 右下
        rect.GetWorldCorners(corners);
        Vector2 localPos = eventData.position - (Vector2)corners[1]; //这里y是负的
        //Debug.Log(localPos);
        if (localPos.x <= rectList.offsetMax.x)
        {   //在list区域内
            int idx = (int)((rectList.offsetMax.y - localPos.y) / cellHeight);
            if (idx >= cellList.Count || idx < 0)
            {
                return;
            }
            ListCell newSelCell = cellList[idx].GetComponent<ListCell>();
            if (newSelCell != selCell)
            {
                newSelCell.Seleted(true);
                if (selCell != null)
                {
                    selCell.Seleted(false);
                }
                selCell = newSelCell;
                if (OnCellSelected != null)
                {
                    OnCellSelected(selCell);
                }
            }
        }
    }

    void OnScrollChanged(float rate)
    {
        FitScrollBar();
    }

    //根据滚动条调整ListView的位置
    void FitScrollBar()
    {
        RectTransform rect = transform as RectTransform;
        float panelHeight = rect.rect.yMax - rect.rect.yMin;
        if (Helper.FloatEqual(bar.value, 1))
        {   //最低处比panel底部要高,直接移动到底部
            rectList.offsetMin = new Vector2(rectList.offsetMin.x, -panelHeight);
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, rectList.offsetMin.y + cellHeight * cellList.Count);
            bar.value = 1;
        }
        else if (Helper.FloatEqual(bar.value, 0))
        {   //最高处比panel顶部要低,直接移动到顶部
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, 0);
            rectList.offsetMin = new Vector2(rectList.offsetMin.x, -cellHeight * cellList.Count);
            bar.value = 0;
        }
        else
        {
            float topValue = bar.value * (1 - bar.size) / bar.size;
            rectList.offsetMax = new Vector2(rectList.offsetMax.x, topValue * panelHeight);
            rectList.offsetMin = new Vector2(rectList.offsetMin.x, rectList.offsetMax.y - cellHeight * cellList.Count);
        }
    }

    bool filled = false;
    List<Transform> cellList = new List<Transform>();
    float cellHeight;
    //用List<>初始化
    public void SetList(List<System.Object> list, Transform cellPrefab)
    {
        ClearList();
        
        RectTransform rect = transform as RectTransform;
        float width = rectList.offsetMax.x - rectList.offsetMin.x; 
        float minY = 0;
        int idx = 0;
        foreach (System.Object t in list)
        {
            RectTransform rectCell = GameObject.Instantiate(cellPrefab, rectList) as RectTransform;
            cellList.Add(rectCell);
            //设置内容
            ListCell cell = rectCell.GetComponent<ListCell>();
            cell.index = idx++;
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
        float listHeight = rectList.rect.yMax - rectList.rect.yMin;
        float panelHeight = rect.rect.yMax - rect.rect.yMin;
        float scrollSize = panelHeight / listHeight;
        bar.value = 0;
        if (scrollSize >= 1)
        {   //不需要滚动,隐藏滚动条
            bar.size = 1;
            bar.gameObject.SetActive(false);
        }
        else
        {
            bar.size = scrollSize;
            bar.gameObject.SetActive(true);
        }
    }

    //用数组初始化
    public void SetList(System.Object[] list, Transform cellPrefab)
    {
        ClearList();

        RectTransform rect = transform as RectTransform;
        float width = rectList.offsetMax.x - rectList.offsetMin.x;
        float minY = 0;
        int idx = 0;
        foreach (System.Object t in list)
        {
            RectTransform rectCell = GameObject.Instantiate(cellPrefab, rectList) as RectTransform;
            cellList.Add(rectCell);
            //设置内容
            ListCell cell = rectCell.GetComponent<ListCell>();
            cell.index = idx++;
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

        rectList.offsetMax = new Vector2(rectList.offsetMax.x, 0);
        rectList.offsetMin = new Vector2(rectList.offsetMin.x, minY);
        //计算bar的size
        float listHeight = rectList.rect.yMax - rectList.rect.yMin;
        float panelHeight = rect.rect.yMax - rect.rect.yMin;
        float scrollSize = panelHeight / listHeight;
        bar.value = 0;
        if (scrollSize >= 1)
        {   //不需要滚动,隐藏滚动条
            bar.size = 1;
            bar.gameObject.SetActive(false);
        }
        else
        {
            bar.size = scrollSize;
            bar.gameObject.SetActive(true);
        }
    }

    void ClearList()
    {
        while(cellList.Count > 0){
            Destroy(cellList[0].gameObject);
            cellList.RemoveAt(0);
        }
        selCell = null;
        filled = false;
    }


}
