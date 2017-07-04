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
        //SetItem(ItemTypeTable.GetItemType(ItemId.Wood), 10);
    }

    Item showItem = null;
    // Update is called once per frame
    void Update()
    {
        //Player localPlayer = Helper.FindLocalPlayer();
        //if (localPlayer != null)
        {
            //获取背包
            //PlayerPackage playerPack = localPlayer.playerPack;
            PlayerPackage playerPack = Helper.playerPackage; //测试用背包
            //获取背包内容
            MouseItem mouseItem = playerPack.mouseItem;
            SetItemInfo(mouseItem.item);
        }

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
}
