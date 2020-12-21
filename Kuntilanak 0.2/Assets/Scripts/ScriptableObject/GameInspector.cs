using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Inspector", menuName = "Game Inspector")]
public class GameInspector : ScriptableObject
{
    public List<CollectableScript> Items;

    [Range(10, 50)] public float MouseX;
    [Range(10, 50)] public float MouseY;
    [Range(-80,0)] public float SFX;
    [Range(-80,0)] public float BGM;

    public void AddItemList(CollectableScript Item)
    {
        Items.Add(Item);
    }
}