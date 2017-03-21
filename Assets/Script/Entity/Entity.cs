using UnityEngine;
using System.Collections;

public class EntityProperties
{
    public int hp = 100;
    public int maxHp = 100;
    public int minAttack = 10;
    public int maxAttack = 20;
    public int defense = 0;
    public float speedScale = 1.0f;
    public float jumpScale = 1.0f;
    public float atkInterval = 1.0f;
    public float atkSpeed = 1.0f;
    public float criticalChance = 0.05f;
    public float criticalRate = 2.0f;
    public float cdr = 0;
    public float rcr = 0;
    public bool invincible = false;

    public static int CalcDamage(EntityProperties from, EntityProperties to, out bool critical)
    {
        //减去防御
        int damage = Random.Range(from.minAttack, from.maxAttack) - to.defense;
        if (damage <= 0) //至少1点伤害
            damage = 1;

        //计算暴击
        float r = Random.Range(0.0f, 1f);
        if (r <= from.criticalChance)
        {
            damage = (int)(damage * from.criticalRate);
            critical = true;
        }
        else
        {
            critical = false;
        }

        return damage;
    }
}

public class Entity : MonoBehaviour {
    public bool isLocal = true;
    protected Rigidbody2D rb = null;
	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();

        BattleInfo battle = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
        hpBar = battle.AddHpBar(this.transform, hpBarOffset);
    }
    
    // Update is called once per frame
    protected virtual void Update () {
        //buff模块
        if(buffModule != null)
            buffModule.UpateBuff(Time.deltaTime);
        
        //血条更新 
        if(hpBar)
            hpBar.SetHpRate((float)Properties.hp / Properties.maxHp);
    }

    protected virtual void OnDestroy()
    {
        if (!hpBar.destroyed)
        {
            hpBar.Delete();
            hpBar = null;
        }

        //
        buffModule.ClearBuff();
        buffModule = null;
    }

    protected HpBar hpBar = null;
    protected float hpBarOffset = -0.5f;

    EntityProperties _properties = new EntityProperties();
    public EntityProperties Properties { get { return _properties;} }
    
    public BuffModule buffModule = new BuffModule();
    public virtual void SetAlpha(float a)
    {

    }
    
    public virtual void HitByOther(EntityProperties other)
    {
        if (Properties.invincible || !GameSetting.isHost)
            return;
        bool critical;
        int damage = EntityProperties.CalcDamage(other, this.Properties, out critical);
        Properties.hp -= damage;
        BattleInfo info = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
        info.AddDamageText(transform.position, damage, critical);
        if(Properties.hp <= 0)
        {   //die
            Destroy(this.gameObject);
        }
    }

    //最大移动速度
    public float _maxHorSpeed = 2.0f;
    public float MaxHorSpeed { get { return _maxHorSpeed * Properties.speedScale; } }

    //移动加速力
    public float _movForce = 3;
    public float MovForce { get { return _movForce * Properties.speedScale; } }

    //空中移动加速力
    public float _movForceInAir = 0.2f;
    public float MovFroceInAir { get { return _movForceInAir * Properties.speedScale; } }

    //跳跃速度
    public float _jumpSpeed = 6f;
    public float JumpSpeed { get { return _jumpSpeed * Properties.jumpScale; } }
}
