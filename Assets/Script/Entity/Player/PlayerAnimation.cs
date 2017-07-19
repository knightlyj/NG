using UnityEngine;
using System.Collections;
using DragonBones;

public partial class Player
{
    //动画名称
    readonly string runAniName = "aimRun";
    readonly string walkAniName = "aimWalk";
    readonly string aimUpAniName = "aimUp";
    readonly string aimDownAniName = "aimDown";
    readonly string idleAniName = "idle";
    readonly string jumpAniName = "jump";
    readonly string fallAniName = "fall";
    readonly string raiseAniName = "raise";
    readonly string backAniName = "back";
    //动画分组
    readonly string aimAniGroup = "AimGroup";
    readonly string bodyAniGroup = "BodyGroup";
    //***********************************************
    UnityArmatureComponent armatureComponent = null;
    UnityEngine.Transform aimBone = null;
    void InitAnimation()
    {
        armatureComponent = transform.FindChild("armature").GetComponent<UnityArmatureComponent>();
        armatureComponent.animation.Reset();
        aimBone = transform.FindChild("armature").FindChild("Bones").FindChild("for_aim");
    }

    void UpdateAnimation()
    {
        UpdateFace();
        UpdateAim();
        UpateBody();
    }

    void UpdateAim()
    {
        Vector2 direction = syncState.targetPos - (Vector2)aimBone.position;
        direction.Normalize();
        float radian = Mathf.Abs(Mathf.Acos(Mathf.Abs(direction.x)));
        float rate = radian / (Mathf.PI / 2);
        //Debug.Log(rate);
        DragonBones.AnimationState _aimState = null;
        //瞄准方向
        if (direction.y > 0)
        {
            _aimState = armatureComponent.animation.FadeIn(aimUpAniName, 0.01f, 1, 0, aimAniGroup, AnimationFadeOutMode.SameGroup);
        }
        else
        {
            _aimState = armatureComponent.animation.FadeIn(aimDownAniName, 0.01f, 1, 0, aimAniGroup, AnimationFadeOutMode.SameGroup);
        }
        _aimState.weight = rate;
    }

    bool faceRight = true;
    void UpdateFace()
    {
        Vector2 direction = syncState.targetPos - (Vector2)this.transform.position;
        bool change = false;
        if (direction.x >= 0 && !faceRight)
            change = true;
        else if (direction.x < 0 && faceRight)
            change = true;
        if (change)
        {
            faceRight = !faceRight;
            //改变朝向
            armatureComponent.armature.flipX = !armatureComponent.armature.flipX;
        }
    }

    enum BodyAnimation
    {
        Nothing,
        Idle,
        Run,
        Walk,
        Back,

        Jump,
        Raise,
        Fall,
    }

    void UpateBody()
    {
        if (true)
        {
            if ((rb.velocity.x > 0 && faceRight) || (rb.velocity.x < 0 && !faceRight))
            {   //朝向与移动方向一致,跑
            }
            else if ((rb.velocity.x < 0 && faceRight) || (rb.velocity.x > 0 && !faceRight))
            {   //朝向与移动方向不同,后退
            }
        }
        
    }


    BodyAnimation lastAnimation = BodyAnimation.Nothing;
    void SetBodyAnimation(BodyAnimation animation)
    {
        if (animation != lastAnimation)
        {   //动画不同,切换 
            lastAnimation = animation;
            Debug.Log("animation state " + animation.ToString());
            switch (animation)
            {
                case BodyAnimation.Run:
                    armatureComponent.animation.FadeIn(runAniName, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Walk:
                    armatureComponent.animation.FadeIn(walkAniName, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Idle:
                    armatureComponent.animation.FadeIn(idleAniName, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Jump:
                    armatureComponent.animation.FadeIn(jumpAniName, 0, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Fall:
                    armatureComponent.animation.FadeIn(fallAniName, 0.2f, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Raise:
                    armatureComponent.animation.FadeIn(raiseAniName, 0.2f, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Back:
                    armatureComponent.animation.FadeIn(backAniName, 0.2f, -1, 0, bodyAniGroup);
                    break;
            }
        }
    }

    public override void SetTransparent(float a)
    {

    }
}
