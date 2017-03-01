using UnityEngine;
using System.Collections;
using System;

public class MyCircleCollier : MyCollider
{
    public MyColliderType Type { get { return MyColliderType.Circle; } }

    MyGameObject _owner = null;
    public MyGameObject Owner { get { return this._owner; } }

    //基于transform的局部坐标
    readonly FpVector2 _localCenter;
    public FpVector2 LocalCenter { get { return this._localCenter; } } //使用transform局部坐标

    FpNumber radius;
    public FpNumber Radius { get { return this.radius; } }

    //center的世界坐标
    FpVector2 _worldCenter;
    public FpVector2 Center { get { return this._worldCenter; } }

    public MyCircleCollier(MyGameObject o, FpVector2 c, FpNumber r)
    {
        if (o == null)
            throw new Exception("owner is null");
        this._owner = o;
        this._localCenter = c;
        this.radius = r;

        UpdateWorldPosition();
    }

    public MyCircleCollier(FpVector2 c, FpNumber r)
    {
        this._owner = null;
        this._localCenter = c;
        this.radius = r;

        UpdateWorldPosition();
    }

    //根据transform的位置和旋转计算collider位置 
    public void UpdateWorldPosition()
    {
        if (_owner == null)
        {   //无主的碰撞体,直接使用局部坐标,并且为旋转中心为自身的中心
            this._worldCenter = this._localCenter;
            return;
        }

        //owner的世界坐标 
        FpVector2 ownerPos = this._owner.transform.position;

        //新的偏移,为原来基于owner的偏移旋转之后的值
        int angle = this._owner.transform.rotation;
        FpVector2 offset = FpVector2.RotatePoint(angle, this._localCenter);
        
        this._worldCenter = offset + ownerPos;
    }

    public bool IsPointInCircle(FpVector2 point)
    {
        FpVector2 d = point - this._worldCenter;
        if (FpVector2.Dot(d, d) <= radius * radius)
            return true;

        return false;
    }
}
