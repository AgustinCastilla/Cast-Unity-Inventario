using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour, ItemsInterface
{
    [SerializeField] protected ItemData _itemData;
    [SerializeField] protected int _cantidad;
    public virtual ItemData itemData
    {
        get { return _itemData; }
        set { _itemData = itemData; }
    }

    public virtual int cantidad
    {
        get { return _cantidad; }
        set { _cantidad = cantidad; }
    }

    public virtual void onDrop(Vector3 position, Vector3 direction, int cant)
    {
        Rigidbody RB = GetComponent<Rigidbody>();
        RB.AddForce(3*direction, ForceMode.VelocityChange);
        if(cant < 1) Destroy(gameObject);
        else _cantidad = cant;
    }
    public virtual void onPickup(int cant)
    {
        if (cant >= _cantidad) Destroy(gameObject);
        else _cantidad -= cant;
    }    
    //public abstract void onUse();
}
