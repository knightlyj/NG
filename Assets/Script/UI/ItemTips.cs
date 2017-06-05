using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class ItemTips : MonoBehaviour {
    Text txt;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        Spawner sp = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        Item it = new Item(sp.table.GetItemType(ItemId.Wood), 10);
        ShowTips(it, new Vector2());

    }

    //const float maxDim = 1.0f;
    //const float minDim = 0.5f;
    //const float fadeSpeed = (maxDim - minDim) * 1.5f;
    //float fadeDir = -1;
    //float dim = 1.0f;
	// Update is called once per frame
	void Update () {
        //dim += fadeDir * fadeSpeed * Time.deltaTime;
        //if(dim > maxDim || dim < minDim)
        //{
        //    fadeDir *= -1;
        //}
        //txt.color = new Color(dim, dim, dim, 1);
	}

    public void ShowTips(Item item, Vector2 pos)
    {
        if(item == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            string content = GenTips(item);
            txt.text = content;
        }
    }

    string GenTips(Item item)
    {
        StringBuilder build = new StringBuilder();
        build.Append("");
        switch (item.Type.quality)
        {
            case ItemQuality.White:
                build.Append("<color=white>" + item.Type.name + "</color>");
                break;
            case ItemQuality.Green:
                build.Append("<color=green>" + item.Type.name + "</color>");
                break;
            case ItemQuality.Blue:
                build.Append("<color=blue>" + item.Type.name + "</color>");
                break;
            case ItemQuality.Golden:
                build.Append("<color=orange>" + item.Type.name + "</color>");
                break;
            case ItemQuality.Red:
                build.Append("<color=red>" + item.Type.name + "</color>");
                break;
            default:
                build.Append("<color=white>" + item.Type.name + "</color>"); //默认用白色
                break;
        }

        if (item.Type.CanStack)
        {
            build.Append(" (" + item.amount + ")");
        }
        build.Append("\n");

        //这里附加装备信息

        build.Append(item.Type.comment);
        build.Append("\n");

        if(item.Type.IsArmor || item.Type.IsWeapon)
        {
            build.Append("equipment");
        }

        if (item.Type.IsConsumable)
        {
            build.Append(" & consumable");
        }

        if (item.Type.IsMaterial)
        {
            build.Append("& material");
        }
        return build.ToString();
    }

    //void GenWeaponTips(Item item)
    //{

    //}
    
}
