using UnityEngine;
using System.Collections;
using System;

//************************************
/// <summary>
/// 物体移动由rigidbody2d完成,这里实现了根据速度调整物体方向,碰撞时的回调,生命期处理.
/// </summary>
public class SlowProjectile : Projectile
{
    public enum ExplodeType
    {
        OnHit, //击中时爆炸
        OnTime, //定时爆炸
    }
    public ExplodeType explodeType = ExplodeType.OnHit;
    
    [SerializeField]
    protected float baseSpeed = 5; //基本速度
    [SerializeField]
    protected float explodeRadius = 0.5f; //爆炸范围

    protected Rigidbody2D rb;
    // Use this for initialization
    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        //最大生命期
        if (lifeTime > 10)
            lifeTime = 10;
    }

    // Update is called once per frame
    protected virtual void Update() {
        CheckLifeTime();
        if (rb.gravityScale != 0)
            DirectionWithVelocity(); //调整方向
    }

    //射击时的初始参数
    public override void Shoot(Entity owner, Vector2 position, Vector2 direction, float speedScale, int hitLayerMask)
    {
        base.Shoot(owner, position, direction, speedScale, hitLayerMask);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * baseSpeed * speedScale; //设置初始速度

        DirectionWithVelocity();
    }

    
    //碰撞时的回调
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (explodeType != ExplodeType.OnHit)
            return;
        GameObject go = other.gameObject;
        if(((1<<go.layer) & hitLayerMask) != 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, this.explodeRadius, hitLayerMask);
            if(hits != null && hits.Length > 0)
            {
                foreach(Collider2D col in hits)
                {
                    if(col.gameObject.tag == "Player"
                        || col.gameObject.layer == LayerMask.NameToLayer("Monster")
                        || col.gameObject.layer == LayerMask.NameToLayer("NPC"))
                    {
                        Entity e = col.gameObject.GetComponent<Entity>();
                        e.HitByOther(this, transform.position);
                    }
                }
            }
            Explode(transform.position);
        }
    }

    //根据速度调整朝向
    void DirectionWithVelocity()
    {
        if (hit)
            return;
        //direction
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        Vector2 dir = rb.velocity;
        if (Mathf.Abs(dir.magnitude) <= 0.001f) //范数为0时不要计算,会产生除0错误
            return;
        dir.Normalize();
        float angle = Mathf.Acos(dir.x) / Mathf.PI * 180;
        if (dir.y < 0)
            angle = -angle;
        angle -= 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    //******************hit event***********************
    //public class OnHitEventArg : EventArgs
    //{

    //}

    //public event EventHandler<OnHitEventArg> OnHitEventHandler;

    //void RaiseHitEvent(OnHitEventArg e)
    //{
    //    EventHandler<OnHitEventArg> temp = this.OnHitEventHandler;
    //    if(temp != null)
    //    {
    //        temp(this, e);
    //    }
    //}

    //***********************************
    bool hit = false;
    
}
