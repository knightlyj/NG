using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftCellInfo
{
    public string name;
    public string icon;
}

public class CraftCell : MonoBehaviour, ListCell
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetContent(object content)
    {
        CraftCellInfo info = content as CraftCellInfo;
        if (info == null)
        {
            Debug.LogError("CraftCell.SetContent >> content error");
            return;
        }

        Image imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        Text txtName = transform.FindChild("Name").GetComponent<Text>();
        
        imgIcon.sprite = Resources.Load<Sprite>(info.icon);
        txtName.text = info.name;
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
