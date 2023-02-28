using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public ItemID ID;
    public string itemName;
    public Sprite Icon;
    public GameObject itemPrefab;
    public int MaxStack;
}
