using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftClassCellInfo
{
    public string name;
    public string icon;
}

public class CraftClassCell : MonoBehaviour, ListCell
{
    int _idx = -1;
    public int index
    {
        get { return _idx; }
        set { _idx = value; }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetContent(object content)
    {
        CraftClassCellInfo info = content as CraftClassCellInfo;
        if (info == null)
        {
            Debug.LogError("CraftCell.SetContent >> content error");
            return;
        }

        Image imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        Text txtName = transform.FindChild("Name").GetComponent<Text>();
        
        imgIcon.sprite = Resources.Load<Sprite>("ItemIcon/" + info.icon);
        imgIcon.SetNativeSize();
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
