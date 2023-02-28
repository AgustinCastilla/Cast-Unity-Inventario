using UnityEngine;

public class ConstrCollider : MonoBehaviour
{
    private int _count;
    private void OnDisable()
    {
        _count = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _count++;
        //Debug.Log("ColEnter: " + collision.gameObject.name + " - " + _count);
    }
    private void OnTriggerEnter(Collider other)
    {
        _count++;
        //Debug.Log("TriggerEnter: " + other.gameObject.name + " - " + _count);
    }
    private void OnCollisionExit(Collision collision)
    {
        _count--;
        //Debug.Log("ColExit: " + collision.gameObject.name + " - " + _count);
    }
    private void OnTriggerExit(Collider other)
    {
        _count--;
        //Debug.Log("TriggerExit: " + other.gameObject.name + " - " + _count);
    }
    public bool isEmpty()
    {
        return _count == 0;
    }
}
