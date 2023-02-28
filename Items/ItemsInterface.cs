using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ItemsInterface
{
    public ItemData itemData { get; set; }
    public int cantidad { get; set; }

    public void onDrop(Vector3 position, Vector3 direction, int cant);
    public void onPickup(int cant);
    //public void onUse();
}
