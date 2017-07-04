using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class UIItemTips : MonoBehaviour {
    Text txt;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        this.gameObject.SetActive(false);

    }

    const float maxDim = 1.0f;
    const float minDim = 0.5f;
    const float fadeSpeed = (maxDim - minDim) * 1.5f;
    float fadeDir = -1;
    float dim = 1.0f;
    //Update is called once per frame
    void Update()
    {
        //明暗变化
        dim += fadeDir * fadeSpeed * Time.deltaTime;
        if (dim > maxDim || dim < minDim)
        {
            fadeDir *= -1;
        }
        Material material = txt.material;
        material.color = new Color(dim, dim, dim, 1);
        
        //跟随鼠标
        Vector2 pos = Input.mousePosition;
        pos = Helper.UnityUIPos2WindowsPos(pos);
        AdaptPos(pos);
    }

    public Vector2 offset = new Vector2(10, 10); //加上一个偏移,不被鼠标挡住
    public void ShowTips(Item item)
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
        }
    }

    //采用windows窗口坐标系坐标
    void AdaptPos(Vector2 pos)
    {
        //位置选择,优先放在当前位置右下角,不够的话再左边和上面
        bool right = true, down = true;
        if (pos.x + txt.preferredWidth + offset.x > Screen.width)
        {
            right = false;
        }
        if ((Screen.height + pos.y - offset.y) < txt.preferredHeight)
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
            min.x = pos.x - txt.preferredWidth;
        }
        if (down)
        {
            min.y = pos.y - txt.preferredHeight - offset.y;
        }
        else
        {
            min.y = pos.y;
        }
        max = min + new Vector2(txt.preferredWidth, txt.preferredHeight);
        rect.offsetMin = min;
        rect.offsetMax = max;
    }

    void GenTips(Item item)
    {
        //name
        StringBuilder build = new StringBuilder();
        switch (item.Type.quality)
        {
            case ItemQuality.White:
                build.Append("<color=white>" + item.Type.itemName);
                break;
            case ItemQuality.Green:
                build.Append("<color=green>" + item.Type.itemName);
                break;
            case ItemQuality.Blue:
                build.Append("<color=blue>" + item.Type.itemName);
                break;
            case ItemQuality.Golden:
                build.Append("<color=orange>" + item.Type.itemName);
                break;
            case ItemQuality.Red:
                build.Append("<color=red>" + item.Type.itemName);
                break;
            case ItemQuality.Purple:
                build.Append("<color=purple>" + item.Type.itemName);
                break;
            default:
                build.Append("<color=white>" + item.Type.itemName); //默认用白色
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
        if (item.Type.IsEquipment)
        {
            build.Append("equipment");
        }

        if (item.Type.IsConsumable)
        {
            build.Append("&consumable");
        }

        //if (item.Type.IsMaterial)
        //{
        //    build.Append("&material");
        //}
        txt.text = build.ToString();
    }
    
    //void GenWeaponTips(Item item)
    //{

    //}
    
}
