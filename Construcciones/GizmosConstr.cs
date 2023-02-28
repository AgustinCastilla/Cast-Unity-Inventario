using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Constr))]
public class GizmosConstr : MonoBehaviour
{
    Constr buildData;

    public bool draw = false;
    public float entranceSize = 0.1f;
    public float arrowMulti = 0.8f;

    private float hue = 0f;

    private void OnDrawGizmos()
    {
        if (draw)
        {
            buildData = GetComponent<Constr>();
            ConstrSystem BS = GameObject.FindGameObjectWithTag("GlobalBuildSystem").GetComponent<ConstrSystem>();
            //
            if (buildData.snapData != null && buildData.snapData.puntos.Length > 0)
            {
                foreach (SnapPoint p in buildData.snapData.puntos)
                {
                    int rot = buildData.RotateTimes();
                    Vector3 tempMIN = BS.RotateCoords(p.min, rot);
                    Vector3 tempMAX = BS.RotateCoords(p.max, rot);
                    Vector3 mi = BS.Min(tempMIN, tempMAX);
                    Vector3 ma = BS.Max(tempMIN, tempMAX);
                    mi = BS.SystemToV3(gameObject, mi);
                    ma = BS.SystemToV3(gameObject, ma);

                    Vector3 snap = BS.RotateCoords(p.snap, rot);
                    snap = BS.SystemToV3(gameObject, snap);

                    Vector3 face = BS.RotateCoords(p.face, rot);
                    face = BS.SystemToV3(gameObject, face);
                    ///Gizmos.color = Color.HSVToRGB(hue, 1, 1);

                    //Draw all 4 lines from xMin to xMax
                    Gizmos.color = ColorCheck(mi.x, ma.x, p.comentario);
                    Gizmos.DrawLine(new Vector3(mi.x, mi.y, mi.z), new Vector3(ma.x, mi.y, mi.z));
                    Gizmos.DrawLine(new Vector3(mi.x, mi.y, ma.z), new Vector3(ma.x, mi.y, ma.z));
                    Gizmos.DrawLine(new Vector3(mi.x, ma.y, mi.z), new Vector3(ma.x, ma.y, mi.z));
                    Gizmos.DrawLine(new Vector3(mi.x, ma.y, ma.z), new Vector3(ma.x, ma.y, ma.z));

                    //Draw all 4 lines from yMin to yMax
                    Gizmos.color = ColorCheck(mi.y, ma.y, p.comentario);
                    Gizmos.DrawLine(new Vector3(mi.x, mi.y, mi.z), new Vector3(mi.x, ma.y, mi.z));
                    Gizmos.DrawLine(new Vector3(mi.x, mi.y, ma.z), new Vector3(mi.x, ma.y, ma.z));
                    Gizmos.DrawLine(new Vector3(ma.x, mi.y, mi.z), new Vector3(ma.x, ma.y, mi.z));
                    Gizmos.DrawLine(new Vector3(ma.x, mi.y, ma.z), new Vector3(ma.x, ma.y, ma.z));

                    //Draw all 4 lines from zMin to zMax
                    Gizmos.color = ColorCheck(mi.z, ma.z, p.comentario);
                    Gizmos.DrawLine(new Vector3(mi.x, mi.y, mi.z), new Vector3(mi.x, mi.y, ma.z));
                    Gizmos.DrawLine(new Vector3(mi.x, ma.y, mi.z), new Vector3(mi.x, ma.y, ma.z));
                    Gizmos.DrawLine(new Vector3(ma.x, mi.y, mi.z), new Vector3(ma.x, mi.y, ma.z));
                    Gizmos.DrawLine(new Vector3(ma.x, ma.y, mi.z), new Vector3(ma.x, ma.y, ma.z));

                    Gizmos.DrawCube(snap, new Vector3(entranceSize, entranceSize, entranceSize));
                    Gizmos.DrawSphere(face, entranceSize);
                    //Gizmos.DrawLine(BS.SystemToV3(buildData.gameObject, Vector3.zero), BS.SystemToV3(buildData.gameObject, p.facing * arrowMulti));
                    //Gizmos.DrawSphere(BS.SystemToV3(buildData.gameObject, p.facing * arrowMulti), radius: entranceSize / 2);
                    Vector3 temp = snap - face;
                    Gizmos.DrawLine(face, face + temp);

                    hue += 0.05f;
                    //if(hue > 1f) { hue = 0f; }
                }
            }
        }
        //
        hue = 0f;
    }

    private Color ColorCheck(float menor, float mayor, string name)
    {
        if (menor > mayor) return Color.black;
        else if (menor == mayor) return Color.grey;
        else return Color.HSVToRGB(hue, 1, 1);
    }
}