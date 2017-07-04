using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Image itemImg = null;
    Text itemAmount = null;

    void Awake()
    {
        itemImg = transform.FindChild("Item").GetComponent<Image>();
        itemAmount = transform.FindChild("Amount").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    UIItemTips tips = null;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tips == null)
            tips = Helper.GetItemTips();
        tips.ShowTips(showItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tips == null)
            tips = Helper.GetItemTips();
        tips.ShowTips(null);
    }


    public delegate void OnMouseDown(UIItemSlot slot);
    public event OnMouseDown MouseDownEvent;
    //点击物品格
    public void OnPointerDown(PointerEventData eventData)
    {
        if (MouseDownEvent != null)
        {
            MouseDownEvent(this);
        }
    }

    public int index = -1;
    Item showItem;
    public void SetItemInfo(Item item)
    {
        showItem = item;
        if (item == null)
        {
            itemImg.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
            //if (itemImg == null)
            //    Debug.Log("null 1");
            //if (itemAmount == null)
            //    Debug.Log("null 2");
        }
        else
        {

            string strCnt = "";
            if (item.Type.CanStack) //材料和消耗品显示叠加数量
            {
                strCnt = String.Format("{0}", item.amount);
            }

            itemImg.sprite = Resources.Load<Sprite>("ItemIcon/" + item.Type.icon);
            itemImg.SetNativeSize();
            itemAmount.text = strCnt;
            itemImg.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
        }
    }
}
