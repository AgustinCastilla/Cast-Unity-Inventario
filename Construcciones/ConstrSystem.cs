using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrSystem : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Distancia de espacio sobre sistema relativo.")]
    [SerializeField] public float relativeSizeXZ;
    [SerializeField] public float relativeSizeY;

    [Header("Variables")]
    /*public int[] buildCount = 
        new int[System.Enum.GetValues(typeof(ConstrData)).Length + 1*/
    public int buildCount;

    public int Signo(float value)
    {
        int ret;
        if (value >= 0) ret = 1;
        else ret = -1;
        return ret;
    }
    public Vector3 HalfV3(Vector3 value)
    {
        return new Vector3(value.x / 2f, value.y / 2f, value.z / 2f);
    }
    public Vector3 FloorV3(Vector3 value)
    {
        return new Vector3(Mathf.Floor(value.x), Mathf.Floor(value.y), Mathf.Floor(value.z));
    }
    public bool InsideV3(Vector3 point, Vector3 minimum, Vector3 maximum, bool reorder)
    {
        Vector3 min, max;
        if (reorder) { min = Min(minimum, maximum); max = Max(minimum, maximum); }
        else { min = minimum; max = maximum; }
        return ((min.x <= point.x && point.x <= max.x) &&
            (min.y <= point.y && point.y <= max.y) &&
            (min.z <= point.z && point.z <= max.z));
    }
    public Vector3 Min(Vector3 value1, Vector3 value2)
    {
        Vector3 ret = new Vector3(0, 0, 0);
        ret.x = Mathf.Min(value1.x, value2.x);
        ret.y = Mathf.Min(value1.y, value2.y);
        ret.z = Mathf.Min(value1.z, value2.z);
        return ret;
    }
    public Vector3 Max(Vector3 value1, Vector3 value2)
    {
        Vector3 ret = new Vector3(0, 0, 0);
        ret.x = Mathf.Max(value1.x, value2.x);
        ret.y = Mathf.Max(value1.y, value2.y);
        ret.z = Mathf.Max(value1.z, value2.z);
        return ret;
    }
    /*public Vector3 SystemToV3(GameObject building, buildSize dimension, Vector3 side)
    {
        Vector3 temp = new Vector3((int)dimension.size.x / 2f, (int)dimension.size.y / 2f, (int)dimension.size.z / 2f);
        Vector3 ret = Vector3.Scale(temp, side);
        if (dimension.parX) ret.x += 0.5f;
        if (dimension.parY) ret.y += 0.5f;
        if (dimension.parZ) ret.z += 0.5f;
        ret *= relativeSize;
        return ret;
    }
    public Vector3 SystemToV3(GameObject building, buildSize dimension, Vector3Int side)
    {
        Vector3 temp = new Vector3((int)dimension.size.x / 2f, (int)dimension.size.y / 2f, (int)dimension.size.z / 2f);
        Vector3 ret = Vector3.Scale(temp, side);
        if (dimension.parX) ret.x += 0.5f;
        if (dimension.parY) ret.y += 0.5f;
        if (dimension.parZ) ret.z += 0.5f;
        ret *= relativeSize;
        return ret;
    }
    public Vector3 SystemToV3(GameObject building, buildSize dimension, Vector3Int side, Vector3 offset)
    {
        Vector3 temp = FloorV3(HalfV3(side));
        Debug.Log("A: " + temp);
        Vector3 ret = Vector3.Scale(temp, side);
        Debug.Log("B: " + ret);
        if (dimension.parX) ret.x += 0.5f * Signo(side.x);
        if (dimension.parY) ret.y += 0.5f * Signo(side.y);
        if (dimension.parZ) ret.z += 0.5f * Signo(side.z);
        Debug.Log("C: " + ret);
        ret += offset;
        Debug.Log("D: " + ret + " * " + relativeSize);
        ret *= relativeSize;
        Debug.Log("E: " + ret);
        ret += building.gameObject.transform.position;
        return ret;
    }*/
    public Vector3 SystemToV3(GameObject building, Vector3 side)
    {
        Vector3 ret = side;
        ret.x *= relativeSizeXZ;
        ret.y *= relativeSizeY;
        ret.z *= relativeSizeXZ;
        ret += building.gameObject.transform.position;
        return ret;
    }
    public Vector3 SystemToV3(GameObject building, Vector3Int side)
    {
        Vector3 ret = side;
        ret.x *= relativeSizeXZ;
        ret.y *= relativeSizeY;
        ret.z *= relativeSizeXZ;
        ret += building.gameObject.transform.position;
        return ret;
    }
    public Vector3 SystemToV3(GameObject building, Vector3Int side, Vector3 offset)
    {
        Vector3 ret = side;
        ret += offset;
        ret.x *= relativeSizeXZ;
        ret.y *= relativeSizeY;
        ret.z *= relativeSizeXZ;
        ret += building.gameObject.transform.position;
        return ret;
    }
    public Vector3 RotateCoords(Vector3 input, int cantidad)
    {
        Vector3 ret = input;
        if(cantidad >= 0)
        {
            for (int i = 0; i < cantidad; i++) ret = new Vector3(ret.z, ret.y, -ret.x);
        }
        else
        {
            for (int i = 0; i < cantidad; i++) ret = new Vector3(-ret.z, ret.y, ret.x);
        }
        return ret;
    }
    public Vector3Int RotateCoords(Vector3Int input, int cantidad)
    {
        Vector3Int ret = input;
        if (cantidad >= 0)
        {
            for (int i = 0; i < cantidad; i++) ret = new Vector3Int(ret.z, ret.y, -ret.x);
        }
        else
        {
            for (int i = 0; i < cantidad; i++) ret = new Vector3Int(-ret.z, ret.y, ret.x);
        }
        return ret;
    }
    public SnapPoint RotateSnap(SnapPoint point, int cantidad)
    {
        SnapPoint ret = point;
        ret.min = RotateCoords(ret.min, cantidad);
        ret.max = RotateCoords(ret.max, cantidad);
        ret.snap = RotateCoords(ret.snap, cantidad);
        ret.face = RotateCoords(ret.face, cantidad);
        return ret;
    }
    public Vector3 GetOffsetCoords(SnapPoint snapPoint)
    {
        return (snapPoint.snap - Vector3.Normalize(snapPoint.snap - snapPoint.face));
    }
}
