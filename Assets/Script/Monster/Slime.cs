using UnityEngine;
using System.Collections;
using System;

public class Slime : BaseMonster
{
    public Transform trTarget = null;

    private Transform groundCheck1;
    private Transform groundCheck2;
    // Use this for initialization
    protected new void Start () {
        base.Start();
        groundCheck1 = transform.Find("GroundCheck1");
        groundCheck2 = transform.Find("GroundCheck2");
        properties.hp = 100;
    }

    bool grounded = true;
    // Update is called once per frame
    protected new void Update () {
        grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Platform"))
            || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Platform"));
        base.Update();
        //Debug.Log(grounded);
	}

    bool needJump = false;
    void FixedUpdate()
    {
        if (needJump)
        {
            needJump = false;
            if (trTarget != null)
                JumpToTarget(trTarget);
        }
    }

    DateTime groundedTime = DateTime.Now;
    DateTime lastJumpTime = DateTime.Now;
    bool lastUpdateGrounded = false;
    protected override void MonsterAI()
    {   //just try to touch target
        if (trTarget == null)
            return;
        if (grounded)
            UpdateFace(trTarget);

        if(lastUpdateGrounded == false && grounded == true)
        {   //上一次update没落地,这次落地了,
            groundedTime = DateTime.Now;
        }
        lastUpdateGrounded = grounded;

        TimeSpan span = DateTime.Now - groundedTime;
        if(span.TotalMilliseconds >= 1000)
        {
            span = DateTime.Now - lastJumpTime;
            if (span.TotalMilliseconds >= 2000)
            {
                lastJumpTime = DateTime.Now;
                needJump = true;
            }
        }
    }
    
    void UpdateFace(Transform tar)
    {
        Vector3 dir = tar.transform.position - transform.position;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (dir.x > 0)
            sr.flipX = true;
        else if (dir.x <= 0)
            sr.flipX = false;
    }

    void JumpToTarget(Transform tar)
    {
        //Debug.Log("jump");
        Vector3 dir = tar.transform.position - transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(dir.x > 0)
            rb.AddForce(new Vector2(80, 200));
        else
            rb.AddForce(new Vector2(-80, 200));
    }

    public override void HitByOther(DamageDate damage, Vector2 pos)
    {
        Vector2 dir = (Vector2)transform.position - pos;
        dir.Normalize();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * damage.knockBack);
        base.HitByOther(damage, pos);
    }

    public override void OnHpEmpty(object sender, EventArgs e)
    {
        base.OnHpEmpty(sender, e);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player") || coll.gameObject.layer == LayerMask.NameToLayer("PlayerCrossPlatform"))
        {   //hit player
            Player p = coll.gameObject.GetComponent<Player>();
            DamageDate damage = new DamageDate();
            damage.knockBack = 100;
            p.HitByOther(damage, transform.position);
        }
    }
}
