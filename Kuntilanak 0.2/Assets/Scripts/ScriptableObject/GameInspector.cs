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

    bool NonDup;

    public void AddItemList(CollectableScript Item)
    {
        NonDup = false;

        if (Items.Count > 0)
        {
            foreach (var Count in Items)
            {
                if (Count.Item == Item.Item)
                {
                    AddItemCount(Item);
                    break;
                }
                else
                {
                    NonDup = true;
                }
            }
        }
        else
        {
            NonDup = true;
        }

        if (NonDup)
        {
            Items.Add(Item);
            Item.Count++;

            NonDup = false;
        }
    }

    private void AddItemCount(CollectableScript Item)
    {
        foreach (var Count in Items)
        {
            if (Count.Item == Item.Item)
            {
                Count.Count++;
            }
            else
            {
                Debug.LogError("Item Problem");
            }
        }
    }
}