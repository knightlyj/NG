using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image itemImg = null;
    Text itemAmount = null;

	// Use this for initialization
	void Start () {
        itemImg = transform.FindChild("Item").GetComponent<Image>();
        itemAmount = transform.FindChild("Amount").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit " + gameObject.name);
    }

    Item _item;
    public void SetItemInfo(Item item)
    {
        _item = item;
        if(item == null)
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
            if (item.Type.IsConsumable || item.Type.IsMaterail) //材料和消耗品显示叠加数量
            {
                strCnt = String.Format("{0}", item.amount);
            }

            itemImg.sprite = Resources.Load<Sprite>(item.icon);
            itemAmount.text = strCnt;
            itemImg.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
        }
    }
}
