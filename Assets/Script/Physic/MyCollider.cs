using UnityEngine;
using System.Collections;
using System;


public enum MyColliderType
{
    Nothing,
    Circle,
    Box,
}

public interface MyCollider {
    MyColliderType Type{get;}
    void UpdateWorldPosition();
}

public class MyColliderManager
{

    public static bool CheckColliding(MyCollider mc1, MyCollider mc2)
    {
        switch (mc1.Type)
        {
            case MyColliderType.Box:
                if (mc2.Type == MyColliderType.Box)
                    return BoxBox((MyBoxCollider)mc1, (MyBoxCollider)mc2);
                else if (mc2.Type == MyColliderType.Circle)
                    return BoxCircle((MyBoxCollider)mc1, (MyCircleCollier)mc2);
                break;
            case MyColliderType.Circle:
                if (mc2.Type == MyColliderType.Box)
                    return BoxCircle((MyBoxCollider)mc2, (MyCircleCollier)mc1);
                else if (mc2.Type == MyColliderType.Circle)
                    return CircleCircle((MyCircleCollier)mc1, (MyCircleCollier)mc2);
                    break;
            default:
                Debug.LogError("unrecognized collider type");
                return false;
        }
        return false;
    }
    //*******************碰撞检测*****************************
    //box和circle
    private static bool BoxCircle(MyBoxCollider box, MyCircleCollier circle)
    {
        FpVector2 h = new FpVector2(box.Width / 2, box.Height / 2);
        FpVector2 v = FpVector2.TransToCoorSys(box.Center, box.RotationCos, box.RotataionSin, circle.Center);

        if (v.x < 0)
            v.x = -v.x;
        if (v.y < 0)
            v.y = -v.y;

        FpVector2 u = v - h;
        if (u.x < 0)
            u.x = 0;
        if (u.y < 0)
            u.y = 0;

        if (FpVector2.Dot(u, u) < circle.Radius * circle.Radius)
            return true;
       

        return false;
    }

    //box和box,使用分离轴判断
    private static bool BoxBox(MyBoxCollider box1, MyBoxCollider box2)
    {
        //将box2投影到box1的坐标系
        FpNumber minX = FpNumber.MaxValue();
        FpNumber maxX = FpNumber.MinValue();
        FpNumber minY = FpNumber.MaxValue();
        FpNumber maxY = FpNumber.MinValue();
        //Debug.Log("comparison 1-----------------");
        for (int i = 0; i < 4; i++)
        {
            FpVector2 vertex = FpVector2.TransToCoorSys(box1.Center, box1.RotationCos, box1.RotataionSin, box2.GetVertex(i));
            //Debug.Log("before " + box2.GetVertex(i) + " after " + vertex);
            if (vertex.x <= minX)
                minX = vertex.x;
            if (vertex.x >= maxX)
                maxX = vertex.x;

            if (vertex.y <= minY)
                minY = vertex.y;
            if (vertex.y >= maxY)
                maxY = vertex.y;
        }

        //Debug.Log("x " + minX + " ~ " + maxX);
        //Debug.Log("y " + minY + " ~ " + maxY);

        if (minX >= (box1.Width/2) || maxX <= -(box1.Width / 2))
        {   //没重叠,则存在分离轴,不相交
            Debug.Log("1");
            return false;
        }
        if (minY >= (box1.Height / 2) || maxY <= -(box1.Height / 2))
        {
            Debug.Log("2");
            return false;
        }

        //将box1投影到box2的坐标系
        minX = FpNumber.MaxValue();
        maxX = FpNumber.MinValue();
        minY = FpNumber.MaxValue();
        maxY = FpNumber.MinValue();
        //Debug.Log("comparison 2--------------");
        for (int i = 0; i < 4; i++)
        {
            FpVector2 vertex = FpVector2.TransToCoorSys(box2.Center, box2.RotationCos, box2.RotataionSin, box1.GetVertex(i));
            //Debug.Log(vertex);
            if (vertex.x <= minX)
                minX = vertex.x;
            if (vertex.x >= maxX)
                maxX = vertex.x;

            if (vertex.y <= minY)
                minY = vertex.y;
            if (vertex.y >= maxY)
                maxY = vertex.y;
        }

        //Debug.Log("x " + minX + " ~ " + maxX);
        //Debug.Log("y " + minY + " ~ " + maxY);

        if (minX >= (box2.Width / 2) || maxX <= -(box2.Width / 2))
        {   //没重叠,则存在分离轴,不相交
            Debug.Log("3");
            return false;
        }
        if (minY >= (box2.Height / 2) || maxY <= -(box2.Height / 2))
        {
            Debug.Log("4");
            return false;
        }
        Debug.Log("5");
        return true;
    }

    //circle和circle
    private static bool CircleCircle(MyCircleCollier circle1, MyCircleCollier circle2)
    {
        FpVector2 d = circle1.Center - circle2.Center;
        FpNumber distSquare = FpVector2.Dot(d, d);
        FpNumber interSquare = (circle1.Radius + circle2.Radius) * (circle1.Radius + circle2.Radius);
        //Debug.Log("dis " + distSquare.Value + ", inter " + interSquare.Value);
        if (distSquare <= interSquare)
            return true;
        return false;
    }
    
    public static bool LineCircle(FpVector2 from, FpVector2 to, MyCircleCollier circle)
    {
        if (circle.IsPointInCircle(from) || circle.IsPointInCircle(to))
        {   //起始点或终点在内部,则相交
            //Debug.Log("1");
            return true;
        }
        //以下起点和终点都在圆外部
        FpVector2 dir = to - from;
        FpNumber length = FpNumber.Sqrt(FpVector2.Dot(dir, dir));
        FpNumber cos = dir.x / length;
        FpNumber sin = dir.y / length;
        FpVector2 center = FpVector2.TransToCoorSys(from, cos, sin, circle.Center);
        //Debug.Log("c2 " + center);

        if (FpNumber.Abs(center.y) >= circle.Radius)
        { //垂直距离比半径大,不相交
            //Debug.Log("2");
            return false;
        }

        if (center.x < 0 || center.x > length)
        {   //射线方向远离圆 或 圆心投影在终点之后,不相交
            //Debug.Log("x" + center.x);
            //Debug.Log("length "+ length);
            //Debug.Log("3");
            return false;
        }
        else
        {
            //Debug.Log("4");
            return true;
        }
    }

    public static bool LineBox(FpVector2 from, FpVector2 to, MyBoxCollider box)
    {
        FpVector2 dir = to - from;
        FpNumber length = FpNumber.Sqrt(FpVector2.Dot(dir, dir));
        FpNumber cos = dir.x / length;
        FpNumber sin = dir.y / length;
        //Debug.Log("cos " + cos);
        //Debug.Log("sin " + sin);
        FpVector2 fakeBoxCenter = (from + to) / 2;
        //Debug.Log("center " + fakeBoxCenter);
        MyBoxCollider fakeBox = new MyBoxCollider(fakeBoxCenter, length, FpNumber.Small()*2, cos, sin);
        return BoxBox(fakeBox, box);
    }
}

internal class QuadTreeNode
{
    QuadTreeNode father;
    QuadTreeNode child1;
    QuadTreeNode child2;
    QuadTreeNode child3;
    QuadTreeNode child4;
}

