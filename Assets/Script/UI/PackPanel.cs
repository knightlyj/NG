using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PackPanel : MonoBehaviour {
    [SerializeField]
    public Transform packSlotPrefab;

    //背包物品格子
    ItemSlot[] packSlots = null;

    //垃圾箱
    ItemSlot trashSlot = null;

    //鼠标拖拽物品
    ItemSlot mouseSlot = null;

    //slot
    public int slotToSide;
    public int slotGap;
    public int slotSize;

    public int width, height;

	// Use this for initialization
	void Start () {

        //物品栏 8行5列
        packSlots = new ItemSlot[Player.packSize];
        for (int i = 0; i < Player.packSize; i++)
        {
            packSlots[i] = transform.FindChild("Slot" + i).GetComponent<ItemSlot>();
        }

        //垃圾箱
        trashSlot = transform.FindChild("Trash").GetComponent<ItemSlot>();

        //鼠标物品
        mouseSlot = transform.FindChild("MouseItem").GetComponent<ItemSlot>();

        //护甲栏
        //Array a = Enum.GetValues(typeof(ArmorType));
        //amorSlots = new Image[a.Length];

        ////武器栏
        //weaponSlots = new Image[Player.weaponAmount];

        //packSlots[0].SetItemInfo(null); //for test
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
                this.mouseSlot.transform.position = Input.mousePosition; //鼠标位置就是屏幕位置
            }
        }
    }

    //更新背包
    void UpdatePack()
    {
        if(localPlayer != null)
        {   //显示所有物品
            for(int i = 0; i < Player.packSize; i++)
            {
                packSlots[i].SetItemInfo(localPlayer.Package[i]);
            }
            //显示trash
            trashSlot.SetItemInfo(localPlayer.trashItem);

            //显示鼠标物品
            mouseSlot.SetItemInfo(localPlayer.mouseItem);
        }
        else
        {   //不应该运行到这里
            Debug.LogError("PackPanel.UpdatePack >> local player is null");
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
