using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIStoreSlot : UIItemSlot
{
    public new void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("store slot");
        Helper.ShowTips(base.showItem, UIItemTips.ShowPrice.Buy);
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        Helper.ShowTips(null);
    }
    
    public new void SetItemInfo(Item item)
    {
        showItem = item;
        if (item == null)
        {
            itemImg.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
        }
        else
        {
            itemImg.sprite = Resources.Load<Sprite>("ItemIcon/" + item.Type.icon);
            itemImg.SetNativeSize();
            itemImg.gameObject.SetActive(true);
        }
    }
    
}
