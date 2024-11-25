using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct Vector
{
    public List<float> coords;
    public int dims => coords.Count;
    public Vector(List<float> coords)
    {
        this.coords = new List<float>();
        this.coords.AddRange(coords);
    }
    public Vector(int coords)
    {
        this.coords = new List<float>();
        for (int i = 0; i < coords; i++)
        {
            this.coords.Add(0);
        }
    }
    public float this[int index]
    {
        get { return coords[index]; }
        set { coords[index] = value; }
    }
    public static Vector Max(Vector a, Vector b)
    {
        return a.dims > b.dims ? a : b;
    }
    public static Vector Min(Vector a, Vector b)
    {
        return a.dims < b.dims ? a : b;
    }
    private Vector copier()
    {
        Vector result = new Vector(dims);
        for (int i = 0; i < dims; i++)
        {
            result[i] = this[i];
        }
        return result;
    }
    public Vector copy => copier();
    public static Vector operator +(Vector a, Vector b)
    {
        Vector max = Max(a,b);
        Vector min = Min(a,b);
        Vector result = max.copy;
        for (int i = 0; i < min.dims; i++)
        {
            result[i] += min[i];
        }
        return result;
    }
    public static Vector operator -(Vector a, Vector b)
    {
        Vector max = Max(a, b);
        Vector min = Min(a, b);
        Vector result = max.copy;
        for (int i = 0; i < min.dims; i++)
        {
            result[i] -= min[i];
        }
        return result;
    }
    public static Vector operator *(Vector a, Vector b)
    {
        Vector max = Max(a, b);
        Vector min = Min(a, b);
        Vector result = max.copy;
        for (int i = 0; i < min.dims; i++)
        {
            result[i] *= min[i];
        }
        return result;
    }
    public static Vector operator /(Vector a, Vector b)
    {
        Vector max = Max(a, b);
        Vector min = Min(a, b);
        Vector result = max.copy;
        for (int i = 0; i < min.dims; i++)
        {
            result[i] /= min[i];
        }
        return result;
    }
    public static Vector operator *(Vector a, float b)
    {
        Vector result = a.copy;
        for (int i = 0; i < result.dims; i++)
        {
            result[i] *= b;
        }
        return result;
    }
    public static Vector operator /(Vector a, float b)
    {
        Vector result = a.copy;
        for (int i = 0; i < result.dims; i++)
        {
            result[i] /= b;
        }
        return result;
    }
    public float lenght => __len();
    private float __len()
    {
        float result = 0;
        foreach (var coord in coords)
        {
            result += Mathf.Pow(coord, 2);
        }
        return Mathf.Sqrt(result);
    }
    private Vector __normalized()
    {
        Vector result = copy;
        float len = lenght;
        for (int i = 0;i < dims;i++)
        {
            result /= len;
        }
        return result;
    }
    public Vector norm => __normalized();
    private Vector3 __ToVec3()
    {
        Vector3 result = Vector3.zero;
        if (dims >= 3)
        {
            result.z = this[2];
        }
        if (dims >= 2)
        {
            result.y = this[1];
        }
        if (dims >= 1)
        {
            result.x = this[0];
        }
        return result;
    }
    public Vector3 vec3 => __ToVec3();
    public void zero()
    {
        for (int i = 0; i < dims; i++)
            coords[i] = 0;
    }
}
