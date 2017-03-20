using UnityEngine;
using System.Collections;
using System;

public partial class Player {
    //enum SimualteState
    //{
    //    Nothing,
    //    OnGround,
    //    OnLadder,
    //    InAir,
    //}
    
    bool onGround = false;
    bool onPlatform = false;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public LayerMask platformLayerMask;
    

    DateTime jumpTime = DateTime.Now;
    DateTime crossPlatformTime = DateTime.Now;
    bool isCrossPlatform = false;
    void Simualte()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        Vector2 box = new Vector2(Mathf.Abs((col.radius * 2 - 0.1f) * transform.localScale.x), 0.01f);
        onGround = Physics2D.OverlapBox(groundCheck.position, box, 0, groundLayerMask);
        onPlatform = Physics2D.OverlapBox(groundCheck.position, box, 0, platformLayerMask);
        if (onLadder)
        {
            SimulateOnLadder();
        }
        else
        {
            SimulateFree();
        }
    }

    void SimulateFree()
    {
        if (onGround || onPlatform)
        { //在地面时,不按左右则立即停下来,或者由摩擦力停下,并且按住左右会有较大的力,摩擦力由物理引擎实现
            if (!state.left && !state.right && rb.velocity.y <= 0) //有向上的速度,也不会触发立即停止或者摩擦力
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                if (state.left && rb.velocity.x > -MaxHorSpeed)
                    rb.AddForce(new Vector2(-MovForce, 0));
                if (state.right && rb.velocity.x < MaxHorSpeed)
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(MovForce, 0));
            }

            if (state.down)
            {
                if (inLadderArea)
                {
                    GetOnLadder();
                }
                else
                {
                    gameObject.layer = LayerMask.NameToLayer("ECP");
                    crossPlatformTime = DateTime.Now;
                    isCrossPlatform = true;
                }
            }
            else if (state.up)
            {
                if (inLadderArea)
                {
                    GetOnLadder();
                }
            }

            if (state.jump)
            {
                TimeSpan span = DateTime.Now - jumpTime;
                if (span.TotalMilliseconds > 500)
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                    jumpTime = DateTime.Now;
                    GetOffLadder();
                }
            }
        }
        else
        {   //在空中时,无摩擦力,按住左右会有较小的力
            if (state.left && rb.velocity.x > -MaxHorSpeed)
                rb.AddForce(new Vector2(-MovFroceInAir, 0));
            if (state.right && rb.velocity.x < MaxHorSpeed)
                rb.AddForce(new Vector2(MovFroceInAir, 0));

            if (state.down)
            {
                if (inLadderArea)
                    GetOnLadder();
            }
            else if (state.up)
            {
                if (inLadderArea)
                    GetOnLadder();
            }
        }
        //max speed
        if (rb.velocity.x < -MaxHorSpeed)
            rb.velocity = new Vector2(-MaxHorSpeed, rb.velocity.y);
        else if (rb.velocity.x > MaxHorSpeed)
            rb.velocity = new Vector2(MaxHorSpeed, rb.velocity.y);

        if (isCrossPlatform)
        {
            //Debug.Log("crossing");
            TimeSpan span = DateTime.Now - crossPlatformTime;
            if (span.TotalMilliseconds > 500 && !onLadder)
            {
                gameObject.layer = LayerMask.NameToLayer("Entity");
                isCrossPlatform = false;
            }
        }
        
        //else
        //{
        //    RaycastHit2D stuckOnPlatform = Physics2D.Raycast(platformCheck1.position, Vector2.down, platformCheck1.position.y - platformCheck2.position.y, 1 << LayerMask.NameToLayer("Platform"));
        //    if(stuckOnPlatform.collider != null)
        //    {
        //        gameObject.layer = LayerMask.NameToLayer("ECP");
        //        crossPlatformTime = DateTime.Now;
        //        isCrossPlatform = true;
        //        Debug.Log("stuck");
        //    }
        //    else
        //    {
        //        Debug.Log("ok");
        //    }
        //}
    }

    float climeSpeed = 1.0f;
    float slideDownSpeed = 2.0f;
    DateTime jumpFromLadderTime = DateTime.Now;
    void SimulateOnLadder()
    {
        if (state.up)
        {
            rb.velocity = new Vector2(rb.velocity.x, climeSpeed);
        }
        else if (state.down)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideDownSpeed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (state.left)
        {
            rb.velocity = new Vector2(-climeSpeed, rb.velocity.y);
        }
        else if (state.right)
        {
            rb.velocity = new Vector2(climeSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (state.jump)
        {
            TimeSpan span = DateTime.Now - jumpTime;
            if (span.TotalMilliseconds > 500)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                jumpTime = DateTime.Now;
                jumpFromLadderTime = DateTime.Now;
                GetOffLadder();
            }
        }
    }

    void GetOnLadder()
    {
        TimeSpan span = DateTime.Now - jumpFromLadderTime;
        if (span.TotalMilliseconds > 500)
        {
            onLadder = true;
            rb.gravityScale = 0;
            gameObject.layer = LayerMask.NameToLayer("ECP");
        }
        
        //Debug.Log("get on ladder");
    }
    
    void GetOffLadder()
    {
        onLadder = false;
        rb.gravityScale = 1;
        gameObject.layer = LayerMask.NameToLayer("Entity");
        //Debug.Log("get off ladder");
    }

    bool onLadder = false;
    bool inLadderArea = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            inLadderArea = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            inLadderArea = true;
        }
    }

        void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            inLadderArea = false;
            GetOffLadder();
        }
    }
}
