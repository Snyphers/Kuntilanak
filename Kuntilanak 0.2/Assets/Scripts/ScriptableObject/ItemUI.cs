using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public CollectableScript CS;
    GameObject ItemDetail;
    GameObject CanvasOri;
    Transform Canvas;
    ItemDetail ID;

    [SerializeField] GameObject ItemDetaidPrefab;

    public Image ItemIcon;
    public TextMeshProUGUI Name;
    
    public void Start()
    {
        CanvasOri = GameObject.FindWithTag("Canvas");
        Canvas = CanvasOri.transform;
        ItemIcon.sprite = CS.Icon;
        Name.text = CS.Name;
    }

    public void Clickitem()
    {
        ItemDetail = Instantiate(ItemDetaidPrefab, Canvas);
        ID = ItemDetail.GetComponent<ItemDetail>();

        ID.ItemUI = this;
        ID.CS = CS;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}