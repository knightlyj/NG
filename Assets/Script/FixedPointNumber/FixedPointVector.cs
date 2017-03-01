using UnityEngine;
using System.Collections;

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

    public static FpVector2 operator *(FpVector2 v, FpNumber n)
    {
        return new FpVector2(v.x * n, v.y * n);
    }

    public static FpVector2 operator /(FpVector2 v, FpNumber n)
    {
        return new FpVector2(v.x / n, v.y / n);
    }

    public static FpNumber Dot(FpVector2 v1, FpVector2 v2)
    {
        FpNumber n = v1.x * v2.x + v1.y * v2.y;
        return n;
    }
    
    //把point转换到此坐标系的坐标
    public static FpVector2 TransToCoorSys(FpVector2 origin, int angle, FpVector2 point)
    {
        FpVector2 p = point - origin;
        //Debug.Log("p " + p);
        return RotatePoint(-angle, p);
    }
    
    public static FpVector2 TransToCoorSys(FpVector2 origin, FpNumber angleCos, FpNumber angleSin, FpVector2 point)
    {
        FpVector2 p = point - origin;
        //Debug.Log("p " + p);
        return RotatePoint(angleCos, -angleSin, p);
    }

    //point绕原点旋转angle
    public static FpVector2 RotatePoint(int angle, FpVector2 point)
    {
        FpVector2 p = RotatePoint(TriFunction.Cos(angle), TriFunction.Sin(angle), point);
        return p;
    }

    //point绕原点旋转,直接用三角函数值
    public static FpVector2 RotatePoint(FpNumber cos, FpNumber sin, FpVector2 point)
    {
        FpVector2 p = new FpVector2();
        //Debug.Log("cos " + cos + ", sin " + sin);
        p.x = cos * point.x - sin * point.y;
        p.y = sin * point.x + cos * point.y;
        //Debug.Log("before " + point + " after " + p);
        return p;
    }

    public override string ToString()
    {
        return string.Format(this.x.Value.ToString("f2") + ", " + this.y.Value.ToString("f2"));
    }
}


