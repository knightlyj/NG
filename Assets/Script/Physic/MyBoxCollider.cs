using UnityEngine;
using System.Collections;
using System;

public class MyBoxCollider : MyCollider {
    public MyColliderType Type { get { return MyColliderType.Box; } }

    MyGameObject _owner = null;
    public MyGameObject Owner { get { return this._owner; } }

    //  .          1
    //
    //  3          2
    //
    //基于transform的局部坐标
    FpVector2[] _localVertices = new FpVector2[4];

    public FpVector2 LocalTopLeft { get { return this._localVertices[0]; } }     
    public FpVector2 LocalTopRight { get { return this._localVertices[1]; } }
    public FpVector2 LocalBottomRight { get { return this._localVertices[2]; } } 
    public FpVector2 LocalBottomLeft { get { return this._localVertices[3]; } }

    FpVector2 _localCenter;
    FpNumber _height;
    FpNumber _width;
    int _rotation;
    FpNumber _rotCos; //无主的碰撞体直接使用cos和sin,_rotation可能没有设置
    FpNumber _rotSin;
    public FpNumber Height { get { return this._height; } }
    public FpNumber Width { get { return this._width; } }
    public int Rotation { get { return this._rotation; } }
    public FpNumber RotationCos { get { return this._rotCos; } }
    public FpNumber RotataionSin { get { return this._rotSin; } }

    public MyBoxCollider(MyGameObject o, FpVector2 c, FpNumber h, FpNumber w)
    {
        if (o == null)
            throw new Exception("owner can't be null in this way");
        this._owner = o;
        this._localCenter = c;
        this._height = h;
        this._width = w;

        FpNumber halfH = h / 2;
        FpNumber halfW = w / 2;
        this._localVertices[0] = new FpVector2(c.x - halfW, c.y + halfH);
        this._localVertices[1] = new FpVector2(c.x + halfW, c.y + halfH);
        this._localVertices[2] = new FpVector2(c.x + halfW, c.y - halfH);
        this._localVertices[3] = new FpVector2(c.x - halfW, c.y - halfH);
        
        UpdateWorldPosition();
    }

    //构造一个无主的box,用来检测碰撞,需要 中心的世界坐标,高度,宽度,旋转角度
    public MyBoxCollider(FpVector2 c, FpNumber w, FpNumber h, FpNumber cos, FpNumber sin)
    {
        this._owner = null;
        this._localCenter = c;
        this._height = h;
        this._width = w;
        this._rotCos = cos;
        this._rotSin = sin;

        FpNumber halfH = h / 2;
        FpNumber halfW = w / 2;
        this._localVertices[0] = new FpVector2(-halfW, halfH); //各顶点相对于中心点的坐标 
        this._localVertices[1] = new FpVector2(halfW, halfH);
        this._localVertices[2] = new FpVector2(halfW, -halfH);
        this._localVertices[3] = new FpVector2(-halfW, -halfH);

        UpdateWorldPosition();
    }
    //无主的box,但用rotation初始化
    public MyBoxCollider(FpVector2 c, FpNumber w, FpNumber h, int rot)
    {
        this._owner = null;
        this._localCenter = c;
        this._height = h;
        this._width = w;
        this._rotation = rot;
        this._rotCos = TriFunction.Cos(rot);
        this._rotSin = TriFunction.Sin(rot);

        FpNumber halfH = h / 2;
        FpNumber halfW = w / 2;
        this._localVertices[0] = new FpVector2(-halfW, halfH); //各顶点相对于中心点的坐标 
        this._localVertices[1] = new FpVector2(halfW, halfH);
        this._localVertices[2] = new FpVector2(halfW, -halfH);
        this._localVertices[3] = new FpVector2(-halfW, -halfH);

        UpdateWorldPosition();
    }


    //世界坐标        
    FpVector2[] _worldVertices  = new FpVector2[4];
    public FpVector2 TopLeft {get { return this._worldVertices[0]; } }
    public FpVector2 TopRight { get { return this._worldVertices[1]; } }
    public FpVector2 BottomRight { get { return this._worldVertices[2]; } }
    public FpVector2 BottomLeft { get { return this._worldVertices[3]; } }

    FpVector2 _worldCenter;
    public FpVector2 Center { get { return this._worldCenter; } }

    public FpVector2 GetVertex(int i)
    {
        if (i < 0 || i > 3)
            throw new Exception("invalid vertex index");

        return this._worldVertices[i];
    }

    //根据transform的位置和旋转计算collider位置 
    public void UpdateWorldPosition()
    {
        if (_owner == null)
        {   //无主的碰撞体,直接使用局部坐标,并且为旋转中心为自身的中心
            for(int i = 0; i < 4; i++)
            {
                this._worldVertices[i] = FpVector2.RotatePoint(this._rotCos, this._rotSin, this._localVertices[i]) + this._localCenter;
            }
            this._worldCenter = this._localCenter;
        }
        else
        {
            FpVector2 ownerPos = this._owner.transform.position;
            this._rotation = this._owner.transform.rotation;
            this._rotCos = TriFunction.Cos(this._rotation);
            this._rotSin = TriFunction.Sin(this._rotation);
            //基于owner的坐标系旋转
            FpVector2 offsetCenter = FpVector2.RotatePoint(this._rotation, this._localCenter);

            //计算各个顶点的世界坐标
            for (int i = 0; i < 4; i++)
            {
                this._worldVertices[i] = FpVector2.RotatePoint(this._rotation, this._localVertices[i]) + ownerPos;
            }
            //计算中心的坐标
            this._worldCenter = offsetCenter + ownerPos;
        }

        
    }
}
