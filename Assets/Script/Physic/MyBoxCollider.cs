using UnityEngine;
using System.Collections;

public class MyBoxCollider : MyCollider {
    public MyColliderType Type { get { return MyColliderType.Box; } }

    Transform owner = null;
    public Transform Owner { get { return this.owner; } }

    FpVector2 topLeft;
    public FpVector2 TopLeft { get { return this.topLeft; } } //使用transform局部坐标

    FpVector2 rightBottom;
    public FpVector2 RightBottom { get { return this.rightBottom; } } //使用transform局部坐标
}
