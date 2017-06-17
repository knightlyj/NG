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
}

public class ItemCell : MonoBehaviour , ListCell
{
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetContent(object content)
    {
        Item item = content as Item;
        if(item == null)
        {
            Debug.LogError("ItemCell.SetContent >> content error");
            return;
        }

        Image imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        Text txtName = transform.FindChild("Name").GetComponent<Text>();
        
        imgIcon.sprite = Resources.Load<Sprite>(item.Type.itemName);
        txtName.text = item.Type.itemName;
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
