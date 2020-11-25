using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public CollectableScript CS;

    public void DestroyThisItem()
    {
        Destroy(gameObject);
    }
}