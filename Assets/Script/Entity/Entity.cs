using UnityEngine;
using System.Collections;

public class PlayerProperties
{
    public float hp;
    public int maxHp;
    public int minAttack;
    public int maxAttack;
    public int defense;
    public float speedScale;
    public float jumpScale;
    public float atkInterval;
    public float atkSpeed;
    public float criticalChance;
    public float criticalRate;
    public float rcr; //资源消耗减少

    public bool invincible = false;

    public PlayerProperties()
    {
        hp = 100;
        maxHp = 100;
        minAttack = 10;
        maxAttack = 20;
        defense = 0;
        speedScale = 1.0f;
        jumpScale = 1.0f;
        atkSpeed = 1.0f;
        criticalChance = 0.05f;
        criticalRate = 2.0f;
        rcr = 0;
    }

    public void Reset() //全部设置为初值
    {
        hp = 100;
        maxHp = 100;
        minAttack = 10;
        maxAttack = 20;
        defense = 0;
        speedScale = 1.0f;
        jumpScale = 1.0f;
        atkSpeed = 1.0f;
        criticalChance = 0.05f;
        criticalRate = 2.0f;
        rcr = 0;
    }

    public static int CalcDamage(PlayerProperties from, PlayerProperties to, out bool critical)
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

        if(EntityDestroyEvent != null)
        {
            EntityDestroyEvent();
        }

        //
        buffModule.ClearBuff();
        buffModule = null;
        
    }

    protected HpBar hpBar = null;
    [SerializeField]
    protected float hpBarOffset = -0.5f;

    PlayerProperties _properties = new PlayerProperties();
    public PlayerProperties Properties { get { return _properties;} }
    
    public BuffModule buffModule = new BuffModule();
    public virtual void SetTransparent(float a)
    {

    }
    
    public virtual void HitByOther(PlayerProperties other)
    {
        if (Properties.invincible || !GameSetting.isHost) //无敌状态或者不是主机,则不产生击中效果
            return;
        //伤害计算
        bool critical;
        int damage = PlayerProperties.CalcDamage(other, this.Properties, out critical);
        Properties.hp -= damage;
        BattleInfo info = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
        info.AddDamageText(transform.position, damage, critical);
        //空血时销毁
        if (Properties.hp <= 0)
        {   //die
            Destroy(this.gameObject);
        }
    }

    public delegate void OnEntityDestory();
    public event OnEntityDestory EntityDestroyEvent;

    //最大前进速度
    public float _maxForwardSpeed = 2.0f;
    public float MaxForwardSpeed { get { return _maxForwardSpeed * Properties.speedScale; } }

    //最大后退速度
    public float _maxBackSpeed = 1.0f;
    public float MaxBackSpeed { get { return _maxBackSpeed * Properties.speedScale; } }

    //前进加速力
    public float _movForwardForce = 3;
    public float MovForwardForce { get { return _movForwardForce * Properties.speedScale; } }

    //后退加速力
    public float _movBackForce = 1;
    public float MovBackForce { get { return _movBackForce * Properties.speedScale; } }

    //空中移动加速力
    public float _movForceInAir = 0.1f;
    public float MovFroceInAir { get { return _movForceInAir * Properties.speedScale; } }

    //跳跃速度
    public float _jumpSpeed = 6f;
    public float JumpSpeed { get { return _jumpSpeed * Properties.jumpScale; } }
}
