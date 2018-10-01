using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Fraction
{
    public long Up { get; private set; }
    private static string Digit = "0.000000";
    public long Down { get; private set; }
    public Fraction(int up)
    {
        Up = up;
        Down = 1;
    }
    public Fraction(int up, int down)
    {
        if (down == 0)
        {
            Debug.LogError("分母に0が代入されています");
        }
        var a=Reduce(up,down);
        Up = a[0];
        Down = a[1];
    }
    public Fraction(Fraction a)
    {
        Up = a.Up;
        Down = a.Down;
    }

    public Fraction(double a) {
        long b = 1000000;
        Up = (long)(a * b);
        Down = b;
        var re=Reduce(Up,Down);
        Up = re[0];
        Down = re[1];
    }

    public override string ToString()
    {
        if (Down == 1) return ((double)Up).ToString(Digit);
        return (Up / (double)Down).ToString("0.000000");
    }

    public double ToDouble()
    {
        return Up / (double)Down;
    }

    public double ToDouble(int index)
    {
        double result = 0;
        for (int i = 0; i <= index; i++)
        {
            long value = (i + 1) * Up / Down;
            if (value >= 1)
            {
                result += value / Ten(i);
            }
        }
        return result;
    }
    public static double Ten(int n)
    {
        const double ten = 10.000000000000000000000000000000000000000000;
        double result = 1;
        for (int i = 0; i < n; i++)
        {
            result *= ten;
        }
        return result;
    }
    public static Fraction operator +(Fraction a, Fraction b)
    {
        var c = new Fraction
        {
            Down = a.Down * b.Down,
            Up = a.Up * b.Down + b.Up * a.Down
        };
        return c;
    }
    public static Fraction operator +(int a, Fraction b)
    {
        return new Fraction(a) + b;
    }
    public static Fraction operator +(Fraction a, int b)
    {
        return a + new Fraction(b);
    }
    public static Fraction operator -(Fraction a, Fraction b)
    {
        var c = new Fraction
        {
            Down = a.Down * b.Down,
            Up = a.Up * b.Down - b.Up * a.Down
        };
        return c;
    }
    public static Fraction operator -(int a, Fraction b)
    {
        return new Fraction(a) + b;
    }
    public static Fraction operator -(Fraction a, int b)
    {
        return a + new Fraction(b);
    }
    public static Fraction operator *(Fraction a, Fraction b)
    {
        var c = new Fraction
        {
            Up = a.Up * b.Up,
            Down = a.Down * b.Down
        };
        return c;
    }
    public static Fraction operator *(int a, Fraction b)
    {
        return new Fraction(a) * b;
    }
    public static Fraction operator *(Fraction a, int b)
    {
        return a * new Fraction(b);
    }
    public static Fraction operator /(Fraction a, Fraction b)
    {
        var c = new Fraction
        {
            Up = a.Up * b.Down,
            Down = a.Down * b.Up
        };
        return c;
    }
    public static Fraction operator /(int a, Fraction b)
    {
        return new Fraction(a) / b;
    }
    public static Fraction operator /(Fraction a, int b)
    {
        return a / new Fraction(b);
    }
    public static Fraction operator ^(Fraction a, Fraction b)
    {
        var c = new Fraction(1);
        for (var i = 0; i < b.Up; i++)
        {
            c = c * a;
        }
        if (b.Down != 1) Debug.LogError("無理数は未定義です");
        return c;
    }
    public static Fraction operator ^(Fraction a, int b)
    {
        return a ^ new Fraction(b);
    }
    private static long[] Reduce(long up,long down)
    {
        bool positive;
        if (up >= 0)
        {
            if (down >= 0) positive = true;
            else
            {
                positive = false;
                down = -down;
            }
        }
        else
        {
            up = -up;
            if (down >= 0) positive = false;
            else
            {
                positive = true;
                down = -down;
            }
        }
        var r = Gcd(up, down);
        up = up / r;
        down = down / r;
        if (!positive) up = -up;
        long[] result = new long[] { up, down };
        return result;
    }
    
    private static long Gcd(long m, long n)
    {
        while (true)
        {
            var r = m % n;
            if (r == 0) return n;
            m = n;
            n = r;
        }
    }
}
