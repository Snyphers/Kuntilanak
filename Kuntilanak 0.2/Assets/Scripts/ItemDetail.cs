using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemDetail : MonoBehaviour
{
    Player player;
    PauseSetting PS;
    public ItemUI ItemUI;
    public CollectableScript CS;

    public Image Icon;

    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI Discription;

    public TextMeshProUGUI NoteBookDiscription;

    public GameObject Message;
    public TextMeshProUGUI MessageText;
    //public void 

    private void Start()
    {
        GameObject P = GameObject.FindWithTag("Player");
        player = P.GetComponent<Player>();

        Icon.sprite = CS.Icon;
        ItemName.text = CS.Name;
        Discription.text = CS.Discription;
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }

    public void UseMedKit()
    {
        if (player.CHealth < 100)
        {
            player.CHealth += 25;
            if (player.CHealth > 100)
            {
                player.CHealth = 100;
            }
        }
        else
        {
            Message.SetActive(true);
            MessageText.text = "I don't need Med Kit";
        }
    }
}