using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIStoreWnd : MonoBehaviour
{
    //布局用参数
    public Transform slotPrefab;
    public int slotToSide;
    public int slotGap;
    public int slotSize;
    public const int slotAmount = 40;

    UIStoreSlot[] slots = new UIStoreSlot[slotAmount];

    void Awake()
    {
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
    void Start()
    {
        StoreInfo info = new StoreInfo();
        for (int i = 1; i <= 3; i++)
        {
            info.AddCommodity(new Commodity(new Item(i, 5)));
        }
        this.SetStoreInfo(info);
    }

    void OnDestroy()
    {
        foreach (UIItemSlot slot in slots)
        {
            slot.MouseDownEvent -= this.OnItemMouseDown;
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    void SetStoreInfo(StoreInfo info)
    {
        this.storeInfo = info; //记录数据
        //更新到界面
        int count = 0;
        foreach (Commodity comm in info)
        {
            slots[count++].SetItemInfo(comm.item);
        }
    }
}
