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

public partial class Player : Creature
{
    //*************玩家特有属性*****************
    [HideInInspector]
    public float atkInterval = 0.5f;
    [HideInInspector]
    public float atkSpeed = 1.0f;
    [HideInInspector]
    public float rcr = 0; //资源消耗减少

    override public void ResetProperties()
    {
        base.ResetProperties();
        atkInterval = 0.5f;
        atkSpeed = 1.0f;
        rcr = 0; //资源消耗减少
    }

    protected string _playerName = "local player";
    public string playerName { get { return this._playerName; } }
    public Player()
    {
        //hpbar偏移,base.Start会使用,需要先设置
        //this.hpBarOffset = -0.35f;
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

        EventManager.RaiseEvent(EventId.LocalPlayerCreate, this);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RaiseEvent(EventId.LocalPlayerDestroy, this);
    }

    protected int atkLayerMask;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        syncState.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateAnimation();
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

    public override void HitByOther(Projectile proj, Vector2 pos)
    {
        base.HitByOther(proj, pos);

        //击中后添加无敌buff
        if (this.buffModule != null)
        {
            buffModule.AddBuff(this, BuffId.Invincible);
        }
    }

    protected override void OnHpEmpty()
    {
        gameObject.SetActive(false);
    }

    public void Shoot()
    {
        Vector2 direction = syncState.targetPos - (Vector2)shootPoint.position;
        direction.Normalize();
        LevelManager manager = Helper.GetLevelManager();
        Projectile proj = manager.GenProj(LevelManager.ProjectileType.BaseBullet);
        //设置抛射物的属性
        proj.damage = (this.minAttack + this.maxAttack) / 2;
        proj.knockBack = this.knockBack;

        proj.Shoot(this, shootPoint.position, direction, 1.0f, this.atkLayerMask);
        manager.ShowFlame(shootPoint.position, direction);

        //设置后坐力,在fixedupdate中再施加力
        Vector2 recoilDir = direction * -1;
        recoilForce = recoilDir * recoil;
    }

}
