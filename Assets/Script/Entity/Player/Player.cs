﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct PlayerSta
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool jump;
    
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 targetPos;
}

public partial class Player : Entity
{
    public PlayerSta state;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        SetUpBodySR();
        InitAnimation();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        state.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateAnimation();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    this.buffModule.AddBuff(this, BuffId.Invincible);
        //}
    }


    void FixedUpdate()
    {

        if (isLocal)
        {
            state.up = Input.GetKey(GameSetting.up);
            state.down = Input.GetKey(GameSetting.down);
            state.left = Input.GetKey(GameSetting.left);
            state.right = Input.GetKey(GameSetting.right);
            state.jump = Input.GetKey(GameSetting.jump);

            Simualte();
        }
    }

    List<SpriteRenderer> bodySRs = new List<SpriteRenderer>();
    void AddBodyPart(Transform t)
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            bodySRs.Add(sr);
        }
    }
    void SetUpBodySR()
    {
        Helper.TravesalGameObj(this.transform, this.AddBodyPart);
        //Debug.Log("count " + bodySRs.Count);
        //foreach(SpriteRenderer sr in bodySRs)
        //{
        //    Debug.Log(sr.gameObject.name);
        //}
    }

    public override void SetAlpha(float a)
    {
        if (bodySRs == null)
            return;
        foreach (SpriteRenderer sr in bodySRs)
        {
            Color newColor = sr.color;
            newColor.a = a;
            sr.color = newColor;

        }
    }

    

}


