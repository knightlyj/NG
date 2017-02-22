using UnityEngine;
using System.Collections;

public class MyCircleCollier : MyCollider
{
    public MyColliderType Type { get { return MyColliderType.Circle; } }

    Transform owner = null;
    public Transform Owner { get { return this.owner; } }

    FpVector2 center;
    public FpVector2 Center { get { return this.center; } } //使用transform局部坐标

    FpNumber radius;
    public FpNumber Radius { get { return this.radius; } }
    
}
