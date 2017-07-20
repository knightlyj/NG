﻿using UnityEngine;
using System.Collections;
using DragonBones;

public partial class Player
{
    //动画名称
    readonly string runAni = "aimRun";
    readonly string walkAni = "aimWalk";
    readonly string aimUpAni = "aimUp";
    readonly string aimDownAni = "aimDown";
    readonly string idleAni = "idle";
    readonly string jumpAni = "jump";
    readonly string fallAni = "fall";
    readonly string raiseAni = "raise";
    readonly string backAni = "back";
    readonly string idleOnLadderAni = "idleOnLadder";
    readonly string climbAni = "climb";
    readonly string climbDownAni = "climbDown";
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
            _aimState = armatureComponent.animation.FadeIn(aimUpAni, 0.01f, 1, 0, aimAniGroup, AnimationFadeOutMode.SameGroup);
        }
        else
        {
            _aimState = armatureComponent.animation.FadeIn(aimDownAni, 0.01f, 1, 0, aimAniGroup, AnimationFadeOutMode.SameGroup);
        }
        _aimState.weight = rate;
    }

    bool faceRight = true;
    void UpdateFace()
    {
        Vector2 direction = syncState.targetPos - (Vector2)aimBone.position;
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

        IdleOnLadder,
        Climb,
        ClimbDown,
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
                    armatureComponent.animation.FadeIn(runAni, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Walk:
                    armatureComponent.animation.FadeIn(walkAni, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Idle:
                    armatureComponent.animation.FadeIn(idleAni, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Jump:
                    armatureComponent.animation.FadeIn(jumpAni, 0, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Fall:
                    armatureComponent.animation.FadeIn(fallAni, 0.2f, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Raise:
                    armatureComponent.animation.FadeIn(raiseAni, 0.2f, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Back:
                    armatureComponent.animation.FadeIn(backAni, 0.2f, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.Climb:
                    armatureComponent.animation.FadeIn(climbAni, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.ClimbDown:
                    armatureComponent.animation.FadeIn(climbDownAni, -1, -1, 0, bodyAniGroup);
                    break;
                case BodyAnimation.IdleOnLadder:
                    armatureComponent.animation.FadeIn(idleOnLadderAni, -1, -1, 0, bodyAniGroup);
                    break;
            }
        }
    }

    public override void SetTransparent(float a)
    {

    }
}
