using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Nuevo SnapGroup", menuName = "Construccion/Constr SnapPoints")]
public class SnapPointGroup : ScriptableObject
{
    public SnapPoint[] puntos;
}
