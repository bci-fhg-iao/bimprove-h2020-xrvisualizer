using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    public int X;
    public int Y;
    public static implicit operator Vector2(Point p)
    {
        return new Vector2(p.X, p.Y);
    }
}