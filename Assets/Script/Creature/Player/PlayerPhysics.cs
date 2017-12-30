using UnityEngine;
using System.Collections;
using System;

public partial class Player
{
    //enum SimualteState
    //{
    //    Nothing,
    //    OnGround,
    //    OnLadder,
    //    InAir,
    //}

    protected bool onGround = false;
    protected bool onPlatform = false;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public LayerMask platformLayerMask;


    DateTime jumpTime = DateTime.Now;
    DateTime crossPlatformTime = DateTime.Now;
    bool isCrossPlatform = false;
    protected Vector2 recoilForce = Vector2.zero; //后坐力,update中生成,存在这里,fixedupdate再来处理
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
            if (!syncState.left && !syncState.right && rb.velocity.y <= 0) //有向上的速度,也不会触发立即停止或者摩擦力
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                SetBodyAnimation(BodyAnimation.Idle);
            }
            else
            {  //地面移动处理
                if (syncState.left)
                { //按住左
                    if (_faceRight)  //朝着右边,则按后退处理
                    {
                        if (rb.velocity.x > -MaxBackSpeed)
                            rb.AddForce(new Vector2(-MovBackForce, 0));
                        else
                            rb.velocity = new Vector2(-MaxBackSpeed, rb.velocity.y);
                        SetBodyAnimation(BodyAnimation.Back);
                    }
                    else
                    { //朝着左边,按前进处理
                        if (rb.velocity.x > -MaxForwardSpeed)
                            rb.AddForce(new Vector2(-MovForwardForce, 0));
                        else
                            rb.velocity = new Vector2(-MaxForwardSpeed, rb.velocity.y);
                        SetBodyAnimation(BodyAnimation.Run);
                    }
                }
                if (syncState.right) //按住右
                {
                    if (_faceRight)  //朝着右边,前进
                    {
                        if (rb.velocity.x < MaxForwardSpeed)
                            rb.AddForce(new Vector2(MovForwardForce, 0));
                        else
                            rb.velocity = new Vector2(MaxForwardSpeed, rb.velocity.y);
                        SetBodyAnimation(BodyAnimation.Run);
                    }
                    else
                    {  //朝着左边,后退 
                        if (rb.velocity.x < MaxBackSpeed)
                            rb.AddForce(new Vector2(MovBackForce, 0));
                        else
                            rb.velocity = new Vector2(MaxBackSpeed, rb.velocity.y);
                        SetBodyAnimation(BodyAnimation.Back);
                    }
                }
            }

            if (syncState.down)
            {
                if (inLadderArea)
                {
                    GetOnLadder();
                }
                else
                {
                    SetCrossPlatform(true);
                    crossPlatformTime = DateTime.Now;
                    isCrossPlatform = true;
                }
            }
            else if (syncState.up)
            {
                if (inLadderArea)
                {
                    GetOnLadder();
                }
            }

            if (syncState.jump)
            {
                TimeSpan span = DateTime.Now - jumpTime;
                if (span.TotalMilliseconds > 500)
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                    jumpTime = DateTime.Now;
                    GetOffLadder();
                    SetBodyAnimation(BodyAnimation.Jump);
                }
            }

            //max speed
            if (rb.velocity.x < -MaxForwardSpeed)
                rb.velocity = new Vector2(-MaxForwardSpeed, rb.velocity.y);
            else if (rb.velocity.x > MaxForwardSpeed)
                rb.velocity = new Vector2(MaxForwardSpeed, rb.velocity.y);
        }
        else
        {   //在空中时,无摩擦力,按住左右会有较小的力
            //if (syncState.left && rb.velocity.x > -MaxForwardSpeed)
            //    rb.AddForce(new Vector2(-MovFroceInAir, 0));
            //if (syncState.right && rb.velocity.x < MaxForwardSpeed)
            //    rb.AddForce(new Vector2(MovFroceInAir, 0));

            if (syncState.down)
            {
                if (inLadderArea)
                    GetOnLadder();
            }
            else if (syncState.up)
            {
                if (inLadderArea)
                    GetOnLadder();
            }
            if (rb.velocity.y > 0)
            {   //上升
                SetBodyAnimation(BodyAnimation.Raise);
            }
            else
            {   //下降
                SetBodyAnimation(BodyAnimation.Fall);
            }
        }

        if (isCrossPlatform)
        {
            //Debug.Log("crossing");
            TimeSpan span = DateTime.Now - crossPlatformTime;
            if (span.TotalMilliseconds > 500 && !onLadder)
            {
                SetCrossPlatform(false);
                isCrossPlatform = false;
            }
        }

        //处理后坐力
        if (recoilForce.x != 0 || recoilForce.y != 0)
        {
            rb.AddForce(recoilForce);
            recoilForce = Vector2.zero; //清空
        }
    }

    float climeSpeed = 1.0f;
    float slideDownSpeed = 2.0f;
    DateTime jumpFromLadderTime = DateTime.Now;
    void SimulateOnLadder()
    {
        if (syncState.up)
        {
            rb.velocity = new Vector2(0, climeSpeed);
            SetBodyAnimation(BodyAnimation.Climb);
        }
        else if (syncState.down)
        {
            rb.velocity = new Vector2(0, -slideDownSpeed);
            SetBodyAnimation(BodyAnimation.ClimbDown);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            SetBodyAnimation(BodyAnimation.IdleOnLadder);
        }

        if (syncState.jump)
        {
            TimeSpan span = DateTime.Now - jumpTime;
            if (span.TotalMilliseconds > 500)
            {
                if (syncState.left || syncState.right)
                {
                    rb.velocity = new Vector2(syncState.left  ? -MaxBackSpeed : MaxBackSpeed, JumpSpeed);
                    jumpTime = DateTime.Now;
                    jumpFromLadderTime = DateTime.Now;
                    GetOffLadder();
                    SetBodyAnimation(BodyAnimation.Jump);
                }
            }
        }


        recoilForce = Vector2.zero; //梯子上不考虑后坐力
    }

    void GetOnLadder()
    {
        TimeSpan span = DateTime.Now - jumpFromLadderTime;
        if (span.TotalMilliseconds > 500)
        {
            onLadder = true;
            rb.gravityScale = 0;
            SetCrossPlatform(true);
            rb.velocity = new Vector2(0, 0);
        }

        //Debug.Log("get on ladder");
    }

    void GetOffLadder()
    {
        onLadder = false;
        rb.gravityScale = 1;
        SetCrossPlatform(false);
        //Debug.Log("get off ladder");
    }

    bool onLadder = false;
    bool inLadderArea = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
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


    public void SetInvincible(bool invincible)
    {
        if (invincible)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Player"))
                gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");
            else if (gameObject.layer == LayerMask.NameToLayer("PCP"))
                gameObject.layer = LayerMask.NameToLayer("PICP");
        }
        else
        {
            if (gameObject.layer == LayerMask.NameToLayer("PlayerInvincible"))
                gameObject.layer = LayerMask.NameToLayer("Player");
            else if (gameObject.layer == LayerMask.NameToLayer("PICP"))
                gameObject.layer = LayerMask.NameToLayer("PCP");
        }
        this.invincible = invincible;
    }

    bool GetInvincible()
    {
        return invincible;
    }


    void SetCrossPlatform(bool cross)
    {
        if (cross)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Player"))
                gameObject.layer = LayerMask.NameToLayer("PCP");
            else if (gameObject.layer == LayerMask.NameToLayer("PlayerInvincible"))
                gameObject.layer = LayerMask.NameToLayer("PICP");
        }

        else
        {
            if (gameObject.layer == LayerMask.NameToLayer("PCP"))
                gameObject.layer = LayerMask.NameToLayer("Player");
            else if (gameObject.layer == LayerMask.NameToLayer("PICP"))
                gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");
        }
    }
}
