using UnityEngine;

[CreateAssetMenu(fileName = "Colactable", menuName = "Scriptable / Colactable")]
public class CollectableScript : ScriptableObject
{
    public enum Items
    {
        OfficeKey,
        MainKey,
        MedKit,
        Note
    }

    public Items Item;
    public string Name;
    public Sprite Icon;

    [TextArea(5, 5)] public string Discription;
}