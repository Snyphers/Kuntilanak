using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PauseSetting : MonoBehaviour
{
    [SerializeField] GameInspector GI;
    [SerializeField] Player Player;

    public GameObject PauseMenu;

    [SerializeField] ItemUI IUI;
    [SerializeField] GameObject Item;

    [SerializeField] GameObject ItemUI;
    [SerializeField] GameObject SettingUI;
    [SerializeField] GameObject RestartUI;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Exit;

    [SerializeField] GameObject CurrentActive;
    [SerializeField] GameObject PreviousActive;

    [SerializeField] Slider BGM;
    [SerializeField] Slider SFX;
    [SerializeField] Slider MouseX;
    [SerializeField] Slider MouseY;

    [SerializeField] TextMeshProUGUI BGMText;
    [SerializeField] TextMeshProUGUI SFXText;
    [SerializeField] TextMeshProUGUI MouseXText;
    [SerializeField] TextMeshProUGUI MouseYText;

    [SerializeField] AudioMixer BackgroundMusic;
    [SerializeField] AudioMixer SoundEffect;

    [SerializeField] Transform FirstInsta;
    [SerializeField] GameObject Click;
    [SerializeField] GameObject ClickLoc;
    [SerializeField] GameObject YesLoc;

    //public TextMeshProUGUI HealthMessage;

    private void Start()
    {
        PauseMenu.SetActive(false);
        ItemUI.SetActive(true);
        CurrentActive = ItemUI;
        ClickLoc = Instantiate(Click,FirstInsta);

        float BGMValue = (GI.BGM / SFX.minValue * -100) + 100;
        BGMText.text = BGMValue.ToString("N0");

        float SFXValue = (GI.SFX / SFX.minValue * -100) + 100;
        SFXText.text = SFXValue.ToString("N0");

        BGM.value = GI.BGM;
        SFX.value = GI.SFX;
        MouseX.value = GI.MouseX;
        MouseY.value = GI.MouseY;

        //for (int i = 0; i < GI.Items.Count; i++)
        //{
        //    Instantiate(Item, ItemUI.transform);

        //    IUI = Item.GetComponent<ItemUI>();
        //    //IUI.IH = GI.Items[i];
        //    IUI.Name.text = GI.Items[i].Name;
        //    IUI.ItemIcon.sprite = GI.Items[i].Icon;
        //    IUI.Count.text = GI.Items.Count.ToString();

        //    Items.Add(Item);
        //}
    }

    public void Resume()
    {
        GameObject ItemDetail = GameObject.FindWithTag("ItemDetail");
        if (ItemDetail != null)
        {
            Destroy(ItemDetail);
        }

        PauseMenu.SetActive(false);
        Player.Pause = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AddItem(CollectableScript CS)
    {
        Instantiate(Item, ItemUI.transform);

        IUI = Item.GetComponent<ItemUI>();
        IUI.CS = CS;
    }

    public void DestroyItem(CollectableScript Collectable)
    {
        GI.Items.Remove(Collectable);
    }

    public void SBackgroundMusic(float Num)
    {
        BackgroundMusic.SetFloat("Background Music", Num);
        GI.BGM = Num;

        float Show = (Num / SFX.minValue * -100) + 100;
        BGMText.text = Show.ToString("N0");
    }

    public void SSoundEffect(float Num)
    {
        SoundEffect.SetFloat("Sound Effect", Num);
        GI.SFX = Num;

        float Show = (Num / SFX.minValue * -100) + 100;
        SFXText.text = Show.ToString("N0");
    }

    public void MouseSensitivityX(float X)
    {
        GI.MouseX = X;
        Player.MouseSX = X;
        MouseXText.text = X.ToString("N0");
    }

    public void MouseSensitivityY(float Y)
    {
        GI.MouseY = Y;
        Player.MouseSY = Y;
        MouseYText.text = Y.ToString("N0");
    }

    public void ItemDisplay()
    {
        ItemUI.SetActive(true);
        SettingUI.SetActive(false);
    }

    public void SettingDisplay()
    {
        SettingUI.SetActive(true);
        ItemUI.SetActive(false);
    }

    public void HoverUI(Transform Loc)
    {
        Loc.localScale = new Vector3(1.2f, 1.2f, 1f);
    }
    
    public void ClickIt(Transform Loc)
    {
        if (ClickLoc != null)
        {
            Destroy(ClickLoc);
        }
        ClickLoc = Instantiate(Click, Loc);
    }

    public void CloseOther(GameObject Sketch) 
    {
        CurrentActive.SetActive(false);
        CurrentActive = Sketch;
        CurrentActive.SetActive(true);
    }

    public void QuitHover(Transform Loc)
    {
        Loc.transform.localScale = new Vector3( 1 , 1f, 1f);
    }

    public void YesHover(Transform Loc)
    {
        YesLoc = Instantiate(Click, Loc);
    }

    public void YesExit()
    {
        Destroy(YesLoc);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level Scene");
        CleanData();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        CleanData();
    }

    public void CleanData()
    {
        GI.Items.Clear();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CreditScene()
    {
        SceneManager.LoadScene("Credit Scene");
    }
}