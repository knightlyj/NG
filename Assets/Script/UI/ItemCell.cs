using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemCell : MonoBehaviour {
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    static int count = 0;
    public void SetContent(Item item)
    {
        Text txtName = transform.FindChild("Name").GetComponent<Text>();
        //txtName.text = item.Type.name;
        txtName.text = count.ToString();
        count++;
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
