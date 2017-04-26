using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Package : MonoBehaviour {
    [SerializeField]
    public Transform slotPrefab;
    public Transform trashPrefab;
    //背包物品格子
    Image[] packSlots = null;
    Text[] packSlotsAmount = null;
    //垃圾箱
    Image trashSlot = null;
    Text trashSlotAmount = null;
    //鼠标拖拽物品
    Image mouseItem = null;
    Text mouseItemAmount = null;

    //slot
    public int toSide;
    public int slotGap;
    public int slotSize;

	// Use this for initialization
	void Start () {

        //物品栏 8行5列
        packSlots = new Image[Player.packSize];
        packSlotsAmount = new Text[Player.packSize];
        int cnt = 0;
        for (int i = 0; i < transform.childCount && cnt < Player.packSize; i++)
        {
            RectTransform tr = transform.GetChild(i) as RectTransform;
            if (tr.tag == "Items")
            {
                packSlots[cnt] = tr.FindChild("Item").GetComponent<Image>();
                packSlotsAmount[cnt++] = tr.FindChild("Amount").GetComponent<Text>();
            }

        }
        //垃圾箱
        trashSlot = transform.FindChild("Trash").FindChild("Item").GetComponent<Image>();
        trashSlotAmount = transform.FindChild("Trash").FindChild("Amount").GetComponent<Text>();

        //鼠标物品
        RectTransform mouseRect = transform.parent.FindChild("MouseItem") as RectTransform;
        mouseItem = mouseRect.GetComponent<Image>();
        mouseItemAmount = mouseRect.FindChild("Amount").GetComponent<Text>();

        if (cnt != 40)
            Debug.LogError("Package.Start package slot error");
        if(trashSlot == null)
            Debug.LogError("Package.Start trash slot error");
        //护甲栏
        //Array a = Enum.GetValues(typeof(ArmorType));
        //amorSlots = new Image[a.Length];

        ////武器栏
        //weaponSlots = new Image[Player.weaponAmount];

        SetItemInfo(0, "ItemIcon/Other/Anvil", 10);
    }

    Player localPlayer = null;
    // Update is called once per frame
    void Update () {
        if (localPlayer == null) 
        {//还没绑定到本地玩家,则寻找
            localPlayer = Helper.FindLocalPlayer();
            if (localPlayer != null)
            {
                UpdatePack();
                localPlayer.PackChangedEvent += this.OnPackChanged;
                localPlayer.EntityDestroyEvent += this.OnPlayerDestroyed;
            }
        }
        else
        { //如果玩家有拖拽物品,则跟随鼠标
            if (localPlayer.mouseItem != null)
            {
                this.mouseItem.transform.position = Input.mousePosition; //鼠标位置就是屏幕位置
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="sprite"></param> 为null时,表示格子为空,否则传入物品图标名称
    /// <param name="count"></param> 物品数量
    void SetItemInfo(int idx, string sprite, uint count)
    {
        if (idx < 0 || idx >= Player.packSize)
        {
            Debug.Log("Package.SetItemInfo >> idx error");
            return;
        }
        if(sprite == null)
        {
            packSlots[idx].gameObject.SetActive(false);
            packSlotsAmount[idx].gameObject.SetActive(false);
        }
        else
        {
            packSlots[idx].sprite = Resources.Load<Sprite>(sprite);
            packSlots[idx].gameObject.SetActive(true);
            packSlotsAmount[idx].gameObject.SetActive(true);
            string strCnt = "";
            if (count > 1)
                strCnt = String.Format("{0}", count);
            packSlotsAmount[idx].text = strCnt;
        }
    }

    void SetItemInfo(Image img, string sprite, Text txt, uint count)
    {
        if (sprite == null)
        {
            img.gameObject.SetActive(false);
            txt.gameObject.SetActive(false);
        }
        else
        {
            img.sprite = Resources.Load<Sprite>(sprite);
            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);
            string strCnt = "";
            if (count > 1)
                strCnt = String.Format("{0}", count);
            txt.text = strCnt;
        }
    }


    //更新背包
    void UpdatePack()
    {
        if(localPlayer != null)
        {   //显示所有物品
            for(int i = 0; i < Player.packSize; i++)
            {
                if (localPlayer.Package[i] != null)
                    SetItemInfo(i, localPlayer.Package[i].icon, localPlayer.Package[i].amount);
                else
                    SetItemInfo(i, null, 0);
            }
            //显示trash
            if (localPlayer.trashItem != null)
                SetItemInfo(trashSlot, localPlayer.trashItem.icon, trashSlotAmount, localPlayer.trashItem.amount);
            else
                SetItemInfo(trashSlot, null, trashSlotAmount, 0);
            //显示鼠标物品
            if (localPlayer.mouseItem != null)
                SetItemInfo(this.mouseItem, localPlayer.mouseItem.icon, this.mouseItemAmount, localPlayer.mouseItem.amount);
            else
                SetItemInfo(this.mouseItem, null, this.mouseItemAmount, 0);
        }
        else
        {   //不应该运行到这里
            Debug.LogError("Package.ShowPack >> local player is null");
        }
    }

    //背包物品有变化时的回调
    void OnPackChanged()
    {
        UpdatePack();
    }

    //玩家销毁时的回调
    void OnPlayerDestroyed()
    {
        localPlayer.EntityDestroyEvent -= this.OnPlayerDestroyed;
        localPlayer.PackChangedEvent -= this.OnPackChanged;
        localPlayer = null;
    }
}
