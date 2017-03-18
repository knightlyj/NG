using UnityEngine;
using System.Collections;
using System;

public class Slime : Entity {
    
	// Use this for initialization
	void Start () {
        base.Start();
    }

    Vector2 groundCheck1 = new Vector2(-0.5f, -0.52f);
    Vector2 groundCheck2 = new Vector2(0.5f, -0.52f);
    bool grounded = false;
    // Update is called once per frame
    void Update () {
        base.Update();
        grounded = Physics2D.Linecast(transform.position, (Vector2)transform.position + groundCheck1, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, (Vector2)transform.position + groundCheck2, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, (Vector2)transform.position + groundCheck1, 1 << LayerMask.NameToLayer("Platform"))
            || Physics2D.Linecast(transform.position, (Vector2)transform.position + groundCheck2, 1 << LayerMask.NameToLayer("Platform"));
    }

    void FixedUpdate()
    {
        if (isLocal)
        {
            Simulate();
        }
    }

    DateTime groundTime = DateTime.Now;
    bool groundedBefore = false;
    void Simulate()
    {
        if (grounded)
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
                    JumpToTarget(target);
                }
            }
        }

        groundedBefore = grounded;
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
            rb.AddForce(new Vector2(80, 200));
        else
            rb.AddForce(new Vector2(-80, 200));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.gameObject.layer == LayerMask.NameToLayer("Player") || coll.gameObject.layer == LayerMask.NameToLayer("PlayerCrossPlatform"))
        //{   //hit player
        //    Player p = coll.gameObject.GetComponent<Player>();
        //    DamageDate damage = new DamageDate();
        //    damage.knockBack = 100;
        //    p.HitByOther(damage, transform.position);
        //}
    }
}
