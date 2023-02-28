using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerLookingTo), typeof(PlayerInventory))]
public class PlayerMain : MonoBehaviour
{
    /// <summary>
    /// Hacer un sistema de ItemsIndex donde use Resources.LoadAll().
    /// Y se pueda acceder a los valores de los items (ScriptableObjects).
    /// Así el PlayerInventory.cs sólo guarda los itemIDs.
    /// Y agrupar todas las variables slot-related en una clase.
    /// 
    /// Hacer que en los "BuildPosition" o en alguno tenga la orientación normal (+-1, 0, 0); (0, +-1, 0); (0, 0, +-1).
    /// Para snapear deben tener orientación igual pero de signo cambiado. Autorotado hasta hacer match.
    /// (Contar rotaciones para no intentar hacer match forever).
    /// 
    /// Hacer listas de las 4 rotaciones para multiples puntos.
    /// Se tendría en la clase: Rotacion, Normal, y una lista de snaps.
    /// 
    /// Al hacer el preview, los offsets de la lista se pasan a la función SystemToV3.
    /// Hacer a futuro arrays de BuildingPreview por categorías (Pasillo, habitación, paredes/marcos/puertas/ventanas, etc).
    /// Van a estar todas las posibles combinaciones de paredes/ventanas/etc en cada bloque de las construcciones.
    /// ¿O lo inicializo como hijo? Creo que sería mejor. Después de todo no se hace tanto
    /// 
    /// Revisar la construcción, que el detectar la zona para el snap anda medio mal.
    /// </summary>

    //[Header("Scripts")]
    private PlayerLookingTo script_looking = null;
    private PlayerInventory script_inventory = null;
    private PlayerBuilding script_building = null;

    [Header("Canvas")]
    [Rename("Item Info Label"), Tooltip("Image del cuadro de información de item objetivo.")]
    public GameObject ItemInfoShow = null;
    private string ItemInfoShowText = null;

    // -----------------------------------------

    private GameObject look_GameObject = null;              // Looking GameObject
    private looktype look_type = looktype.nothing;
    private ItemsInterface look_Item = null;                // Looking "itemPickup"
    private GameObject look_Maquina = null;                 // Looking "Maquina"
    private Constr look_Building = null;                  // Looking "Building"
    private SnapPoint look_BuildingSnapPoint = null;        // Snap coords para building
    private SnapPoint look_BuildingSnapPoint_LAST = null;

    [Header("Building")]
    public Constr[] Buildings;
    //[Rename("Preview Prefab: Pared"), Tooltip("Prefab del GameObject preview para la pared.")]
    //public GameObject buildingWallPrefab = null;            //Prefab de la pared
    //private GameObject buildingWallPrefab;

    // -----------------------------------------

    ConstrSystem BS = null;
    private void Awake()
    {
        BS = GameObject.FindGameObjectWithTag("GlobalBuildSystem").GetComponent<ConstrSystem>();
    }

    void Start()
    {
        // ---------------------------------------- GET SCRIPTS
        script_looking = GetComponent<PlayerLookingTo>();
        script_inventory = GetComponent<PlayerInventory>();
        script_building = GetComponent<PlayerBuilding>();
        if (ItemInfoShow != null) { ItemInfoShowText = ItemInfoShowText = ItemInfoShow.GetComponentInChildren<Text>().text; }
        //buildingWallPrefab = Buildings[0].prefab;
        //selectedBuilding = Buildings[0];
    }

    void Update()
    {
        look_GameObject = script_looking.GetSightGameObject();
        look_type = script_looking.GetSightType();
        look_Item = script_looking.GetSightItemsInterface();
        look_Maquina = script_looking.GetSightMaquina();
        look_Building = script_looking.GetSightBuilding();
        look_BuildingSnapPoint = script_looking.GetSightBuildingSnapPoint();

        // ************************************************************************
        // ************************************************************************

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int iS = script_inventory.GetSlot() - (int)scroll;
            script_inventory.SetSlot(iS);
        }
        // ----------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha1)) { script_inventory.SetSlot(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { script_inventory.SetSlot(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { script_inventory.SetSlot(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { script_inventory.SetSlot(3); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { script_inventory.SetSlot(4); }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { script_inventory.SetSlot(5); }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { script_inventory.SetSlot(6); }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { script_inventory.SetSlot(7); }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) { script_inventory.SetSlot(8); }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) { script_inventory.SetSlot(9); }

        // ************************************************************************
        // ************************************************************************

        if (Input.GetKeyDown(KeyCode.I)) { script_inventory.ToggleFullInventory(); }

        // ************************************************************************
        // ************************************************************************

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool dropAll = false;
            if (Input.GetKey(KeyCode.LeftControl)) { dropAll = true; }
            script_inventory.DropItem(dropAll);
        }

        // ************************************************************************
        // ************************************************************************

        if (Input.GetKeyDown(KeyCode.R))
        {
            script_building.WhenRotate();
        }

        // ************************************************************************
        // ************************************************************************

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (look_type == looktype.itemPickup && look_Item != null)
            {
                ItemData itemdata = look_Item.itemData;
                int cant = look_Item.cantidad;
                int ret = script_inventory.AddItem(itemdata, cant);
                int restantes = cant - ret;
                //if (ret < 0) Debug.Log("Error trying to pickup item.");
                //if (ret >= 0) Debug.Log("Pickup " + ret + " of " + itemdata.itemName + " - " + restantes + " items left on ground");
                look_Item.onPickup(ret);
            }
        }

        // ************************************************************************
        // ************************************************************************
        if (look_GameObject != null)
        {
            if (look_type == looktype.itemPickup && look_Item != null)
            {
                ItemInfoShow.SetActive(true);
                ItemInfoShowText = look_Item.itemData.itemName;
            }
            else ItemInfoShow.SetActive(false);

            if (look_type == looktype.building && look_BuildingSnapPoint != null)
            {
                if (!look_Building.IsPointBuilded(look_BuildingSnapPoint))
                {
                    script_building.SnapPreview(look_GameObject, look_Building, look_BuildingSnapPoint);
                    // -------------------------------------------------------------------------
                    if (Input.GetMouseButtonDown(0))
                    {
                        script_building.CrearEdificio(look_GameObject.transform.parent);
                    }
                }
                else script_building.HidePreview();
            }
            else script_building.HidePreview();
        }
        else script_building.HidePreview();

        look_BuildingSnapPoint_LAST = look_BuildingSnapPoint;
    }
}