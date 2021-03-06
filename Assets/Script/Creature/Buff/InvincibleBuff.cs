﻿using UnityEngine;
using System.Collections;
using System;

public class InvincibleBuff : BaseBuff
{
    bool transparent = false;
    const float switchInterval = 0.06f; //这里时间用的是秒
    float switchTime = switchInterval; //透明切换显示时间
    const float invincibleDuration = 1;
    
    // Use this for initialization
    public override void Start (Creature target) {
        base.Start(target);
        this.duration = invincibleDuration;
        //Debug.Log("buff start");
        Player p = this.buffTarget as Player;
        p.SetInvincible(true);
    }

    // Update is called once per frame
    public override bool Update(float deltaTime)
    {
        switchTime -= deltaTime;
        if(switchTime <= 0)
        {
            //Debug.Log("buff switch");
            if (transparent)
            {
                transparent = false;
                buffTarget.SetTransparent(0.1f);
            }
            else
            {
                transparent = true;
                buffTarget.SetTransparent(0.8f);
            }
            switchTime = switchInterval;
        }
        this.duration -= deltaTime;
        if (this.duration <= 0)
            return true;
        return false;
    }

    public override void Stop()
    {
        Player p = this.buffTarget as Player;
        p.SetTransparent(1f);

        p.SetInvincible(false);
    }
}
