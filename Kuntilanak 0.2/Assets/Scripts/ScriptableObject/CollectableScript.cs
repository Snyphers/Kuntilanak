using System.Collections.Generic;
using System.Collections;
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

    public int Count;
    public Items Item;
    public string Name;
    public Sprite Icon;
}