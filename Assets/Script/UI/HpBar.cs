using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
    Transform owner;
    float offsetY = -0.5f;
    float scale;
    // Use this for initialization
    void Start () {
        scale = (float)Screen.height / GameSetting.defaultScreenheight;
        transform.localScale = new Vector3(scale, scale, scale);
    }
	
	// Update is called once per frame
	void Update () {
        if (owner == null)
            return;
        if (!owner.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
            return;
        }
        Vector2 position = this.owner.position;
        position.y += offsetY;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(position);
        RectTransform rec = this.transform as RectTransform;
        rec.position = screenPos;
    }

    public bool destroyed = false;
    void OnDestroy()
    {
        destroyed = true;
    }

    public void SetOwner(Transform owner, float offset)
    {
        this.owner = owner;
        this.offsetY = offset;
    }

    public void SetHpRate(float rate)
    {
        if (rate < 0 || rate > 1)
            return;
        Transform hp = transform.FindChild("Hp");
        hp.localScale = new Vector3(scale * rate, 1, 1);
        Image img = hp.GetComponent<Image>();
        img.color = new Color(1, rate, rate, img.color.a);
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
