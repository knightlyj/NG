using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICraftWnd : MonoBehaviour
{
    UIScrollPanel craftClassPanel;
    UIScrollPanel craftItemPanel;
    System.Object[] crafgClassInfo;
    UIMatSlot[] matSlot = new UIMatSlot[CraftFormula.maxRawMatSorts];
    UIMatSlot productSlot = null;
    Button btnCraft;

    void Awake()
    {
        //制造按钮
        btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.AddListener(this.OnCraftClick);

        //合成类型
        craftClassPanel = transform.FindChild("Bg").FindChild("ClassBg").FindChild("ClassPanel").GetComponent<UIScrollPanel>();
        //合成物品
        craftItemPanel = transform.FindChild("Bg").FindChild("ItemBg").FindChild("ItemPanel").GetComponent<UIScrollPanel>();
        //slots
        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
        {
            matSlot[i] = transform.FindChild("Bg").FindChild("MatBg").FindChild("MatSlot" + i).GetComponent<UIMatSlot>();
        }
        productSlot = transform.FindChild("Bg").FindChild("Product").GetComponent<UIMatSlot>();

        
    }

    // Use this for initialization
    void Start()
    {
        Player localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
            BindBag(localPlayer.bag);
        //订阅本地玩家创建事件
        EventManager.AddListener(EventId.LocalPlayerCreate, this.OnLocalPlayerCreate);
        //订阅本地玩家销毁事件
        EventManager.AddListener(EventId.LocalPlayerDestroy, this.OnLocalPlayerDestroy);

        //合成类型列表
        crafgClassInfo = new System.Object[ItemTypeTable.className.Count];
        for (int i = 0; i < crafgClassInfo.Length; i++)
        {
            CraftClassCellInfo info = new CraftClassCellInfo();
            info.icon = ItemTypeTable.classIcon[i];
            info.name = ItemTypeTable.className[i];
            crafgClassInfo[i] = (System.Object)info;
        }
        craftClassPanel.SetList(crafgClassInfo);
        craftClassPanel.OnCellSelected += this.OnClassClick;
        //合成物品列表
        craftItemPanel.OnCellSelected += this.OnItemClick;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        //制造按钮
        btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.RemoveAllListeners();
        //类型选择列表
        craftClassPanel.OnCellSelected -= this.OnClassClick;
        //合成物品
        craftItemPanel.OnCellSelected -= this.OnItemClick;

        UnbindBag();
        //退订本地玩家创建事件
        EventManager.RemoveListener(EventId.LocalPlayerCreate, this.OnLocalPlayerCreate);
        //退订本地玩家销毁事件
        EventManager.RemoveListener(EventId.LocalPlayerDestroy, this.OnLocalPlayerDestroy);
    }

    //选中分类
    void OnClassClick(ListCell cell)
    {
        UICraftClassCell craftClass = cell as UICraftClassCell;
        if (craftClass == null)
        {
            Debug.LogError("CraftWnd.OnClassClick error");
            return;
        }
        //Debug.Log(cell.index);
        if (cell.index >= ItemTypeTable.craftFormulas.Length)
            return;
        CraftFormula[] list = new CraftFormula[ItemTypeTable.craftFormulas[cell.index].Count];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = ItemTypeTable.craftFormulas[cell.index][i];
        }
        craftItemPanel.SetList(list);
    }

    //选中物品
    UICraftItemCell selCraftItem = null;
    void OnItemClick(ListCell cell)
    {
        UICraftItemCell craftItem = cell as UICraftItemCell;
        if (craftItem == null)
        {
            Debug.LogError("CraftWnd.OnItemClick error");
            return;
        }
        if (craftItem.formula == null || craftItem.formula.rawMats == null)
        {
            for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
            {
                matSlot[i].SetMaterial(null, 0);
            }
            productSlot.SetMaterial(null, 0);
        }

        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
        {
            if (craftItem.formula.rawMats[i] != null)
            {
                ItemType matType = ItemTypeTable.GetItemType((ItemId)craftItem.formula.rawMats[i].id);
                matSlot[i].SetMaterial(matType, craftItem.formula.rawMats[i].amount);
            }
            else
            {
                matSlot[i].SetMaterial(null, 0);
            }
            ItemType productType = ItemTypeTable.GetItemType((ItemId)craftItem.formula.outputId);
            productSlot.SetMaterial(productType, craftItem.formula.outputAmount);
        }
        selCraftItem = craftItem;
        ShowMatEnough();
    }

    //点击制造
    void OnCraftClick()
    {
        Helper.MoveWndToFront(this.transform); //窗口移动到前面

        //获取当前使用的合成公式
        CraftFormula formula = GetCurFormula();
        if (formula == null)
            return;

        for (int i = 0; i < formula.matCount; i++)
        {
            if (!bindBag.itemPack.ItemEnough(formula.rawMats[i].id, formula.rawMats[i].amount))
            {   //有一种材料不够
                return;
            }
        }

        for (int i = 0; i < formula.matCount; i++)
        {
            bindBag.itemPack.RemoveAmount(formula.rawMats[i].id, formula.rawMats[i].amount);
        }

        Item product = new Item((ItemId)formula.outputId, formula.outputAmount);
        if (!bindBag.itemPack.PickUpItem(product))
        { //如果捡东西失败,丢到地上
            Helper.DropItemByPlayer(product);
        }
    }

    PlayerBag bindBag = null;
    void BindBag(PlayerBag bag)
    {
        //先解绑
        UnbindBag();
        if (bag == null)
            return;
        //绑定到新的package
        bindBag = bag;
        //暂时没考虑背包关闭的问题
        bindBag.itemPack.PackChangedEvent += this.OnPackChanged; //item包改变
    }

    void UnbindBag()
    {
        if (bindBag != null)
        {
            bindBag.itemPack.PackChangedEvent -= this.OnPackChanged;
            bindBag = null;
        }
    }

    //背包物品有变化时的回调
    void OnPackChanged()
    {
        ShowMatEnough();
    }

    /// <summary>
    /// 根据材料是否足够,改变材料和按钮显示
    /// </summary>
    void ShowMatEnough()
    {
        //获取当前使用的合成公式
        CraftFormula formula = GetCurFormula();
        if (formula == null)
            return;
        
        for (int i = 0; i < formula.matCount; i++)
        {
            if (bindBag.itemPack.ItemEnough(formula.rawMats[i].id, formula.rawMats[i].amount))
            {   //材料足够
                matSlot[i].SetMatEnough(true);
            }
            else
            {  //材料不够
                matSlot[i].SetMatEnough(false);
            }
        }
    }

    CraftFormula GetCurFormula()
    {
        if (selCraftItem == null || bindBag == null)
            return null;

        return selCraftItem.formula;
    }

    void OnLocalPlayerCreate(System.Object sender)
    {
        Player localPlayer = sender as Player;
        if(localPlayer != null)
        {
            BindBag(localPlayer.bag);
        }
    }

    void OnLocalPlayerDestroy(System.Object sender)
    {
        UnbindBag();
    }
}
