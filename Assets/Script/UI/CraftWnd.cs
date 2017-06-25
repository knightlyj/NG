using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftWnd : MonoBehaviour
{
    ScrollPanel craftClassPanel;
    ScrollPanel craftItemPanel;
    System.Object[] crafgClassInfo;
    UIMatSlot[] matSlot = new UIMatSlot[CraftFormula.maxRawMatSorts];
    UIMatSlot productSlot = null;
    // Use this for initialization
    void Start()
    {
        //制造按钮
        Button btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.AddListener(this.OnCraftClick);
        //合成类型
        craftClassPanel = transform.FindChild("Bg").FindChild("ClassBg").FindChild("ClassPanel").GetComponent<ScrollPanel>();
        crafgClassInfo = new System.Object[ItemTypeTable.className.Length];
        for (int i = 0; i < crafgClassInfo.Length; i++)
        {
            CraftClassCellInfo info = new CraftClassCellInfo();
            info.icon = ItemTypeTable.classIcon[i];
            info.name = ItemTypeTable.className[i];
            crafgClassInfo[i] = (System.Object)info;
        }
        craftClassPanel.SetList(crafgClassInfo);
        craftClassPanel.OnCellSelected += this.OnClassClick;

        //合成物品
        craftItemPanel = transform.FindChild("Bg").FindChild("ItemBg").FindChild("ItemPanel").GetComponent<ScrollPanel>();
        craftItemPanel.OnCellSelected += this.OnItemClick;

        //slots
        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
        {
            matSlot[i] = transform.FindChild("Bg").FindChild("MatBg").FindChild("MatSlot" + i).GetComponent<UIMatSlot>();
        }
        productSlot = transform.FindChild("Bg").FindChild("Product").GetComponent<UIMatSlot>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        //制造按钮
        Button btnCraft = transform.FindChild("Craft").GetComponent<Button>();
        btnCraft.onClick.RemoveAllListeners();
        //类型选择列表
        craftClassPanel.OnCellSelected -= this.OnClassClick;
        //合成物品
        craftItemPanel.OnCellSelected -= this.OnItemClick;
    }

    void OnCraftClick()
    {
        Debug.Log("craft");
    }

    void OnClassClick(ListCell cell)
    {
        CraftClassCell craft = cell as CraftClassCell;
        if (craft == null)
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

    void OnItemClick(ListCell cell)
    {
        CraftItemCell craft = cell as CraftItemCell;
        if (craft == null)
        {
            Debug.LogError("CraftWnd.OnItemClick error");
            return;
        }
        if(craft.formula == null || craft.formula.rawMats == null)
        {
            for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
            {
                matSlot[i].SetMaterial(null, 0);
            }
            productSlot.SetMaterial(null, 0);
        }

        for (int i = 0; i < CraftFormula.maxRawMatSorts; i++)
        {
            if (craft.formula.rawMats[i] != null)
            {
                ItemType matType = ItemTypeTable.GetItemType((ItemId)craft.formula.rawMats[i].id);
                matSlot[i].SetMaterial(matType, craft.formula.rawMats[i].amount);
            }
            else
            {
                matSlot[i].SetMaterial(null, 0);
            }
            ItemType productType = ItemTypeTable.GetItemType((ItemId)craft.formula.outputId);
            productSlot.SetMaterial(productType, craft.formula.outputAmount);
        }
    }
}
