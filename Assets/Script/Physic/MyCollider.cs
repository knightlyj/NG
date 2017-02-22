using UnityEngine;
using System.Collections;
using System;

public struct FpVector2
{
    public FpNumber x;
    public FpNumber y;

    public FpVector2(double x, double y)
    {
        this.x = new FpNumber(x);
        this.y = new FpNumber(y);
    }

    public FpVector2(FpNumber x, FpNumber y)
    {
        this.x = x;
        this.y = y;
    }

    public static FpVector2 operator +(FpVector2 v1, FpVector2 v2)
    {
        FpVector2 v = new FpVector2(v1.x + v2.x, v1.y + v2.y);
        return v;
    }

    public static FpVector2 operator -(FpVector2 v1, FpVector2 v2)
    {
        FpVector2 v = new FpVector2(v1.x - v2.x, v1.y - v2.y);
        return v;
    }
    
    public static FpNumber Dot(FpVector2 v1, FpVector2 v2)
    {
        FpNumber n = v1.x * v2.x + v1.y * v2.y;
        return n;
    }
}

public enum MyColliderType
{
    Nothing,
    Circle,
    Box,
}

public interface MyCollider {
    MyColliderType Type{get;}
}

public class MyColliderManager
{
    
}

