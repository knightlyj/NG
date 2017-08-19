using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;

public class UIConsole : MonoBehaviour {
    InputField itemId, itemAmount;
    InputField moneyInput;
    Text txtProp;
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

        //属性
        txtProp = transform.FindChild("Bg").FindChild("Properties").GetComponent<Text>();

        Button btnSave = transform.FindChild("Bg").FindChild("Save").GetComponent<Button>();
        btnSave.onClick.AddListener(this.Save);
        Button btnLoad = transform.FindChild("Bg").FindChild("Load").GetComponent<Button>();
        btnLoad.onClick.AddListener(this.Load);
    }

    // Use this for initialization
    void Start () {
	
	}

    int propUpateCnt = 0;
	// Update is called once per frame
	void Update () {
	    if(++propUpateCnt >= 20)
        {
            UpdateProperties();
        }
	}

    void OnDestroy()
    {
        //物品
        Button addItem = transform.FindChild("Bg").FindChild("AddItem").GetComponent<Button>();
        addItem.onClick.RemoveListener(this.OnAddItemClick);

        //钱
        Button addMoney = transform.FindChild("Bg").FindChild("AddMoney").GetComponent<Button>();
        addMoney.onClick.RemoveListener(this.OnAddMoneyClick);


        Button btnSave = transform.FindChild("Bg").FindChild("Save").GetComponent<Button>();
        btnSave.onClick.RemoveListener(this.Save);
        Button btnLoad = transform.FindChild("Bg").FindChild("Load").GetComponent<Button>();
        btnLoad.onClick.RemoveListener(this.Load);
    }

    void OnAddItemClick()
    {
        int intId = -1;
        uint amount = 0;
        try
        {
            intId = Int32.Parse(itemId.text);
        }
        catch
        {
            intId = -1;
        }
        try
        {
            amount = UInt32.Parse(itemAmount.text);
        }
        catch
        {
            amount = 1;
        }

        if (amount == 0)
        {

        }
        else
        {
            int id = intId;
            Item newItem = new Item(id, amount);
            if (newItem.valid)
            {
                LocalPlayer localPlayer = Helper.FindLocalPlayer();
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

        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            //获取背包
            PlayerBag playerBag = localPlayer.bag;
            playerBag.money += money;
        }
    }

    const string propFormat = "";
    void UpdateProperties()
    {
        Player localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            EntityProperties prop = localPlayer.Properties;
            txtProp.text = string.Format("血量: {0}/{1}, 攻击: {2}-{3},防御: {4}\n攻击间隔: {5:N}, 攻速: {6}%\n暴击率: {7}%, 暴击伤害: {8}%\nrcr: {9}%,  速度: {10}%, 跳跃: {11}%\n 后坐力: {12}, 击退: {13}", 
                prop.hp, prop.maxHp, prop.minAttack, prop.maxAttack, prop.defense,
                prop.atkInterval, Mathf.Round(prop.atkSpeed * 100),
                Mathf.Round(prop.criticalChance * 100), Mathf.Round(prop.criticalRate * 100),
                Mathf.Round(prop.rcr * 100), Mathf.Round(prop.speedScale * 100), Mathf.Round(prop.jumpScale * 100),
                prop.recoil, prop.knockBack);
        }
    }

    void Save()
    {
        GameManager manager = Helper.GetManager();
        if(manager != null)
        {
            manager.SaveGame();
        }
    }

    void Load()
    {
        GameManager manager = Helper.GetManager();
        if (manager != null)
        {
            manager.LoadGame("local player", null);
        }
    }
}
