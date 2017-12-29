using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIStoreWnd : GameWindow
{
    //布局用参数
    public Transform slotPrefab;
    public int slotToSide;
    public int slotGap;
    public int slotSize;
    public const int slotAmount = 40;

    UIStoreSlot[] slots = new UIStoreSlot[slotAmount];

    public new void Awake()
    {
        base.Awake();
        Transform trStore = transform.FindChild("Bg");
        //8行5列
        for (int i = 0; i < slotAmount; i++)
        {
            slots[i] = trStore.FindChild("Slot" + i).GetComponent<UIStoreSlot>();
            slots[i].index = i;
            slots[i].MouseDownEvent += this.OnItemMouseDown;
        }
    }

    StoreInfo storeInfo = null;
    // Use this for initialization
    public new void Start()
    {
        base.Start();
        StoreInfo info = new StoreInfo();
        for (int i = 1; i <= 3; i++)
        {
            info.AddCommodity(new Commodity(new Item(i, 5)));
        }
        this.SetStoreInfo(info);
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        foreach (UIItemSlot slot in slots)
        {
            slot.MouseDownEvent -= this.OnItemMouseDown;
        }
    }

    //点下物品格
    void OnItemMouseDown(UIItemSlot slot, PointerEventData eventData)
    {
        LocalPlayer player = Helper.FindLocalPlayer();
        if (player != null)
        {
            PlayerBag bag = player.bag;
            if (eventData.button == PointerEventData.InputButton.Right)
            { //点击右键购买

            }
        }

        Helper.MoveWndToFront(transform);
    }

    public void SetStoreInfo(StoreInfo info)
    {
        //清除原来的数据
        if (this.storeInfo != null)
        {
            for (int i = 0; i < this.storeInfo.Count; i++)
            {
                slots[i].ClearItem();
            }
        }

        
        this.storeInfo = info; //记录数据
        if (info != null)
        {
            //更新到界面
            int count = 0;
            foreach (Commodity comm in info)
            {
                slots[count++].SetItemInfo(comm.item);
            }
        }
    }
}
