using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constr : MonoBehaviour
{
    public bool[] buildedStatus;
    public GameObject[] buildedGameObject;
    public Constr[] buildedSide;

    public SnapPointGroup snapData;

    public const float OverlapRadius = 0.05f;
    public const float OverlapDistance = 0.01f;

    ConstrSystem BS = null;
    private void Awake()
    {
        BS = GameObject.FindGameObjectWithTag("GlobalBuildSystem").GetComponent<ConstrSystem>();
    }

    private void Start()
    {
        int temp_size = snapData.puntos.Length;
        buildedStatus = new bool[temp_size];
        buildedGameObject = new GameObject[temp_size];
        buildedSide = new Constr[temp_size];
    }

    private int snapPointIndex(SnapPoint point)
    {
        int ret = -1;
        for (int i = 0; i < snapData.puntos.Length; i ++)
        {
            if (snapData.puntos[i] == point) { ret = i; }
        }
        return ret;
    }

    public int RotateTimes()
    {
        return (int)(Mathf.Round(gameObject.transform.rotation.eulerAngles.y) / 90f);
    }

    public void Check(GameObject invoker)
    {
        foreach (SnapPoint p in snapData.puntos)
        {
            if (!IsPointBuilded(p))
            {
                Vector3 temp = BS.SystemToV3(gameObject, BS.RotateCoords(p.snap, RotateTimes()));
                Collider[] col = Physics.OverlapSphere(temp, radius: OverlapRadius);
                for (int i = 0; i < col.Length; i++)
                {
                    if (col[i].CompareTag("Building"))
                    {
                        Constr tempBuild = col[i].GetComponent<Constr>();
                        if (tempBuild != null)
                        {
                            foreach (SnapPoint sp in tempBuild.snapData.puntos)
                            {
                                if (DoorToDoorDist(p, tempBuild, sp) <= OverlapDistance)
                                {
                                    BuildPoint(p, col[i].gameObject, tempBuild);
                                    if (col[i].gameObject != invoker) { tempBuild.Check(gameObject); }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public float DoorToDoorDist(SnapPoint mySnap, Constr building, SnapPoint point)
    {
        Vector3 tempMySnap = BS.RotateCoords(mySnap.face, RotateTimes());
        tempMySnap = BS.SystemToV3(gameObject, tempMySnap);
        Vector3 tempPoint = BS.RotateCoords(point.face, building.RotateTimes());
        tempPoint = BS.SystemToV3(building.gameObject, tempPoint);
        return Vector3.Distance(tempMySnap, tempPoint);
    }
    public bool BuildPoint(SnapPoint point, GameObject newGameObject, Constr newBuilding)
    {
        bool ret = false;
        int index = snapPointIndex(point);
        if (index != -1)
        {
            buildedStatus[index] = true;
            buildedGameObject[index] = newGameObject;
            buildedSide[index] = newBuilding;
            ret = true;
        }
        return ret;
    }
    public bool IsPointBuilded(SnapPoint point)
    {
        bool ret = false;
        int index = snapPointIndex(point);
        if (index != -1)
        {
            if (buildedStatus == null) { Debug.LogWarning("buildedStatus es null!"); }
            else ret = buildedStatus[index];
        }
        return ret;
    }
    public bool InsideSnap(Vector3 point, SnapPoint snap, GameObject reference)
    {
        Vector3 tempMin = BS.RotateCoords(snap.min, RotateTimes());
        tempMin = BS.SystemToV3(gameObject, tempMin);
        Vector3 tempMax = BS.RotateCoords(snap.max, RotateTimes());
        tempMax = BS.SystemToV3(gameObject, tempMax);
        return BS.InsideV3(point, tempMin, tempMax, true);
    }
    public bool InsideSnap(Vector3 point, SnapPoint snap)
    {
        return InsideSnap(point, snap, gameObject);
    }
    public SnapPoint InsideSomeSnap(Vector3 point)
    {
        SnapPoint ret = null;
        foreach (SnapPoint p in snapData.puntos)
        {
            if (InsideSnap(point, p)) { ret = p; break; }
        }
        return ret;
    }
    public Vector3 SnapPointOrientation(SnapPoint point)
    {
        return BS.RotateCoords(point.snap - point.face, RotateTimes());
    }
    public Vector3 SnapPointOrientation(SnapPoint point, bool normalize)
    {
        Vector3 ret = BS.RotateCoords(point.snap - point.face, RotateTimes());
        if (normalize) ret *= 2;//Vector3.Normalize(ret);
        return ret;
    }
}