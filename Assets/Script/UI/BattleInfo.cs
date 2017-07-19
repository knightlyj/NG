using UnityEngine;
using System.Collections;

public class BattleInfo : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        //伤害数字
        damageTextPool = new Transform[damagePoolSize];
        for(int i = 0; i < damagePoolSize; i++)
        {
            damageTextPool[i] = Instantiate(damageTextPrefab, this.transform) as Transform;
            damageTextPool[i].gameObject.SetActive(false);
        }
        damagePoolCount = damagePoolSize;

        //本地玩家血条
        LocalHpBarInit();
        //本机玩家弹药条
        LocalAmmoInit();
    }
	
	// Update is called once per frame
	void Update () {
        //本机玩家弹药和血条更新
        localPlayer = Helper.FindLocalPlayer(); //每次都查找,在helper内部有localpalyer缓存
        LocalHpBarUpdate();
        LocalAmmoUpdate();
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
    public void AddDamageText(Vector2 pos, int damage, bool critical)
    {
        if (damagePoolCount <= 0)
        {
            NewDamageText(pos, damage, critical);
        }
        else
        {
            Transform d = damageTextPool[damagePoolCount - 1];
            damageTextPool[damagePoolCount - 1] = null;
            damagePoolCount--;
            DamageText txt = d.GetComponent<DamageText>();
            if (txt != null)
                txt.SetDamage(pos, damage, critical);
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

    public void NewDamageText(Vector2 pos, int damage, bool critical)
    {
        Transform d = Instantiate(damageTextPrefab, this.transform) as Transform;
        DamageText txt = d.GetComponent<DamageText>();
        txt.SetDamage(pos, damage, critical);
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

    //本机玩家血条
    void LocalHpBarInit()
    {
        localHpBar = transform.FindChild("LocalHp").FindChild("HpBar") as RectTransform;
        if(localHpBar == null)
        {
            Debug.LogError("BattleInfo.LocalHpBarInit >> localHpBar is null");
        }
    }

    RectTransform localHpBar = null;
    Player localPlayer = null;
    void LocalHpBarUpdate()
    {
        if(localPlayer == null)
        { //血条设置为空,并寻找本地玩家
            localHpBar.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            PlayerProperties prop = localPlayer.Properties;
            float hpRate = prop.hp / prop.maxHp;
            if (hpRate > 1)
                hpRate = 1;
            localHpBar.localScale = new Vector3(hpRate, 1, 1);
        }
    }

    //本机玩家弹药显示
    void LocalAmmoInit()
    {
        localAmmoBar = transform.FindChild("LocalAmmo").FindChild("AmmoBar") as RectTransform;
        if (localAmmoBar == null)
        {
            Debug.LogError("BattleInfo.LocalHpBarInit >> localAmmoBar is null");
        }
    }

    RectTransform localAmmoBar = null;
    void LocalAmmoUpdate()
    {
        if (localPlayer == null)
        { //没有本地玩家,不更新弹药显示
            //localAmmoBar.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            //Weapon w = localPlayer.CurWeapon;
            //float ammoRate = 0;
            //if (w != null && w.Properties.maxAmmo != 0)
            //{
            //    ammoRate = (float)w.Properties.curAmmor / w.Properties.maxAmmo;
            //}
            //if (ammoRate > 1)
            //    ammoRate = 1;
            //localAmmoBar.localScale = new Vector3(ammoRate, 1, 1);
        }
    }
}
