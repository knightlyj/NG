using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public interface ListCell
{
    float GetHeight();
    void SetContent(object content);
    void Seleted(bool sel);
    int index { get; set; }
}

public class UICraftItemCell : MonoBehaviour, ListCell
{
    int _idx = -1;
    public int index
    {
        get { return _idx; }
        set { _idx = value; }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public CraftFormula formula = null;
    public void SetContent(object content)
    {
        formula = content as CraftFormula;
        if (formula == null)
        {
            Debug.LogError("ItemCell.SetContent >> content error");
            return;
        }

        Image imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        Text txtName = transform.FindChild("Name").GetComponent<Text>();

        ItemType type = ItemTypeTable.GetItemType(formula.outputId);
        imgIcon.sprite = Resources.Load<Sprite>("ItemIcon/" + type.icon);
        imgIcon.SetNativeSize();
        txtName.text = type.itemName;
    }

    public float GetHeight()
    {
        //RectTransform rect = this.transform as RectTransform;
        return 25;// rect.offsetMax.y - rect.offsetMin.y;
    }

    public void Seleted(bool sel)
    {
        Image img = GetComponent<Image>();
        if (sel)
        {
            img.color = Helper.HexToColor(0x4A766E);
        }
        else
        {
            img.color = Color.white;
        }
    }
}
