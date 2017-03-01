using UnityEngine;
using System.Collections;
using System;

public struct FpNumber
{
    public Int64 number;
    static readonly int SigOfFraction = 2; //小数部分有效位数
    static readonly double NumberScale = Math.Pow(10, SigOfFraction);

    public FpNumber(double d)
    {
        this.number = (Int64)Math.Round(d * NumberScale);
    }

    public double Value
    {
        get
        {
            return ((double)this.number) / NumberScale;
        }
    }

    public override string ToString()
    {
        return this.Value.ToString("f2");
    }

    public static FpNumber MinValue()
    {
        FpNumber n = new FpNumber();
        n.number = Int64.MinValue;
        return n;
    }

    public static FpNumber Small()
    {
        FpNumber n = new FpNumber();
        n.number = 1;
        return n;
    }

    public static FpNumber MaxValue()
    {
        FpNumber n = new FpNumber();
        n.number = Int64.MaxValue;
        return n;
    }

    public static FpNumber Sqrt(FpNumber n)
    {
        if (n.number < 0)
            throw new Exception("sqrt invalid number");
        FpNumber root = new FpNumber();
        root.number = (Int64)Math.Round(Math.Sqrt(n.number) * 10);
        return root;
    }

    public static FpNumber Abs(FpNumber n)
    {
        FpNumber abs = new FpNumber();
        abs.number = Math.Abs(n.number);
        return abs;
    }

    //****************double的类型转换**************************
    public static implicit operator FpNumber(double d)
    {
        FpNumber n = new FpNumber(d);
        return n;
    }

    //**********************四则运算*****************************
    // +
    public static FpNumber operator +(FpNumber n1, FpNumber n2)
    {
        FpNumber n = new FpNumber();
        n.number = n1.number + n2.number;
        return n;
    }

    // -
    public static FpNumber operator -(FpNumber n1, FpNumber n2)
    {
        FpNumber n = new FpNumber();
        n.number = n1.number - n2.number;
        return n;
    }

    // *
    public static FpNumber operator *(FpNumber n1, FpNumber n2)
    {
        FpNumber n = new FpNumber();
        n.number = n1.number * n2.number / (Int64)NumberScale;
        return n;
    }

    // /
    public static FpNumber operator /(FpNumber n1, FpNumber n2)
    {
        FpNumber n = new FpNumber();
        n.number = n1.number * (Int64)NumberScale / n2.number ;
        return n;
    }

    //**************比较运算*******************
    public static bool operator < (FpNumber n1, FpNumber n2)
    {
        return n1.number < n2.number;
    }

    public static bool operator > (FpNumber n1, FpNumber n2)
    {
        return n1.number > n2.number;
    }

    public static bool operator <= (FpNumber n1, FpNumber n2)
    {
        return n1.number <= n2.number;
    }

    public static bool operator >= (FpNumber n1, FpNumber n2)
    {
        return n1.number >= n2.number;
    }

    //负数
    public static FpNumber operator -(FpNumber n1)
    {
        FpNumber n = new FpNumber();
        n.number = -n1.number;
        return n;
    }

    //*************与double的四则运算*******************
    // +
    public static FpNumber operator +(FpNumber n1, double d)
    {
        FpNumber n = n1 + new FpNumber(d);
        return n;
    }

    // -
    public static FpNumber operator -(FpNumber n1, double d)
    {
        FpNumber n = n1 - new FpNumber(d);
        return n;
    }

    // *
    public static FpNumber operator *(FpNumber n1, double d)
    {
        FpNumber n = n1 * new FpNumber(d);
        return n;
    }

    // /
    public static FpNumber operator /(FpNumber n1, double d)
    {
        FpNumber n = n1 / new FpNumber(d);
        return n;
    }

}

public static class TriFunction
{
    private static readonly double[] CosTable = {1.00,1.00,1.00,1.00,1.00,1.00,0.99,0.99,0.99,0.99,0.98,0.98,0.98,0.97,0.97,0.97,0.96,0.96,0.95,0.95,0.94,
                0.93,0.93,0.92,0.91,0.91,0.90,0.89,0.88,0.87,0.87,0.86,0.85,0.84,0.83,0.82,0.81,0.80,0.79,0.78,0.77,
                0.75,0.74,0.73,0.72,0.71,0.69,0.68,0.67,0.66,0.64,0.63,0.62,0.60,0.59,0.57,0.56,0.54,0.53,0.52,0.50,
                0.48,0.47,0.45,0.44,0.42,0.41,0.39,0.37,0.36,0.34,0.33,0.31,0.29,0.28,0.26,0.24,0.22,0.21,0.19,0.17,
                0.16,0.14,0.12,0.10,0.09,0.07,0.05,0.03,0.02,0.00,-0.02,-0.03,-0.05,-0.07,-0.09,-0.10,-0.12,-0.14,-0.16,-0.17,
                -0.19,-0.21,-0.22,-0.24,-0.26,-0.28,-0.29,-0.31,-0.33,-0.34,-0.36,-0.37,-0.39,-0.41,-0.42,-0.44,-0.45,-0.47,-0.48,-0.50,
                -0.52,-0.53,-0.54,-0.56,-0.57,-0.59,-0.60,-0.62,-0.63,-0.64,-0.66,-0.67,-0.68,-0.69,-0.71,-0.72,-0.73,-0.74,-0.75,-0.77,
                -0.78,-0.79,-0.80,-0.81,-0.82,-0.83,-0.84,-0.85,-0.86,-0.87,-0.87,-0.88,-0.89,-0.90,-0.91,-0.91,-0.92,-0.93,-0.93,-0.94,
                -0.95,-0.95,-0.96,-0.96,-0.97,-0.97,-0.97,-0.98,-0.98,-0.98,-0.99,-0.99,-0.99,-0.99,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,
                -1.00,-1.00,-1.00,-1.00,-1.00,-0.99,-0.99,-0.99,-0.99,-0.98,-0.98,-0.98,-0.97,-0.97,-0.97,-0.96,-0.96,-0.95,-0.95,-0.94,
                -0.93,-0.93,-0.92,-0.91,-0.91,-0.90,-0.89,-0.88,-0.87,-0.87,-0.86,-0.85,-0.84,-0.83,-0.82,-0.81,-0.80,-0.79,-0.78,-0.77,
                -0.75,-0.74,-0.73,-0.72,-0.71,-0.69,-0.68,-0.67,-0.66,-0.64,-0.63,-0.62,-0.60,-0.59,-0.57,-0.56,-0.54,-0.53,-0.52,-0.50,
                -0.48,-0.47,-0.45,-0.44,-0.42,-0.41,-0.39,-0.37,-0.36,-0.34,-0.33,-0.31,-0.29,-0.28,-0.26,-0.24,-0.22,-0.21,-0.19,-0.17,
                -0.16,-0.14,-0.12,-0.10,-0.09,-0.07,-0.05,-0.03,-0.02,0.00,0.02,0.03,0.05,0.07,0.09,0.10,0.12,0.14,0.16,0.17,
                0.19,0.21,0.22,0.24,0.26,0.28,0.29,0.31,0.33,0.34,0.36,0.37,0.39,0.41,0.42,0.44,0.45,0.47,0.48,0.50,
                0.52,0.53,0.54,0.56,0.57,0.59,0.60,0.62,0.63,0.64,0.66,0.67,0.68,0.69,0.71,0.72,0.73,0.74,0.75,0.77,
                0.78,0.79,0.80,0.81,0.82,0.83,0.84,0.85,0.86,0.87,0.87,0.88,0.89,0.90,0.91,0.91,0.92,0.93,0.93,0.94,
                0.95,0.95,0.96,0.96,0.97,0.97,0.97,0.98,0.98,0.98,0.99,0.99,0.99,0.99,1.00,1.00,1.00,1.00,1.00};

    private static readonly double[] SinTable = {0.00,0.02,0.03,0.05,0.07,0.09,0.10,0.12,0.14,0.16,0.17,0.19,0.21,0.22,0.24,0.26,0.28,0.29,0.31,0.33,0.34,
                0.36,0.37,0.39,0.41,0.42,0.44,0.45,0.47,0.48,0.50,0.52,0.53,0.54,0.56,0.57,0.59,0.60,0.62,0.63,0.64,
                0.66,0.67,0.68,0.69,0.71,0.72,0.73,0.74,0.75,0.77,0.78,0.79,0.80,0.81,0.82,0.83,0.84,0.85,0.86,0.87,
                0.87,0.88,0.89,0.90,0.91,0.91,0.92,0.93,0.93,0.94,0.95,0.95,0.96,0.96,0.97,0.97,0.97,0.98,0.98,0.98,
                0.99,0.99,0.99,0.99,1.00,1.00,1.00,1.00,1.00,1.00,1.00,1.00,1.00,1.00,1.00,0.99,0.99,0.99,0.99,0.98,
                0.98,0.98,0.97,0.97,0.97,0.96,0.96,0.95,0.95,0.94,0.93,0.93,0.92,0.91,0.91,0.90,0.89,0.88,0.87,0.87,
                0.86,0.85,0.84,0.83,0.82,0.81,0.80,0.79,0.78,0.77,0.75,0.74,0.73,0.72,0.71,0.69,0.68,0.67,0.66,0.64,
                0.63,0.62,0.60,0.59,0.57,0.56,0.54,0.53,0.52,0.50,0.48,0.47,0.45,0.44,0.42,0.41,0.39,0.37,0.36,0.34,
                0.33,0.31,0.29,0.28,0.26,0.24,0.22,0.21,0.19,0.17,0.16,0.14,0.12,0.10,0.09,0.07,0.05,0.03,0.02,0.00,
                -0.02,-0.03,-0.05,-0.07,-0.09,-0.10,-0.12,-0.14,-0.16,-0.17,-0.19,-0.21,-0.22,-0.24,-0.26,-0.28,-0.29,-0.31,-0.33,-0.34,
                -0.36,-0.37,-0.39,-0.41,-0.42,-0.44,-0.45,-0.47,-0.48,-0.50,-0.52,-0.53,-0.54,-0.56,-0.57,-0.59,-0.60,-0.62,-0.63,-0.64,
                -0.66,-0.67,-0.68,-0.69,-0.71,-0.72,-0.73,-0.74,-0.75,-0.77,-0.78,-0.79,-0.80,-0.81,-0.82,-0.83,-0.84,-0.85,-0.86,-0.87,
                -0.87,-0.88,-0.89,-0.90,-0.91,-0.91,-0.92,-0.93,-0.93,-0.94,-0.95,-0.95,-0.96,-0.96,-0.97,-0.97,-0.97,-0.98,-0.98,-0.98,
                -0.99,-0.99,-0.99,-0.99,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-1.00,-0.99,-0.99,-0.99,-0.99,-0.98,
                -0.98,-0.98,-0.97,-0.97,-0.97,-0.96,-0.96,-0.95,-0.95,-0.94,-0.93,-0.93,-0.92,-0.91,-0.91,-0.90,-0.89,-0.88,-0.87,-0.87,
                -0.86,-0.85,-0.84,-0.83,-0.82,-0.81,-0.80,-0.79,-0.78,-0.77,-0.75,-0.74,-0.73,-0.72,-0.71,-0.69,-0.68,-0.67,-0.66,-0.64,
                -0.63,-0.62,-0.60,-0.59,-0.57,-0.56,-0.54,-0.53,-0.52,-0.50,-0.48,-0.47,-0.45,-0.44,-0.42,-0.41,-0.39,-0.37,-0.36,-0.34,
                -0.33,-0.31,-0.29,-0.28,-0.26,-0.24,-0.22,-0.21,-0.19,-0.17,-0.16,-0.14,-0.12,-0.10,-0.09,-0.07,-0.05,-0.03,-0.02 };

    public static FpNumber Cos(int angle)
    {
        if (angle < 0)
            angle += (angle / 360) * 360 + 360;
        double res = CosTable[angle % 360];
        return new FpNumber(res);
    }

    public static FpNumber Sin(int angle)
    {
        if (angle < 0)
            angle += (angle / 360) * 360 + 360;
        double res = SinTable[angle % 360];
        return new FpNumber(res);
    }
}
