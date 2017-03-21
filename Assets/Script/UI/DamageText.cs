using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    float fontSize = defaultScreen.damageFontSize;
	// Use this for initialization
	void Start () {
        fontSize = (int)((float)Screen.height / defaultScreen.height * defaultScreen.damageFontSize);
        Text txt = GetComponent<Text>();
        txt.fontSize = (int)this.fontSize;
    }

    //向上飘的速度
    float upSpeed = 0.5f;
    //持续生命期,最后一段时间fade out
    float lifeTime = 2;
    const float maxLifeTime = 2;
    const float fadeTime = 0.5f;
    //伤害数字跳出效果,在maxScaleTime内从最小scale放大到最大scale
    float scaleTime = 0;
    const float maxScaleTime = 0.2f;
    const float minScale = 0.5f;
    float maxScale = 1.0f;
    // Update is called once per frame
    void Update () {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            this.gameObject.SetActive(false);
            BattleInfo battle = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
            battle.CollectDamageText(this.transform);
            return;
        }

        position.y += upSpeed * Time.deltaTime;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(position);
        RectTransform rec = this.transform as RectTransform;
        rec.position = screenPos;

        if (lifeTime < fadeTime)
        {
            float alpha = lifeTime / fadeTime;
            Text txt = GetComponent<Text>();
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);
        }

        if (transform.localScale.x < maxScale)
        {
            scaleTime += Time.deltaTime;
            float scaleRate = scaleTime / maxScaleTime * (maxScale - minScale) + minScale;
            if (scaleRate > maxScale || Mathf.Abs(scaleRate - maxScale) < 0.1)
                scaleRate = maxScale;
            transform.localScale = new Vector3(scaleRate, scaleRate, scaleRate);
        }
    }


    public enum DamageStyle
    {
        Nothing,
        Normal,
        Critical,
        Heal,
    }


    Vector2 position;
    public void SetDamage(Vector2 pos, int damage, bool critical)
    {
        this.position = pos + new Vector2(0, 0.3f);
        Text txt = GetComponent<Text>();
        if (txt == null)
            return;

        txt.text = damage.ToString(); //伤害数字
        if (damage < 0)
        {   // 伤害
            txt.color = Color.green;
        }
        else
        {   //治疗
            txt.color = new Color(0.5f, 0, 0);
        }
        if (critical)
        {   //暴击
            txt.text += "!";
            maxScale = 1.5f;
        }
        else
        {   //普通伤害
            maxScale = 1.0f;
        }

        //开始运行的初值
        lifeTime = maxLifeTime;
        scaleTime = 0;
        transform.localScale = new Vector3(minScale, minScale, minScale);

        this.gameObject.SetActive(true);
    }
}
