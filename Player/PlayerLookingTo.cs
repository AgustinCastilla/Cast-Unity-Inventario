using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookingTo : MonoBehaviour
{
    public GameObject Camara = null;
    private Vector3 camPos;

    // LayerMask[] ignoreLayerMul;
    public LayerMask ignoreLayer; //private LayerMask ignoreLayer;

    [ReadOnly] public Vector3 lookCoords;

    [Range(1f, 50f), Rename("Rango: Raycast"), Tooltip("Rango del raycast.")]
    public float rangeRaycast = 20.0f;
    [Range(1f, 6f), Rename("Rango: Agarrar items"), Tooltip("Distancia desde donde se puede agarrar items.")]
    public float rangePickup = 1.0f;
    [Range(1f, 20f), Rename("Rango: Usar maquinas"), Tooltip("Distancia desde donde se puede acceder a la interfaz de las maquinas.")]
    public float rangeMaquinas = 1.0f;
    [Range(1f, 20f), Rename("Rango: Construir"), Tooltip("Distancia desde donde se puede snappear estructuras.")]
    public float rangeBuild = 1.0f;

    [Tooltip("Tomar la distancia desde el centro del objeto (false para tomar distancia desde el raycast hit).")]
    public bool toCenter = false;

    private GameObject looking_GameObject = null;       // Looking GameObject
    private looktype type = looktype.nothing;

    private ItemsInterface looking_ItemInter = null;    // Looking "itemPickup"
    private GameObject looking_Maquina = null;          // Looking "Maquina"
    private Constr looking_Building = null;           // Looking "Building"
    private SnapPoint snapPoint = null;             // Snap coords para building

    private bool buildingPreviewRotated = false;

    void Start()
    {
        /*for(int i = 0; i < ignoreLayerMul.Length; i ++)
        {
            ignoreLayer &= ignoreLayerMul[i];
        }*/
        camPos = Camara.transform.localPosition;
    }

    void Update()
    {
        var pos = gameObject.transform.position + (camPos.x * transform.right) + (camPos.y * transform.up) + (camPos.z * transform.forward);
        var looking = gameObject.transform.forward;

        Ray rayo = new Ray(pos, looking);
        RaycastHit hit;
        if (Physics.Raycast(rayo, out hit, rangeRaycast, ~ignoreLayer))
        {
            looking_GameObject = hit.collider.gameObject;
            lookCoords = hit.point;

            float dist;
            if (toCenter) dist = Vector3.Distance(transform.position, looking_GameObject.transform.position);
            else dist = Vector3.Distance(transform.position, hit.point);

            if (looking_GameObject.CompareTag("Env"))
            {
                type = looktype.environment;
                looking_ItemInter = null;
                looking_Maquina = null;
                looking_Building = null;
                snapPoint = null;
            }
            else if (looking_GameObject.CompareTag("ItemPickup") && dist <= rangePickup)
            {
                type = looktype.itemPickup;
                looking_ItemInter = looking_GameObject.GetComponent<ItemsInterface>();
                looking_Maquina = null;
                looking_Building = null;
                snapPoint = null;
            }
            else if (looking_GameObject.CompareTag("Maquina") && dist <= rangeMaquinas)
            {
                type = looktype.maquina;
                looking_ItemInter = null;
                looking_Maquina = null; //THIS
                looking_Building = null;
                snapPoint = null;
            }
            else if (looking_GameObject.CompareTag("Building") && dist <= rangeBuild)
            {
                type = looktype.building;
                looking_ItemInter = null;
                looking_Maquina = null;
                looking_Building = null;
                snapPoint = null;
                looking_Building = looking_GameObject.GetComponent<Constr>();

                snapPoint = looking_Building.InsideSomeSnap(hit.point);
                //if (!buildingPreviewRotated) snapPoint = looking_Building.Inside(hit.point, new Vector3(0, 0, 0));
                //else snapPoint = looking_Building.Inside(hit.point, new Vector3(0, 90, 0));
            }
            else
            {
                type = looktype.nothing;
                looking_ItemInter = null;
                looking_Maquina = null;
                looking_Building = null;
                snapPoint = null;
            }
        }
        else
        {
            looking_GameObject = null;
            /*type = looktype.nothing;
            looking_ItemInter = null;
            looking_Maquina = null;
            looking_Building = null;
            snapPoint = Vector3.zero;*/
        }
    }

    public looktype GetSightType() { return type; }
    public GameObject GetSightGameObject() { return looking_GameObject; }
    public ItemsInterface GetSightItemsInterface() {
        if (looking_GameObject != null) return looking_ItemInter;
        else return null;
    }
    public GameObject GetSightMaquina ()
    {
        if (looking_GameObject != null) return looking_Maquina;
        else return null;
    }
    public Constr GetSightBuilding()
    {
        if (looking_GameObject != null) return looking_Building;
        else return null;
    }
    public SnapPoint GetSightBuildingSnapPoint()
    {
        if (looking_GameObject != null && looking_Building != null) return snapPoint;
        else return null;
    }

    public void SetPreviewRotation(bool status) { buildingPreviewRotated = status; }
    public void TogglePreviewRotation() { buildingPreviewRotated = !buildingPreviewRotated; }
}
