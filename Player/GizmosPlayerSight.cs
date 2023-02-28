using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosPlayerSight : MonoBehaviour
{
    /*private enum form
    {
        cube,
        sphere
    };
    [System.Serializable]
    public class stepPoint
    {
        private Vector3 pos;
        [SerializeField] private form forma = form.cube;
        [SerializeField, Range(0f, 50f)] private float distancia = 1.0f;
        [SerializeField, Range(0f, 5f)] private float size = 1.0f;
        [SerializeField] private Color color = Color.white;
        private GameObject father;

        public void Draw()
        {
            pos = Vector3.forward * distancia;
            Gizmos.color = color;
            if (forma == form.cube)
                Gizmos.DrawCube(pos, new Vector3(size, size, size));
            else if (forma == form.sphere)
                Gizmos.DrawSphere(pos, size);
        }
        public void Draw(GameObject go)
        {
            go = father;
            pos = go.transform.position + Vector3.forward * distancia;
            Gizmos.color = color;
            if (forma == form.cube)
                Gizmos.DrawCube(pos, new Vector3(size, size, size));
            else if (forma == form.sphere)
                Gizmos.DrawSphere(pos, size);
        }
        public void Draw(Vector3 position)
        {
            pos = position * distancia;
            Gizmos.color = color;
            if (forma == form.cube)
                Gizmos.DrawCube(pos, new Vector3(size, size, size));
            else if (forma == form.sphere)
                Gizmos.DrawSphere(pos, size);
        }
        public void setPos(Vector3 position) { pos = position; }
        public void resetPos() { pos = father.transform.forward * distancia; }
        public void setSize(float siz) { size = siz; }
    }*/

    [Header("Camara")]
    public GameObject cam;
    private GameObject lookingItem = null;
    [ReadOnly] public Vector3 GizCast;
    private Vector3 camPos;

    [Header("Puntos")]
    //public List<stepPoint> puntillos = new List<stepPoint>();

    [Range(1f, 50f)] public float rangeRaycast = 20.0f;
    [Range(0f, 5f)] public float sizeRaycast = 1.0f;
    public Color colorRaycast = Color.white;

    [Space(20)]
    [Range(1f, 6f)] public float rangePickup = 1.0f;
    [Range(0f, 5f)] public float sizePickup = 1.0f;
    [ReadOnly] public float sizePi = 0.4f;
    public Color colorPickup = Color.white;

    [Space(20)]
    [Range(1f, 20f)] public float rangeMaquinas = 1.0f;
    [Range(0f, 5f)] public float sizeMaquinas = 1.0f;
    [ReadOnly] public float sizeMa = 0.4f;
    public Color colorMaquinas = Color.white;

    [Space(20)]
    [Range(1f, 20f)] public float rangeBuild = 1.0f;
    [Range(0f, 5f)] public float sizeBuild = 1.0f;
    [ReadOnly] public float sizeBu = 0.4f;
    public Color colorBuild = Color.white;

    void Start()
    {
        //camPos = cam.transform.localPosition;
        camPos = Vector3.zero;

        var pos = gameObject.transform.position + camPos;
        var looking = gameObject.transform.forward;
        GizCast = pos + looking * rangeRaycast;
        //foreach (stepPoint p in puntillos) p.Draw(gameObject);
        /*foreach (stepPoint p in puntillos)
        {
            if (cam != null) p.Draw(cam);
        }*/
        sizePi = 0f;
        sizeMa = 0f;
        sizeBu = 0f;
}

    void Update()
    {
        var pos = gameObject.transform.position + camPos;
        var looking = gameObject.transform.forward;
        Ray rayoG = new Ray(pos, looking);
        RaycastHit hitG;
        if (Physics.Raycast(rayoG, out hitG, rangeRaycast))
        {
            GizCast = hitG.point;
            //
            lookingItem = hitG.collider.gameObject;
            float dist = Vector3.Distance(transform.position, lookingItem.transform.position);
            //float dist = Vector3.Distance(transform.position, hitG.point);
            if (lookingItem.CompareTag("Env"))
            {
                sizeRaycast = 0.2f;
                sizePi = 0.0f;
                sizeMa = 0.0f;
                sizeBu = 0.0f;
            }
            else if (lookingItem.CompareTag("ItemPickup") && dist <= rangePickup)
            {
                sizeRaycast = 0.0f;
                sizePi = 0.2f;
                sizeMa = 0.0f;
                sizeBu = 0.0f;
            }
            else if (lookingItem.CompareTag("Maquina") && dist <= rangeMaquinas)
            {
                sizeRaycast = 0.0f;
                sizePi = 0.0f;
                sizeMa = 0.2f;
                sizeBu = 0.0f;
            }
            else if (lookingItem.CompareTag("Building") && dist <= rangeBuild)
            {
                sizeRaycast = 0.0f;
                sizePi = 0.0f;
                sizeMa = 0.0f;
                sizeBu = 0.2f;
            }
            else
            {
                sizeRaycast = 0.2f;
                sizePi = 0.0f;
                sizeMa = 0.0f;
                sizeBu = 0.0f;
            }
        }
        else
        {
            sizeRaycast = 0.2f;
            sizePi = 0.0f;
            sizeMa = 0.0f;
            sizeBu = 0.0f;
            GizCast = pos + looking * rangeRaycast;
            lookingItem = null;
        }
    }

    void OnDrawGizmos()
    {
        var pos = gameObject.transform.position;
        var looking = gameObject.transform.forward;
        //GizCast = pos + looking * rangeRaycast;

        Gizmos.color = colorRaycast;
        Gizmos.DrawLine(pos, pos + looking * rangeRaycast);
        Gizmos.DrawWireSphere(GizCast, sizeRaycast);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GizCast, radius: 0.03f);

        Gizmos.color = colorPickup;
        Gizmos.DrawWireCube(pos + looking * rangePickup, new Vector3(sizePickup, sizePickup, sizePickup));
        Gizmos.DrawWireSphere(GizCast, sizePi);

        Gizmos.color = colorMaquinas;
        Gizmos.DrawWireCube(pos + looking * rangeMaquinas, new Vector3(sizeMaquinas, sizeMaquinas, sizeMaquinas));
        Gizmos.DrawWireSphere(GizCast, sizeMa);

        Gizmos.color = colorBuild;
        Gizmos.DrawWireCube(pos + looking * rangeBuild, new Vector3(sizeBuild, sizeBuild, sizeBuild));
        Gizmos.DrawWireSphere(GizCast, sizeBu);
    }
}
