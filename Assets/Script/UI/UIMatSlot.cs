using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIMatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        SetMaterial(ItemTypeTable.GetItemType(ItemId.Iron), 20);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter " + gameObject.name);
        if (itemType != null)
        {
            if (tips == null)
                tips = Helper.GetItemTips();
            tips.ShowTips(showItem);
        }
    }

    UIItemTips tips = null;
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit " + gameObject.name);
        if (tips == null)
            tips = Helper.GetItemTips();
        tips.ShowTips(null);
    }

    Item showItem = null; //用来显示
    ItemType itemType = null;
    public void SetMaterial(ItemType type, uint amount)
    {
        itemType = type;
        if (type == null || amount == 0) //数量为0也视为空
        {
            itemImg.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
            showItem = null;
            itemType = null;
        }
        else
        {

            string strCnt = "";
            if (type.CanStack) //材料和消耗品显示叠加数量
            {
                strCnt = String.Format("{0}", amount);
            }

            itemImg.sprite = Resources.Load<Sprite>("ItemIcon/" + type.icon);
            itemAmount.text = strCnt;
            itemImg.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
            itemImg.SetNativeSize();

            showItem = new Item(type, amount);
        }
    }
}
