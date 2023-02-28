using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Nueva construccion", menuName = "Construccion/Constr Data")]
public class ConstrData : ScriptableObject
{
    public string constrNombre;
    public ConstrCategory categoria;
    public Mesh mesh;
    public Material material;
    public SnapPointGroup snapData;
}
