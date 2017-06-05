using UnityEngine;
using System.Collections;

public partial class Player{

    //***********************************************
    Animator bodyAnimator = null;
    //Animator aimAnimator = null;
    Transform trHead;
    Transform trMainHand;
    Transform trOffHand;
    void InitAnimation()
    {
        bodyAnimator = transform.FindChild("Body").GetComponent<Animator>();
        //aimAnimator = bodyAnimator.transform.FindChild("Aim").GetComponent<Animator>();

        trHead = transform.FindChild("Body").FindChild("Head");
        trMainHand = transform.FindChild("Body").FindChild("MainUpperArm");
        trOffHand = transform.FindChild("Body").FindChild("OffUpperArm");
    }

    void UpdateAnimation()
    {
        UpdateFace();
        UpdateAim();
        UpateBody();
    }
    
    void UpdateAim()
    {
        Vector2 direction = state.targetPos - (Vector2)this.transform.position;
        direction.Normalize();
        float angle = Mathf.Abs(Mathf.Acos(Mathf.Abs(direction.x)) / Mathf.PI * 180);
        if (direction.y < 0)
            angle = -angle;

        if (!faceRight) //朝向左边时会导致角度反转,没详细了解原因
            angle = -angle;
        
        trHead.rotation = Quaternion.Euler(0, 0, angle/2);
        trMainHand.rotation = Quaternion.Euler(0, 0, angle);
        trOffHand.rotation = Quaternion.Euler(0, 0, angle);
        //待补充
        //Debug.Log("angle " + angle);
    }

    bool faceRight = true;
    void UpdateFace()
    {
        Vector2 direction = state.targetPos - (Vector2)this.transform.position;
        bool change = false;
        if (direction.x >= 0 && !faceRight)
            change = true;
        else if (direction.x < 0 && faceRight)
            change = true;
        if (change)
        {
            faceRight = !faceRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    enum BodyAnimationState
    {
        Nothing,
        Stand,
        Run,
        Back,
        InAir,
    }
    BodyAnimationState bodyAnimationState = BodyAnimationState.Nothing;
    void UpateBody()
    {
        BodyAnimationState newState = BodyAnimationState.Nothing;
        if (true)
        {
            if ((rb.velocity.x > 0 && faceRight) || (rb.velocity.x < 0 && !faceRight))
            {   //朝向与移动方向一致,跑
                newState = BodyAnimationState.Run;
            }
            else if ((rb.velocity.x < 0 && faceRight) || (rb.velocity.x > 0 && !faceRight))
            {   //朝向与移动方向不同,后退
                newState = BodyAnimationState.Back;
            }
            else
            {   //速度为0,站立
                newState = BodyAnimationState.Stand;
            }
        }
        //else
        //{
        //    newState = BodyAnimationState.InAir;
        //}
        if (newState != bodyAnimationState)
        {   //动画不同,切换 待补充
            bodyAnimationState = newState;
            bodyAnimator.SetInteger("BodyState", (int)bodyAnimationState);
            //Debug.Log("animation state " + bodyAnimationState.ToString());
        }
    }

}
