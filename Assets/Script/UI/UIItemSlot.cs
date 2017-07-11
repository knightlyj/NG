using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected Image itemImg = null;
    protected Text itemAmount = null;

    protected void Awake()
    {
        itemImg = transform.FindChild("Item").GetComponent<Image>();
        itemAmount = transform.FindChild("Amount").GetComponent<Text>();
    }

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Helper.ShowTips(showItem, UIItemTips.ShowPrice.Sell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Helper.ShowTips(null);
    }


    public delegate void OnMouseDown(UIItemSlot slot, PointerEventData eventData);
    public event OnMouseDown MouseDownEvent;
    //点击物品格
    public void OnPointerDown(PointerEventData eventData)
    {
        if (MouseDownEvent != null)
        {
            MouseDownEvent(this, eventData);
        }
    }

    [HideInInspector]
    public int index = -1;
    protected Item showItem;
    public void SetItemInfo(Item item)
    {
        showItem = item;
        if (item == null)
        {
            itemImg.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
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
