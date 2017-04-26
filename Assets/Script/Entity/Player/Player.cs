using UnityEngine;
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
    public Player()
    {
        //hpbar偏移,base.Start会使用,需要先设置
        this.hpBarOffset = -0.35f;
    }

    public PlayerSta state;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //物理效果参数
        _maxHorSpeed = 2.0f;
        _movForce = 3;
        _movForceInAir = 0.2f;
        _jumpSpeed = 6f;

        //
        SetUpBodySR();
        InitAnimation();

        atkLayerMask = 1 << LayerMask.NameToLayer("Monster") |
                       1 << LayerMask.NameToLayer("Ground");

        PackageInit();
    }

    int atkLayerMask;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        state.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateAnimation();
        CheckShoot();
        
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
        if (sr != null)
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

    public override void HitByOther(EntityProperties other)
    {
        if (Properties.invincible || !GameSetting.isHost) //无敌状态或者不是主机,则不产生击中效果
            return;
        //伤害计算
        bool critical;
        int damage = EntityProperties.CalcDamage(other, this.Properties, out critical);
        Properties.hp -= damage;
        BattleInfo info = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
        info.AddDamageText(transform.position, damage, critical);

        //击中后添加无敌buff
        if (this.buffModule != null)
        {
            buffModule.AddBuff(this, BuffId.Invincible);
        }

        //空血处理
        if (Properties.hp <= 0)
        {   //隐藏,不销毁
            this.gameObject.SetActive(false);
        }
    }

    
}
