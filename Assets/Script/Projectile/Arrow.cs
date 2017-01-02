using UnityEngine;
using System.Collections;
using System;

//************************************
/// <summary>
/// 物体移动由rigidbody2d完成,这里实现了根据速度调整物体方向,碰撞时的回调,生命期处理.
/// 扩展时,可以修改whenhit发生爆炸或其他效果
/// </summary>
public class Arrow : MonoBehaviour, Projectile
{
    public Transform stoneSplashPrefab;

    DamageDate damage;
    GameObject owner = null; //射击者
    public float lifeTime = 10 * 1000; //生命期
    public float baseSpeed = 5; //基本速度
    public float speedScale = 1; //速度scale

    DateTime spawnTime;
    // Use this for initialization
    protected void Start() {
        spawnTime = DateTime.Now;

    }

    // Update is called once per frame
    protected void Update() {
        //life time
        TimeSpan span = DateTime.Now - spawnTime;
        if (span.TotalMilliseconds > this.lifeTime)
        {
            Destroy(this.gameObject);
        }

        DirectionWithVelocity(); //调整方向
    }

    protected void FixedUpdate()
    {

    }

    //射击时的初始参数
    public void Shoot(GameObject owner, Vector2 position, Vector2 target, float speedScale, DamageDate damage)
    {
        //保存参数
        this.owner = owner;
        this.speedScale = speedScale;
        this.damage = damage;
        //position direction speed
        Vector2 direction = target - position;
        direction.Normalize();
        transform.position = position;
        float angle = Mathf.Acos(direction.x) / Mathf.PI * 180;
        if (direction.y < 0)
            angle = -angle;
        angle -= 90;
        transform.Rotate(new Vector3(0, 0, 1), angle); //设置初始角度

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * baseSpeed * speedScale; //设置初始速度

    }

    //碰撞时的回调
    protected void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player") || go.layer == LayerMask.NameToLayer("PlayerCrossPlatform"))
        {   //hit player
            OnHitEventArg e = new OnHitEventArg();
            RaiseHitEvent(e);
            if (go != owner)
            {
                Explode();
                Player p = other.gameObject.GetComponent<Player>();
                p.HitByOther(this.damage, transform.position);
            }
        }
        else if( go.layer == LayerMask.NameToLayer("Monster"))
        {
            OnHitEventArg e = new OnHitEventArg();
            RaiseHitEvent(e);
            if (go != owner)
            {
                Explode();
                BaseMonster m = other.gameObject.GetComponent<BaseMonster>();
                m.HitByOther(this.damage, transform.position);
            }
        }
        else if(go.layer == LayerMask.NameToLayer("Ground"))
        {
            OnHitEventArg e = new OnHitEventArg();
            RaiseHitEvent(e);
            Explode();
        }
    }
    //根据速度调整朝向
    protected void DirectionWithVelocity()
    {
        if (hit)
            return;
        //direction
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        Vector2 dir = rb.velocity;
        dir.Normalize();
        float angle = Mathf.Acos(dir.x) / Mathf.PI * 180;
        if (dir.y < 0)
            angle = -angle;
        angle -= 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    //******************hit event***********************
    public class OnHitEventArg : EventArgs
    {

    }

    public event EventHandler<OnHitEventArg> OnHitEventHandler;

    void RaiseHitEvent(OnHitEventArg e)
    {
        EventHandler<OnHitEventArg> temp = this.OnHitEventHandler;
        if(temp != null)
        {
            temp(this, e);
        }
    }

    //***********************************
    bool hit = false;
    virtual protected void Explode()
    {
        //Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;
        //rb.velocity = Vector2.zero;
        //hit = true;

        Transform trStoneSplash = Instantiate(stoneSplashPrefab, transform.position, Quaternion.identity) as Transform;
        ParticleSystem ps = trStoneSplash.GetComponent<ParticleSystem>();
        Destroy(trStoneSplash.gameObject, ps.startLifetime);
        Destroy(this.gameObject);
    }
}
