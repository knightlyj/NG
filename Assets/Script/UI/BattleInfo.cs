using UnityEngine;
using System.Collections;

public class BattleInfo : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        damageTextPool = new Transform[damagePoolSize];
        for(int i = 0; i < damagePoolSize; i++)
        {
            damageTextPool[i] = Instantiate(damageTextPrefab, this.transform) as Transform;
            damageTextPool[i].gameObject.SetActive(false);
        }
        damagePoolCount = damagePoolSize;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        for (int i = 0; i < damagePoolCount; i++)
        {
            Destroy(damageTextPool[i].gameObject);
        }
    }

    //伤害数字
    public Transform damageTextPrefab;
    Transform[] damageTextPool;
    readonly int damagePoolSize = 30;
    int damagePoolCount;
    public void AddDamageText(Vector2 pos, int damage, DamageText.DamageStyle style)
    {
        if (damagePoolCount <= 0)
        {
            NewDamageText(pos, damage, style);
        }
        else
        {
            Transform d = damageTextPool[damagePoolCount - 1];
            damageTextPool[damagePoolCount - 1] = null;
            damagePoolCount--;
            DamageText txt = d.GetComponent<DamageText>();
            if (txt != null)
                txt.SetDamage(pos, damage, style);
            else
                Debug.LogError("BattleInfo.AddDamageText >> no DamageText component");
        }
    }

    public void CollectDamageText(Transform t)
    {
        if(damagePoolCount < damagePoolSize)
        {
            damageTextPool[damagePoolCount++] = t;
        }
        else
        {
            Destroy(t.gameObject);
        }
    }

    public void NewDamageText(Vector2 pos, int damage, DamageText.DamageStyle style)
    {
        Transform d = Instantiate(damageTextPrefab, this.transform) as Transform;
        DamageText txt = d.GetComponent<DamageText>();
        txt.SetDamage(pos, damage, style);
    }

    //生命条
    public Transform hpBarPrefab;
    public HpBar AddHpBar(Transform owner, float offset)
    {
        Transform h = Instantiate(hpBarPrefab, this.transform) as Transform;
        HpBar b = h.GetComponent<HpBar>();
        b.SetOwner(owner, offset);
        return b;
    }
}
