using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct PlayerSyncState
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
    protected string _playerName = "local player";
    public string playerName { get { return this._playerName; } }
    public Player()
    {
        //hpbar偏移,base.Start会使用,需要先设置
        this.hpBarOffset = -0.35f;
    }

    public PlayerSyncState syncState; //同步所需状态
    bool _alive = true;
    public bool alive { get { return this._alive; } }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //物理效果参数
        _maxForwardSpeed = 2.0f;
        _movForwardForce = 3;
        _movForceInAir = 0.2f;
        _jumpSpeed = 6f;
        
        InitAnimation();

        atkLayerMask = 1 << LayerMask.NameToLayer("Monster") |
                       1 << LayerMask.NameToLayer("Ground");

        ItemInit();
        EventManager.RaiseEvent(EventId.LocalPlayerCreate, this);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RaiseEvent(EventId.LocalPlayerDestroy, this);
    }

    int atkLayerMask;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        syncState.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateAnimation();
        CheckShoot();
        
    }


    protected void FixedUpdate()
    {
        if (isLocal)
        {
            syncState.up = Input.GetKey(GameSetting.up);
            syncState.down = Input.GetKey(GameSetting.down);
            syncState.left = Input.GetKey(GameSetting.left);
            syncState.right = Input.GetKey(GameSetting.right);
            syncState.jump = Input.GetKey(GameSetting.jump);

            Simualte();
        }
    }

    public override void HitByOther(PlayerProperties other)
    {
        if (Properties.invincible || !GameSetting.isHost) //无敌状态或者不是主机,则不产生击中效果
            return;
        //伤害计算
        bool critical;
        int damage = PlayerProperties.CalcDamage(other, this.Properties, out critical);
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
