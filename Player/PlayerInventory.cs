using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    /// <summary>
    /// Hacer un sistema de ItemsIndex donde use Resources.LoadAll().
    /// Y se pueda acceder a los valores de los items (ScriptableObjects).
    /// Así el PlayerInventory.cs sólo guarda los itemIDs.
    /// Y agrupar todas las variables slot-related en una clase.
    /// </summary>

    [Header("Canvas")]
    [Rename("Fast Inventory"), Tooltip("Image del inventario rápido.")]
    public Image InvBar = null;
    [Rename("Full Inventory"), Tooltip("Image del inventario completo.")]
    public Image InvFull = null;
    [Rename("Inventory Arrow"), Tooltip("Image de la flecha roja apuntando al item seleccionado.")]
    public Image InvSelect = null;
    [Rename("Fast Inv. Parent"), Tooltip("GameObject padre de los íconos de items del inventario rápido.")]
    public GameObject iconParentBar;
    [Rename("Full Inv. Parent"), Tooltip("GameObject padre de los íconos de items del inventario completo.")]
    public GameObject iconParentFull;

    // -----------------------------------------

    // [Header("Inventory")]
    private const int sizeBar = 10;
    private const int sizeFull = sizeBar * 3;
    private ItemData[] UserItemsBar = new ItemData[sizeBar];
    private ItemData[] UserItemsFull = new ItemData[sizeFull];
    private Image[] ItemIconBar = new Image[sizeBar];
    private Image[] ItemIconFull = new Image[sizeFull];
    private int[] ItemCantBar = new int[sizeBar];
    private int[] ItemCantFull = new int[sizeFull];
    

    private Vector2 SelectOriginalPos;
    private int itemSelected = 0;
    private bool OpenInvBar = false;
    private bool OpenInvFull = false;

    // -----------------------------------------

    private const float iconWidth = 74f, iconHeight = 73f;
    private const float barX = -383f, barY = 100f;
    private const float fullX = -383f, fullY = 84f;
    private const float iconOffsetX = 85f, iconOffsetY = 83f;

    // -----------------------------------------

    void Start()
    {
        SelectOriginalPos = new Vector2(InvSelect.rectTransform.anchoredPosition.x,
            InvSelect.rectTransform.anchoredPosition.y);
        InvFull.enabled = false;

        for (int i = 0; i < sizeBar; i ++)
        {
            GameObject NewObj = new GameObject("Item Icon Bar " + i);  //Create the GameObject
            ItemIconBar[i] = NewObj.AddComponent<Image>(); //Add the Image Component script
            NewObj.GetComponent<RectTransform>().SetParent(iconParentBar.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            NewObj.SetActive(true); //Activate the GameObject

            ItemIconBar[i].rectTransform.anchorMin = new Vector2(0.5f, 0.5f/*0f*/);
            ItemIconBar[i].rectTransform.anchorMax = new Vector2(0.5f, 0.5f/*0f*/);
            ItemIconBar[i].rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ItemIconBar[i].rectTransform.sizeDelta = new Vector2(iconWidth, iconHeight);
            ItemIconBar[i].rectTransform.anchoredPosition = new Vector2(barX + (i * iconOffsetX), barY);
            ItemIconBar[i].enabled = false;
        }
        for (int i = 0; i < (sizeFull/sizeBar); i++)
        {
            for (int b = 0; b < sizeBar; b ++)
            {
                int t = b + i * sizeBar;

                GameObject NewObj = new GameObject("Item Icon Full " + t);  //Create the GameObject
                ItemIconFull[t] = NewObj.AddComponent<Image>(); //Add the Image Component script
                NewObj.GetComponent<RectTransform>().SetParent(iconParentFull.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
                NewObj.SetActive(true); //Activate the GameObject

                ItemIconFull[t].rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                ItemIconFull[t].rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                ItemIconFull[t].rectTransform.pivot = new Vector2(0.5f, 0.5f);
                ItemIconFull[t].rectTransform.sizeDelta = new Vector2(iconWidth, iconHeight);
                //ItemIconFull[t].rectTransform.anchoredPosition = new Vector2(barX + (b * iconOffsetX), barY + (i * iconOffsetY));
                ItemIconFull[t].rectTransform.anchoredPosition = new Vector2(barX + (b * iconOffsetX), barY + ((2-i) * iconOffsetY));
                ItemIconFull[t].enabled = false;
            }
        }
        iconParentFull.SetActive(false);
    }

    public int GetSlot() { return itemSelected; }

    public void SetSlot(int number)
    {
        number = Mathf.Clamp(number, 0, 9);
        itemSelected = number;
        UpdateSelect();
    }

    public void IncreaseSlot() { SetSlot(++itemSelected); }
    public void DecreaseSlot() { SetSlot(--itemSelected); }

    private void UpdateSelect()
    {
        float temp = SelectOriginalPos.x + (85.2f * itemSelected);
        InvSelect.rectTransform.anchoredPosition = new Vector2(temp, SelectOriginalPos.y);
    }

    public void ShowFullInventory(bool status)
    {
        OpenInvFull = !OpenInvFull;
        InvFull.enabled = OpenInvFull;
        iconParentFull.SetActive(OpenInvFull);
    }

    public void ToggleFullInventory() { ShowFullInventory(!OpenInvFull); }

    public void ShowBarInventory(bool status)
    {
        OpenInvBar = !OpenInvBar;
        InvBar.enabled = OpenInvBar;
        iconParentBar.SetActive(OpenInvBar);
    }

    public void DropItem(bool allItems)
    {
        if (UserItemsBar[itemSelected] != null)
        {
            Vector3 tempLooking = gameObject.transform.position;
            tempLooking += 1.5f * transform.forward;

            int tirarCant = 0;
            if (allItems) tirarCant = ItemCantBar[itemSelected];
            else tirarCant = 1;

            GameObject tempPrefab = Instantiate(UserItemsBar[itemSelected].itemPrefab, tempLooking, gameObject.transform.rotation);
            tempPrefab.GetComponent<ItemsInterface>().onDrop(transform.position, transform.forward, tirarCant);
            if (tirarCant == ItemCantBar[itemSelected])
            {
                UserItemsBar[itemSelected] = null;
                ItemIconBar[itemSelected].enabled = false;
            }
            ItemCantBar[itemSelected] -= tirarCant;
        }
    }

    public int AddItem(ItemData item, int cant)
    {
        if (cant > item.MaxStack) return -1;
        int[] addBar = new int[sizeBar];
        int[] addFull = new int[sizeFull];
        int cantRestante = cant;
        int LastAssigned = -1;
        
        // Reviso para llenar slots sin el máximo del item.
        for(int i = 0; i < sizeBar; i ++)
        {
            addBar[i] = 0;
            ItemData usrIt = UserItemsBar[i];
            if (cantRestante > 0 && usrIt != null)
            {
                int disponible = usrIt.MaxStack - ItemCantBar[i];
                if (usrIt.ID == item.ID && disponible > 0)
                {
                    addBar[i] += disponible;
                    cantRestante -= disponible;
                    LastAssigned = i;
                }
            }
            else break;
        }
        for (int i = 0; i < sizeFull; i++)
        {
            addFull[i] = 0;
            ItemData usrIt = UserItemsFull[i];
            if (cantRestante > 0 && usrIt != null)
            {
                int disponible = usrIt.MaxStack - ItemCantFull[i];
                if (usrIt.ID == item.ID && disponible > 0)
                {
                    addFull[i] += disponible;
                    cantRestante -= disponible;
                    LastAssigned = i + 10;
                }
            }
            else break;
        }

        // Reviso para llenar slots vacios.
        // Si estoy agarrando un item y el slot objetivo está vacio... entra todo.
        // Le pongo la cantidad y listo. Un item del piso nunca va a tener mas del MaxStack.
        for (int i = 0; i < sizeBar; i++)
        {
            if (cantRestante > 0)
            {
                ItemData usrIt = UserItemsBar[i];
                if (usrIt == null)
                {
                    addBar[i] += cant;
                    cantRestante -= cant;
                    LastAssigned = i;
                }
            }
            //else break;
        }
        for (int i = 0; i < sizeFull; i++)
        {
            if (cantRestante > 0)
            {
                ItemData usrIt = UserItemsFull[i];
                if (usrIt == null)
                {
                    addFull[i] += cant;
                    cantRestante -= cant;
                    LastAssigned = i + 10;
                }
            }
            else break;
        }

        // Chequeo y seteo.
        // Chequeo:
        if (cantRestante < 0 && LastAssigned != -1)
        {
            if(LastAssigned <= 9)
            {
                addBar[LastAssigned] += cantRestante;
                cantRestante = 0;
            }
            if (LastAssigned >= 10)
            {
                LastAssigned -= 10;
                addFull[LastAssigned] += cantRestante;
                cantRestante = 0;
            }
        }

        // Seteo:
        for (int i = 0; i < sizeBar; i++)
        {
            if (addBar[i] > 0)
            {
                UserItemsBar[i] = item;
                ItemCantBar[i] += addBar[i];
                ItemIconBar[i].sprite = UserItemsBar[i].Icon;
                ItemIconBar[i].enabled = true;
            }
        }
        for (int i = 0; i < sizeFull; i++)
        {
            if(addFull[i] > 0)
            {
                UserItemsFull[i] = item;
                ItemCantFull[i] += addFull[i];
                ItemIconFull[i].sprite = UserItemsFull[i].Icon;
                ItemIconFull[i].enabled = true;
            }
        }

        // Devuelvo:
        int agarrados = cant - cantRestante;
        return agarrados;
        //return cantRestante;
    }
}
