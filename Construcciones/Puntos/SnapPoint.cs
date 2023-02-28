using UnityEngine;

[System.Serializable]
public class SnapPoint
{
    public string comentario = "Sin comment";
    public Vector3 max;
    public Vector3 min;
    public Vector3Int snap;
    public Vector3 face;

    public SnapPoint(SnapPoint original)
    {
        comentario = original.comentario;
        max = original.max;
        min = original.min;
        snap = original.snap;
        face = original.face;
    }
}
