using UnityEngine;

[CreateAssetMenu(fileName = "ItemColeccionable", menuName = "Scriptable Objects/ItemColeccionable")]
public class ItemColeccionable : ScriptableObject
{
    [Header("Informacion")]
    public string itemName;
    [TextArea] public string description;

    [Header("Visuales")]
    public Sprite icon;
    public GameObject model3D;
}
