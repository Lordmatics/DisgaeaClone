using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basically a custom Vector2, but uses ints not floats, and contains
// static functions for all directions
// Just overload more operators if and when any are needed
// syntax is copy pastable
public struct Coord
{
    public int x;
    public int z;

    public Coord(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public static Coord Zero()
    {
        return new Coord(0, 0);
    }

    public static Coord NE()
    {
        return Up() + Right();
    }

    public static Coord SE()
    {
        return Down() + Right();
    }

    public static Coord SW()
    {
        return Down() + Left();
    }

    public static Coord NW()
    {
        return Up() + Left();
    }

    public static Coord Up()
    {
        return new Coord(0, 1);
    }

    public static Coord Right()
    {
        return new Coord(1, 0);
    }

    public static Coord Left()
    {
        return -Right();
    }

    public static Coord Down()
    {
        return -Up();
    }

    public static Coord operator +(Coord a, Coord b)
    {
        Coord temp = new Coord();
        temp.x = a.x + b.x;
        temp.z = a.z + b.z;
        return temp;
    }

    public static Coord operator +(Coord a, Vector2 b)
    {
        Coord temp = new Coord();
        temp.x = a.x + (int)b.x;
        temp.z = a.z + (int)b.y;
        return temp;
    }

    public static Coord operator -(Coord a)
    {
        Coord temp = new Coord();
        temp.x = -temp.x;
        temp.z = -temp.z;
        return temp;
    }

    public static Coord operator -(Coord a, Coord b)
    {
        Coord temp = new Coord();
        temp.x = a.x - b.x;
        temp.z = a.z - b.z;
        return temp;
    }

    public static Coord operator *(Coord a, Coord b)
    {
        Coord temp = new Coord();
        temp.x = a.x * b.x;
        temp.z = a.z * b.z;
        return temp;
    }

    public static Coord operator *(Coord a, int b)
    {
        Coord temp = new Coord();
        temp.x = a.x * b;
        temp.z = a.z * b;
        return temp;
    }

    public static bool operator ==(Coord a, Coord b)
    {
        // Neccesary if a type within structure is "nullable"

        //if (object.ReferenceEquals(a, b))
        //{
        //    return true;
        //}
        //if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
        //{
        //    return false;
        //}

        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(Coord a, Coord b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        // Neccesary if a type within structure is "nullable"
        //return this == (obj as Coord);

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

}
