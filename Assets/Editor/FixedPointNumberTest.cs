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

    readonly double Error = 0.05;

    bool Addition()
    {
        for (int i = 0; i < 10; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a + b;
            FixedPointNumber n1 = new FixedPointNumber(a);
            FixedPointNumber n2 = new FixedPointNumber(b);
            FixedPointNumber n3 = n1 + n2;
            FixedPointNumber n4 = new FixedPointNumber(a + b);

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
        for (int i = 0; i < 10; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a - b;
            FixedPointNumber n1 = new FixedPointNumber(a);
            FixedPointNumber n2 = new FixedPointNumber(b);
            FixedPointNumber n3 = n1 - n2;
            FixedPointNumber n4 = new FixedPointNumber(a - b);

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
        for (int i = 0; i < 10; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a * b;
            FixedPointNumber n1 = new FixedPointNumber(a);
            FixedPointNumber n2 = new FixedPointNumber(b);
            FixedPointNumber n3 = n1 * n2;
            FixedPointNumber n4 = new FixedPointNumber(a * b);

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
        for (int i = 0; i < 10; i++)
        {
            double a = (double)UnityEngine.Random.Range(1.0f, 1000000.0f);
            double b = (double)UnityEngine.Random.Range(50000.0f, 200000.0f);
            double c = a / b;
            FixedPointNumber n1 = new FixedPointNumber(a);
            FixedPointNumber n2 = new FixedPointNumber(b);
            FixedPointNumber n3 = n1 / n2;
            FixedPointNumber n4 = new FixedPointNumber(a / b);

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

            FixedPointNumber sin2 = TriFunction.Sin(i);
            FixedPointNumber cos2 = TriFunction.Cos(i);
            
            double error = Math.Abs(sin - sin2.Value);
            Assert.Less(error, 0.01f);

            error = Math.Abs(cos - cos2.Value);
            Assert.Less(error, 0.01f);
        }
    }
}
