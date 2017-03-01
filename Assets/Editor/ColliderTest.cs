using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using System;

public class ColliderTest {

	[Test]
    public void CircleCircleTest()
    {
        MyCircleCollier c1 = new MyCircleCollier(new FpVector2(0, 0), 1);
        MyCircleCollier c2 = new MyCircleCollier(new FpVector2(0, 2.01), 1);
        bool res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, false);

        c1 = new MyCircleCollier(new FpVector2(0, 0), 1);
        c2 = new MyCircleCollier(new FpVector2(0, 1.99), 1);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, true);

        c1 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c2 = new MyCircleCollier(new FpVector2(20, 26.2), 4);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, false);

        c1 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c2 = new MyCircleCollier(new FpVector2(30, 26.2), 5);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, false);

        c1 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c2 = new MyCircleCollier(new FpVector2(19, 26.2), 4);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, true);
        
        c1 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c2 = new MyCircleCollier(new FpVector2(20, 26.1), 4);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, true);

        c1 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c2 = new MyCircleCollier(new FpVector2(20, 26.2), 4.1);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, true);

        c2 = new MyCircleCollier(new FpVector2(10, 10), 15);
        c1 = new MyCircleCollier(new FpVector2(5.3, 5.8), 3.26);
        res = MyColliderManager.CheckColliding(c1, c2);
        Assert.AreEqual(res, true);
    }

    [Test]
    public void CircleBoxTest()
    {
        MyBoxCollider box = new MyBoxCollider(new FpVector2(-10.9, -8.4), 3, 3, 0);
        MyCircleCollier circle = new MyCircleCollier(new FpVector2(-4.1, -6.9), 5);
        bool res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(-10.9, -8.4), 3, 3, 0);
        circle = new MyCircleCollier(new FpVector2(-4.95, -7.7), 5);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(-17.87, -12.92), 5, 3, 0);
        circle = new MyCircleCollier(new FpVector2(-13.69, -7.78), 15);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(27.3, -7.8), 5, 10, 0);
        circle = new MyCircleCollier(new FpVector2(21.9, -1.9), 7);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(27.7, -7), 5, 10, 0);
        circle = new MyCircleCollier(new FpVector2(55.9, 24.8), 7);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(37, 3.4), 5, 10, -20);
        circle = new MyCircleCollier(new FpVector2(47.1, 6.3), 7);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(37, 3.4), 5, 10, 20);
        circle = new MyCircleCollier(new FpVector2(47.1, 6.3), 7);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(53.1, 11.8), 5, 10, 60);
        circle = new MyCircleCollier(new FpVector2(47.1, 6.3), 7);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(3.57, 11.93), 3, 1, -30);
        circle = new MyCircleCollier(new FpVector2(5.02, 10.31), 2);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(6.21, 9.19), 3, 1, 45);
        circle = new MyCircleCollier(new FpVector2(5.02, 10.31), 1);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(6.27, 11.83), 3, 1, 45);
        circle = new MyCircleCollier(new FpVector2(5.02, 10.31), 1);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(6.27, 11.83), 3, 1, 120);
        circle = new MyCircleCollier(new FpVector2(5.02, 10.31), 1);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(-9.52, -10.67), 2, 8, 90);
        circle = new MyCircleCollier(new FpVector2(-9.43, -4.52), 5);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(-9.52, -10.67), 2, 8, 0);
        circle = new MyCircleCollier(new FpVector2(-9.43, -4.52), 5);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(1280, -2004), 2, 8, 0);
        circle = new MyCircleCollier(new FpVector2(1284.02, -2010.14), 5);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(1280, -2004), 2, 8, 100);
        circle = new MyCircleCollier(new FpVector2(1284.02, -2010.14), 5);
        res = MyColliderManager.CheckColliding(box, circle);
        Assert.AreEqual(res, false);
    }

    [Test]
    public void BoxBoxTest()
    {
        MyBoxCollider box1 = new MyBoxCollider(new FpVector2(54, 99), 2, 8, 100);
        MyBoxCollider box2 = new MyBoxCollider(new FpVector2(50, 100), 2, 8, 100);
        bool res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, true);

        box1 = new MyBoxCollider(new FpVector2(47.76, 101.57), 6, 10, 85);
        box2 = new MyBoxCollider(new FpVector2(53.74, 96.23), 2, 8, 55);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, true);

        box1 = new MyBoxCollider(new FpVector2(47.76, 101.57), 6, 10, 85);
        box2 = new MyBoxCollider(new FpVector2(53.74, 96.23), 2, 8, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, false);

        box1 = new MyBoxCollider(new FpVector2(47.76, 101.57), 6, 10, 0);
        box2 = new MyBoxCollider(new FpVector2(53.74, 96.23), 2, 8, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, true);

        box1 = new MyBoxCollider(new FpVector2(55.57, 99.23), 5, 3, 0);
        box2 = new MyBoxCollider(new FpVector2(53.74, 96.23), 2, 8, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, false);

        box1 = new MyBoxCollider(new FpVector2(55.57, 99.23), 5, 3, 0);
        box2 = new MyBoxCollider(new FpVector2(53.74, 96.23), 6, 1, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, true);

        box1 = new MyBoxCollider(new FpVector2(55.57, 99.23), 5, 3, 0);
        box2 = new MyBoxCollider(new FpVector2(56.31, 103.97), 6, 1, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, false);

        box1 = new MyBoxCollider(new FpVector2(55.57, 99.23), 5, 3, 0);
        box2 = new MyBoxCollider(new FpVector2(58.27, 98.99), 6, 1, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, true);

        box1 = new MyBoxCollider(new FpVector2(45.41, 102.51), 5, 3, 0);
        box2 = new MyBoxCollider(new FpVector2(58.27, 98.99), 6, 1, 75);
        res = MyColliderManager.CheckColliding(box1, box2);
        Assert.AreEqual(res, false);
    }

    [Test]
    public void LineCircleTest()
    {
        MyCircleCollier circle = new MyCircleCollier(new FpVector2(46.2, 11.4), 3.5);
        bool res = MyColliderManager.LineCircle(new FpVector2(36.1, 9.2), new FpVector2(56.01, 8.92), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(46.2, 11.4), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(36.1, 9.2), new FpVector2(55.9, 8.6), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(40.14, 12.51), 3);
        res = MyColliderManager.LineCircle(new FpVector2(36.1, 9.2), new FpVector2(55.9, 8.6), circle);
        Assert.AreEqual(res, false);

        circle = new MyCircleCollier(new FpVector2(40.14, 12.51), 3);
        res = MyColliderManager.LineCircle(new FpVector2(44.34, 11.27), new FpVector2(49.69, 11.77), circle);
        Assert.AreEqual(res, false);

        circle = new MyCircleCollier(new FpVector2(53.36, 14.58), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(44.34, 11.27), new FpVector2(49.69, 11.77), circle);
        Assert.AreEqual(res, false);

        circle = new MyCircleCollier(new FpVector2(49.26, 7.13), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(44.34, 11.27), new FpVector2(49.69, 11.77), circle);
        Assert.AreEqual(res, false);

        circle = new MyCircleCollier(new FpVector2(49.26, 7.13), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(48.13, 6.6), new FpVector2(49.69, 11.77), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(49.26, 7.13), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(48.13, 6.6), new FpVector2(51.14, 6.09), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(49.26, 7.13), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(48.13, 6.6), new FpVector2(51.14, 6.09), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(49.26, 7.13), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(43.94, 5.54), new FpVector2(51.14, 6.09), circle);
        Assert.AreEqual(res, true);

        circle = new MyCircleCollier(new FpVector2(17.1, 24.7), 3.5);
        res = MyColliderManager.LineCircle(new FpVector2(43.94, 5.54), new FpVector2(51.14, 6.09), circle);
        Assert.AreEqual(res, false);
    }

    [Test]
    public void LineBoxTest()
    {
        MyBoxCollider box = new MyBoxCollider(new FpVector2(46.82, 7.64), 2, 2, 0);
        bool res = MyColliderManager.LineBox(new FpVector2(42.15, 5.29), new FpVector2(48.57, 7.45), box);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(43.28, 7.49), 2, 2, 0);
        res = MyColliderManager.LineBox(new FpVector2(42.15, 5.29), new FpVector2(48.57, 7.45), box);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(50.21, 8.33), 2, 2, 60);
        res = MyColliderManager.LineBox(new FpVector2(42.15, 5.29), new FpVector2(48.57, 7.45), box);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(48.7, 5.68), 6, 2, 60);
        res = MyColliderManager.LineBox(new FpVector2(47.55, 4.03), new FpVector2(49.04, 5.72), box);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(48.7, 5.68), 6, 2, 60);
        res = MyColliderManager.LineBox(new FpVector2(46.74, 2.92), new FpVector2(50.44, 8.48), box);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(46.6, 6.3), 6, 2, 60);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92),  box);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(52.32, 4.71), 6, 2, 160);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92), box);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(50.94, 3.81), 6, 2, 160);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92), box);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(50.94, 3.81), 6, 2, 100);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92), box);
        Assert.AreEqual(res, false);

        box = new MyBoxCollider(new FpVector2(50.94, 3.81), 6, 2, 120);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92), box);
        Assert.AreEqual(res, true);

        box = new MyBoxCollider(new FpVector2(45.78, 2.02), 6, 2, 120);
        res = MyColliderManager.LineBox(new FpVector2(50.44, 8.48), new FpVector2(46.74, 2.92), box);
        Assert.AreEqual(res, false);
    }
}
