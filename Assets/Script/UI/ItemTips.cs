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
        Item it = new Item(sp.table.GetItemType(ItemId.Gold), 10);
        ShowTips(it, new Vector2());
    }

    const float maxDim = 1.0f;
    const float minDim = 0.5f;
    const float fadeSpeed = (maxDim - minDim) * 1.5f;
    float fadeDir = -1;
    float dim = 1.0f;
    //Update is called once per frame
    void Update()
    {
        dim += fadeDir * fadeSpeed * Time.deltaTime;
        if (dim > maxDim || dim < minDim)
        {
            fadeDir *= -1;
        }
        Material material = txt.material;
        material.color = new Color(dim, dim, dim, 1);
    }

    public Vector2 offset = new Vector2(10, 10); //加上一个偏移,不被鼠标挡住
    public void ShowTips(Item item, Vector2 pos)
    {
        //Debug.Log(pos);
        if(item == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            GenTips(item); //生成提示
            //位置选择,优先放在当前位置右下角,不够的话再左边和上面
            bool right = true, down = true;
            if(pos.x + txt.preferredWidth + offset.x > Screen.width)
            {
                right = false;
            }
            if((Screen.height + pos.y - offset.y) < txt.preferredHeight)
            {
                down = false;
            }
            RectTransform rect = transform as RectTransform;
            Vector2 min = new Vector2(), max = new Vector2();
            if (right)
            {
                min.x = pos.x + offset.x;
            }
            else
            {
                min.x = pos.x - txt.preferredWidth - offset.x;
            }
            if (down)
            {
                min.y = pos.y - txt.preferredHeight - offset.y;
            }
            else
            {
                min.y = pos.y + offset.y;
            }
            max = min + new Vector2(txt.preferredWidth, txt.preferredHeight);
            rect.offsetMin = min;
            rect.offsetMax = max;
        }
    }

    void GenTips(Item item)
    {
        //name
        StringBuilder build = new StringBuilder();
        switch (item.Type.quality)
        {
            case ItemQuality.White:
                build.Append("<color=white>" + item.Type.name);
                break;
            case ItemQuality.Green:
                build.Append("<color=green>" + item.Type.name);
                break;
            case ItemQuality.Blue:
                build.Append("<color=blue>" + item.Type.name);
                break;
            case ItemQuality.Golden:
                build.Append("<color=orange>" + item.Type.name);
                break;
            case ItemQuality.Red:
                build.Append("<color=red>" + item.Type.name);
                break;
            case ItemQuality.Purple:
                build.Append("<color=purple>" + item.Type.name);
                break;
            default:
                build.Append("<color=white>" + item.Type.name); //默认用白色
                break;
        }
        

        //叠加数量
        if (item.Type.CanStack)
        {
            build.Append(" (" + item.amount + ")");
        }
        build.Append("</color>");
        build.Append("\n");

        //这里附加装备信息

        //comment
        build.Append(item.Type.comment);
        build.Append("\n");

        //usability
        if (item.Type.IsArmor || item.Type.IsWeapon)
        {
            build.Append("equipment");
        }

        if (item.Type.IsConsumable)
        {
            build.Append("&consumable");
        }

        if (item.Type.IsMaterial)
        {
            build.Append("&material");
        }
        txt.text = build.ToString();
    }
    
    //void GenWeaponTips(Item item)
    //{

    //}
    
}
