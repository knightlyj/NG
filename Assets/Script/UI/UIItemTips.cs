using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class UIItemTips : MonoBehaviour
{
    public enum ShowPrice
    {
        None,
        Buy,
        Sell,
    }

    Text txt;
    // Use this for initialization
    void Start()
    {
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
        if (dim > maxDim)
            dim = maxDim;
        else if (dim < minDim)
            dim = minDim;
        if (dim >= maxDim || dim <= minDim)
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
    public void ShowTips(Item item, ShowPrice showPrice = ShowPrice.None)
    {
        //Debug.Log(pos);
        if (item == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            UIMouseItem uiMouseItem = Helper.GetUIMouseItem();
            if (!uiMouseItem.hasItem) //鼠标没有挂物品时,才显示提示
            {   
                gameObject.SetActive(true);
                GenTips(item, showPrice); //生成提示
            }
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

    void GenTips(Item item, ShowPrice showPrice = ShowPrice.None)
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
        if (item.Type.IsWeapon)
        {
            WeaponProperties weaponProp = EquipTable.GetWeaponProp(item.Type.weaponId);
            if (weaponProp == null)
            {

            }
            else
            {
                build.Append("<color=white>");
                //攻击
                build.Append(string.Format(TextResources.atkFormat, weaponProp.minAtkBonus, weaponProp.maxAtkBonus));
                //攻击速率,为间隔的倒数
                build.Append(string.Format(TextResources.atkInvervalFormat, 1 / weaponProp.atkInterval));
                if (weaponProp.crtlChanceBonus != 0)
                { //暴击率
                    build.Append(string.Format(TextResources.crtlChanceFormat, Mathf.Round(weaponProp.crtlChanceBonus * 100)));
                }
                if (weaponProp.crtlRateBonus != 0)
                {  //暴击伤害
                    build.Append(string.Format(TextResources.crtlRateFormat, Mathf.Round(weaponProp.crtlRateBonus * 100)));
                }
                if (weaponProp.rcrBonus != 0)
                {   //rcr
                    build.Append(string.Format(TextResources.rcrFormat, Mathf.Round(weaponProp.rcrBonus * 100)));
                }
                build.Append("</color>");
            }
        }
        else if (item.Type.IsArmor)
        {
            ArmorProperties armorProp = EquipTable.GetArmorProp(item.Type.armorId);
            if (armorProp == null)
            {

            }
            else
            {
                build.Append("<color=white>");
                //防御
                build.Append(string.Format(TextResources.defFormat, armorProp.defBonus));
                if (armorProp.hpBonus != 0)
                {   //血量
                    build.Append(string.Format(TextResources.hpFormat, armorProp.hpBonus));
                }
                if (armorProp.minAtkBonus != 0 || armorProp.maxAtkBonus != 0)
                {   //攻击
                    build.Append(string.Format(TextResources.atkFormat, armorProp.minAtkBonus, armorProp.maxAtkBonus));
                }
                if (armorProp.atkSpdBonus != 0)
                {   //攻速
                    build.Append(string.Format(TextResources.atkSpdFormat, Mathf.Round((armorProp.atkSpdBonus * 100))));
                }
                if (armorProp.spdScaleBonus != 0)
                {   //速度
                    build.Append(string.Format(TextResources.spdFormat, Mathf.Round(armorProp.spdScaleBonus * 100)));
                }
                if (armorProp.jmpScaleBonus != 0)
                {   //跳跃
                    build.Append(string.Format(TextResources.jmpFormat, Mathf.Round(armorProp.jmpScaleBonus * 100)));
                }
                if (armorProp.crtlChanceBonus != 0)
                {   //暴击率
                    build.Append(string.Format(TextResources.crtlChanceFormat, Mathf.Round(armorProp.crtlChanceBonus * 100)));
                }
                if (armorProp.crtlRateBonus != 0)
                {   //暴击伤害
                    build.Append(string.Format(TextResources.crtlRateFormat, Mathf.Round(armorProp.crtlRateBonus * 100)));
                }
                if (armorProp.rcrBonus != 0)
                {   //rcr
                    build.Append(string.Format(TextResources.rcrFormat, Mathf.Round(armorProp.rcrBonus * 100)));
                }
                build.Append("</color>");
            }
        }

        //附魔属性

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

        //价格显示
        switch (showPrice)
        {
            case ShowPrice.None:
                break;
            case ShowPrice.Buy:
                build.Append("\n");
                build.Append("<color=yellow>");
                build.Append("买价: " + item.buyPrice);
                build.Append("</color>");
                break;
            case ShowPrice.Sell:
                build.Append("\n");
                build.Append("<color=white>");
                build.Append("卖价: " + item.sellPrice);
                build.Append("</color>");
                break;
        }

        txt.text = build.ToString();
    }

    //void GenWeaponTips(Item item)
    //{

    //}

}
