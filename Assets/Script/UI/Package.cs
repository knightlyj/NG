using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Package : MonoBehaviour {
    [SerializeField]
    public Transform slotPrefab;
    Image[] packSlots = null;
    Text[] packSlotsAmount = null;
    Image mouseItem;

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
                packSlotsAmount[cnt] = tr.FindChild("Amount").GetComponent<Text>();
                packSlotsAmount[cnt++].fontSize = (int)((tr.offsetMax.y - tr.offsetMin.y) / 40 * 15);
            }
        }

        if (cnt != 40)
            Debug.LogError("Package.Start package slot error");
        //护甲栏
        //Array a = Enum.GetValues(typeof(ArmorType));
        //amorSlots = new Image[a.Length];

        ////武器栏
        //weaponSlots = new Image[Player.weaponAmount];

        SetItemInfo(0, "ItemIcon/Other/Anvil", 10);
    }

    // Update is called once per frame
    void Update () {
	
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
        }
        else
        {
            packSlots[idx].sprite = Resources.Load<Sprite>(sprite);
            packSlots[idx].gameObject.SetActive(true);
            string strCnt = "";
            if (count > 1)
                strCnt = String.Format("{0}", count);
            packSlotsAmount[idx].text = strCnt;
        }
        
    }

    void SetSprite(Image img, string name)
    {
        //if (idx < 0 || idx >= Player.packSize)
        //{
        //    Debug.Log("Package.SetSprite >> idx error");
        //    return;
        //}
        img.sprite = Resources.Load<Sprite>(name);
    }
}
