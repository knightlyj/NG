using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIMouseItem : MonoBehaviour
{
    public Vector2 offset;
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
        EventManager.AddListener(EventId.MouseItemChange, this.OnMouseItemChanged);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(EventId.MouseItemChange, this.OnMouseItemChanged);
    }

    Item showItem = null;
    // Update is called once per frame
    void Update()
    {
        if (showItem != null)
        {
            //跟随鼠标
            Vector2 pos = Input.mousePosition;
            RectTransform rect = transform as RectTransform;
            rect.anchoredPosition = pos + offset;
        }
    }

    public void SetItemInfo(Item item)
    {
        showItem = item;
        if (item == null) //数量为0也视为空
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
            itemAmount.text = strCnt;
            itemImg.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
            itemImg.SetNativeSize();
        }
    }

    public bool hasItem { get { return this.showItem != null; } }

    void OnMouseItemChanged(System.Object sender)
    {
        MouseItem mouseItem = sender as MouseItem;
        if(mouseItem != null)
            SetItemInfo(mouseItem.item);
    }
}
