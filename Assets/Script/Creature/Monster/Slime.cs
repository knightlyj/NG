using UnityEngine;
using System.Collections;
using System;

public class Slime : Creature {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        _movForwardForce = 8.0f;
        _jumpSpeed = 3.0f;
    }

    bool onGround = false;
    bool onPlatform = false;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public LayerMask platformLayerMask;
    // Update is called once per frame
    protected override void Update () {
        base.Update();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 box = new Vector2(Mathf.Abs((col.size.x - 0.06f) * transform.localScale.x), 0.01f);
        onGround = Physics2D.OverlapBox(groundCheck.position, box, 0, groundLayerMask);
        onPlatform = Physics2D.OverlapBox(groundCheck.position, box, 0, platformLayerMask);
    }

    void FixedUpdate()
    {
        if (isLocal)
        {
            Simulate();
        }
    }
    
    void UpdateFace(Transform target)
    {
        Vector2 direction = target.position - this.transform.position;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (direction.x >= 0)
            sr.flipX = true;
        else if (direction.x < 0)
            sr.flipX = false;
    }

    DateTime groundTime = DateTime.Now;
    bool groundedBefore = false;
    void Simulate()
    {
        if (onGround || onPlatform)
        {
            if(groundedBefore == false)
            {
                groundTime = DateTime.Now;
            }

            

            double interval = (DateTime.Now - groundTime).TotalMilliseconds;
            if(interval >= 1000)
            {
                Transform target = FindTarget();
                if(target != null)
                {
                    UpdateFace(target);
                    JumpToTarget(target);
                }
            }
        }

        if(knockForce.x != 0 || knockForce.y != 0)
        {
            rb.AddForce(knockForce);
            knockForce = Vector2.zero;
        }
        groundedBefore = onGround || onPlatform;
    }

    Transform FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 0)
            return null;
        Transform closest = null;
        float closestDist = 0;
        foreach(GameObject p in players)
        {
            if (closest == null)
            {
                closest = p.transform;
                Vector2 dist = p.transform.position - transform.position;
                closestDist = dist.magnitude;
            }
            else
            {
                Vector2 dist = p.transform.position - transform.position;
                float d = dist.magnitude;
                if (d < closestDist)
                {
                    closestDist = d;
                    closest = p.transform;
                }
            }
        }
        return closest;
    }
    
    void JumpToTarget(Transform tar)
    {
        //Debug.Log("jump");
        Vector3 dir = tar.transform.position - transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (dir.x > 0)
            rb.AddForce(new Vector2(MovForwardForce, 0));
        else
            rb.AddForce(new Vector2(-MovForwardForce, 0));

        rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {   //hit player
            Player p = coll.gameObject.GetComponent<Player>();
            //p.HitByOther(this.Properties, transform.position);
        }
    }
}
