using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class FixedPointNumberTest
{

	[Test]
	public void OperationTest()
	{
        bool res = Addition();
        Assert.AreEqual(res, true);

        res = Subtraction();
        Assert.AreEqual(res, true);

        res = Multiplication();
        Assert.AreEqual(res, true);

        res = Division();
        Assert.AreEqual(res, true);
    }

    readonly double Error = 0.02;

    bool Addition()
    {
        for (int i = 0; i < 100; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a + b;
            FpNumber n1 = new FpNumber(a);
            FpNumber n2 = new FpNumber(b);
            FpNumber n3 = n1 + n2;
            FpNumber n4 = new FpNumber(a + b);

            double res = n1.Value;
            if (Math.Abs(res - a) > Error)
                return false;

            res = n2.Value;
            if(Math.Abs(res - b) > Error)
                return false;

            res = n3.Value;
            if (Math.Abs(res - c) > Error)
                return false;

            res = n4.Value;
            if (Math.Abs(res - c) > Error)
                return false;
        }
        return true;
    }

    bool Subtraction()
    {
        for (int i = 0; i < 100; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a - b;
            FpNumber n1 = new FpNumber(a);
            FpNumber n2 = new FpNumber(b);
            FpNumber n3 = n1 - n2;
            FpNumber n4 = new FpNumber(a - b);

            double res = n3.Value;
            if (Math.Abs(res - c) > Error)
                return false;

            res = n4.Value;
            if (Math.Abs(res - c) > Error)
                return false;
        }
        return true;
    }

    double ErrorRate = 0.01;
    bool Multiplication()
    {
        for (int i = 0; i < 100; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a * b;
            FpNumber n1 = new FpNumber(a);
            FpNumber n2 = new FpNumber(b);
            FpNumber n3 = n1 * n2;
            FpNumber n4 = new FpNumber(a * b);

            double res = n3.Value;
            if (Math.Abs(res - c) / c > ErrorRate)
            {
                Debug.Log("fixed " + res + ", double " + c);
                return false;
            }

            res = n4.Value;
            if (Math.Abs(res - c) / c > ErrorRate)
            {
                Debug.Log("fixed " + res + ", double " + c);
                return false;
            }
        }
        return true;
    }

    bool Division()
    {
        for (int i = 0; i < 100; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a / b;
            FpNumber n1 = new FpNumber(a);
            FpNumber n2 = new FpNumber(b);
            FpNumber n3 = n1 / n2;
            FpNumber n4 = new FpNumber(a / b);

            double res = n3.Value;
            if (Math.Abs(res - c) > ErrorRate)
            {
                Debug.Log("fixed " + res + ", double " + c);
                return false;
            }

            res = n4.Value;
            if (Math.Abs(res - c) > ErrorRate)
            {
                Debug.Log("fixed " + res + ", double " + c);
                return false;
            }
        }
        return true;
    }

    [Test]
    public void TriFuncTest()
    {
        for(int i = 0; i < 1000; i++)
        {
            double sin = Math.Sin((double)i / 180 * Math.PI);
            double cos = Math.Cos((double)i / 180 * Math.PI);

            FpNumber sin2 = TriFunction.Sin(i);
            FpNumber cos2 = TriFunction.Cos(i);
            
            double error = Math.Abs(sin - sin2.Value);
            Assert.Less(error, 0.01f);

            error = Math.Abs(cos - cos2.Value);
            Assert.Less(error, 0.01f);
        }
    }

    [Test]
    public void FpVector2Test()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 v1 = new Vector2(UnityEngine.Random.Range(1.0f, 100000.0f), UnityEngine.Random.Range(1.0f, 100000.0f));
            Vector2 v2 = new Vector2(UnityEngine.Random.Range(1000.0f, 10000.0f), UnityEngine.Random.Range(1.0f, 100000.0f));
            Vector2 v3 = v1 + v2;
            Vector2 v4 = v1 - v2;
            float dot1 = Vector2.Dot(v1, v2);
            float dot2 = Vector2.Dot(v3, v4);

            FpVector2 fpv1 = new FpVector2(v1.x, v1.y);
            FpVector2 fpv2 = new FpVector2(v2.x, v2.y);
            FpVector2 fpv3 = fpv1 + fpv2;
            FpVector2 fpv4 = fpv1 - fpv2;
            FpNumber fpDot1 = FpVector2.Dot(fpv1, fpv2);
            FpNumber fpDot2 = FpVector2.Dot(fpv3, fpv4);

            double error = 0.02;
            //v1
            double res = Math.Abs(v1.x - fpv1.x.Value);
            Assert.Less(res, error);

            res = Math.Abs(v1.y - fpv1.y.Value);
            Assert.Less(res, error);

            //v2
            res = Math.Abs(v2.x - fpv2.x.Value);
            Assert.Less(res, error);

            res = Math.Abs(v2.y - fpv2.y.Value);
            Assert.Less(res, error);

            //v3
            res = Math.Abs(v3.x - fpv3.x.Value);
            Assert.Less(res, error);

            res = Math.Abs(v3.y - fpv3.y.Value);
            Assert.Less(res, error);

            //v4
            res = Math.Abs(v4.x - fpv4.x.Value);
            Assert.Less(res, error);

            res = Math.Abs(v4.y - fpv4.y.Value);
            Assert.Less(res, error);

            //dot1
            if (Math.Abs(dot1) > 1)
            {
                res = Math.Abs(dot1 - fpDot1.Value) / dot1;
                //Debug.Log("double " + dot1 + ", fpdot " + fpDot1.Value);
                Assert.Less(res, 0.001);
            }

            //dot2
            if (Math.Abs(dot2) > 1)
            {
                res = Math.Abs(dot2 - fpDot2.Value) / dot2;
                Assert.Less(res, 0.001);
            }
        }
    }
}
