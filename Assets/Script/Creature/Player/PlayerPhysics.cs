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


    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public LayerMask platformLayerMask;
    Collider2D hitGround = null;  //保存踩到的地面,地面可能有移动
    protected bool onGround { get { return hitGround != null; } }
    protected bool onPlatform = false;

    DateTime jumpTime = DateTime.Now;
    DateTime crossPlatformTime = DateTime.Now;
    bool isCrossPlatform = false;
    protected Vector2 recoilForce = Vector2.zero; //后坐力,update中生成,存在这里,fixedupdate再来处理
    void Simualte()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        Vector2 box = new Vector2(Mathf.Abs((col.radius * 2 - 0.1f) * transform.localScale.x), 0.01f);
        hitGround = Physics2D.OverlapBox(groundCheck.position, box, 0, groundLayerMask);
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
                SetBodyAnimation(BodyAnimation.Idle);
            }
            else
            {  //地面移动处理
                float groundSpeed = 0; //地面移动速度
                if (onGround)
                {
                    Rigidbody2D rbGnd = hitGround.GetComponent<Rigidbody2D>();
                    if(rbGnd != null)  //部分ground没有加刚体组件
                        groundSpeed = rbGnd.velocity.x;
                }
                if (syncState.left)
                { //按住左
                    if (_faceRight)  //朝着右边,则按后退处理  
                    {  //处理水平移动速度时,要考虑地面可能移动,比如电梯上
                        if (rb.velocity.x > -MaxBackSpeed + groundSpeed)
                            rb.AddForce(new Vector2(-MovBackForce, 0));
                        SetBodyAnimation(BodyAnimation.Back);
                    }
                    else
                    { //朝着左边,按前进处理
                        if (rb.velocity.x > -MaxForwardSpeed + groundSpeed)
                            rb.AddForce(new Vector2(-MovForwardForce, 0));
                        SetBodyAnimation(BodyAnimation.Run);
                    }
                }
                if (syncState.right) //按住右
                {
                    if (_faceRight)  //朝着右边,前进
                    {
                        if (rb.velocity.x < MaxForwardSpeed + groundSpeed)
                            rb.AddForce(new Vector2(MovForwardForce, 0));
                        SetBodyAnimation(BodyAnimation.Run);
                    }
                    else
                    {  //朝着左边,后退 
                        if (rb.velocity.x < MaxBackSpeed + groundSpeed)
                            rb.AddForce(new Vector2(MovBackForce, 0));
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
        }
        else
        {   //在空中时,无摩擦力,按住左右会有较小的力
            if (syncState.left && rb.velocity.x > -MaxForwardSpeed)
                rb.AddForce(new Vector2(-MovFroceInAir, 0));
            if (syncState.right && rb.velocity.x < MaxForwardSpeed)
                rb.AddForce(new Vector2(MovFroceInAir, 0));

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
                    rb.velocity = new Vector2(syncState.left ? -MaxBackSpeed : MaxBackSpeed, JumpSpeed);
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
            rb.isKinematic = true;
            SetCrossPlatform(true);
            rb.velocity = new Vector2(0, 0);
        }

        //Debug.Log("get on ladder");
    }

    void GetOffLadder()
    {
        onLadder = false;
        rb.isKinematic = false;
        SetCrossPlatform(false);
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
        this.invincible = invincible;
    }

    bool GetInvincible()
    {
        return invincible;
    }


    void SetCrossPlatform(bool cross)
    {
        if (cross)
            gameObject.layer = LayerMask.NameToLayer(TextResources.ccpLayer);

        else
            gameObject.layer = LayerMask.NameToLayer(TextResources.creatureLayer);
    }
}
