using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIConsole : MonoBehaviour {
    InputField itemId, itemAmount;
    InputField moneyInput;
    void Awake()
    {
        //物品
        itemId = transform.FindChild("Bg").FindChild("ItemId").GetComponent<InputField>();
        itemAmount = transform.FindChild("Bg").FindChild("ItemAmount").GetComponent<InputField>();

        Button addItem = transform.FindChild("Bg").FindChild("AddItem").GetComponent<Button>();
        addItem.onClick.AddListener(this.OnAddItemClick);

        //金钱
        moneyInput = transform.FindChild("Bg").FindChild("Money").GetComponent<InputField>();

        Button addMoney = transform.FindChild("Bg").FindChild("AddMoney").GetComponent<Button>();
        addMoney.onClick.AddListener(this.OnAddMoneyClick);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        //物品
        Button addItem = transform.FindChild("Bg").FindChild("AddItem").GetComponent<Button>();
        addItem.onClick.RemoveListener(this.OnAddItemClick);

        //钱
        Button addMoney = transform.FindChild("Bg").FindChild("AddMoney").GetComponent<Button>();
        addMoney.onClick.RemoveListener(this.OnAddMoneyClick);
    }

    void OnAddItemClick()
    {
        int intId = -1;
        uint amount = 0;
        try
        {
            intId = Int32.Parse(itemId.text);
            amount = UInt32.Parse(itemAmount.text);
        }
        catch
        {
            intId = -1;
            amount = 0;
        }

        if (amount == 0)
        {

        }
        else
        {
            ItemId id = (ItemId)intId;
            Item newItem = new Item(id, amount);
            if (newItem.valid)
            {
                Player localPlayer = Helper.FindLocalPlayer();
                if (localPlayer != null)
                {
                    //获取背包
                    PlayerBag playerBag = localPlayer.bag;
                    ItemPackage pack = playerBag.itemPack;//获取背包内容
                    pack.PickUpItem(newItem); //捡起物品
                }
            }
        }
    }

    void OnAddMoneyClick()
    {
        int money = 0;
        try
        {
            money = Int32.Parse(moneyInput.text);
        }
        catch
        {
            money = 0;
        }

        Player localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            //获取背包
            PlayerBag playerBag = localPlayer.bag;
            playerBag.money += money;
        }

    }
}
