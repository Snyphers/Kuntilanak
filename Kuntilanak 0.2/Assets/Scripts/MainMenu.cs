using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameInspector GI;

    [SerializeField] GameObject Setting;
    [SerializeField] GameObject Credit;

    [SerializeField] Slider MouseX;
    [SerializeField] Slider MouseY;
    [SerializeField] Slider BGM;
    [SerializeField] Slider SFX;

    [SerializeField] TextMeshProUGUI BGMText;
    [SerializeField] TextMeshProUGUI SFXText;
    [SerializeField] TextMeshProUGUI MouseXText;
    [SerializeField] TextMeshProUGUI MouseYText;

    [SerializeField] AudioMixer SoundEffect;
    [SerializeField] AudioMixer BackgroundMusic;

    [SerializeField] GameObject Click;
    [SerializeField] GameObject ClickLoc;

    public void Start()
    {
        BGM.value = GI.BGM;
        SFX.value = GI.SFX;
        MouseX.value = GI.MouseX;
        MouseY.value = GI.MouseY;

        BGMText.text = GI.BGM.ToString("N0");
        SFXText.text = GI.SFX.ToString("N0");
        MouseXText.text = GI.MouseX.ToString("N0");
        MouseYText.text = GI.MouseY.ToString("N0");
    }

    public void HoverUI(Transform Loc)
    {
        Loc.localScale = new Vector3(1.2f, 1.2f, 1f);
        ClickLoc = Instantiate(Click, Loc);
    }

    public void QuitHover(Transform Loc)
    {
        Loc.transform.localScale = new Vector3(1, 1f, 1f);
        Destroy(ClickLoc);
    }

    public void NormalHover(Transform Loc)
    {
        Loc.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void NormalQuit(Transform Loc)
    {
        Loc.transform.localScale = new Vector3(1, 1f, 1f);
    }

    public void TurnCredit()
    {
        if (Credit.activeSelf)
        {
            Credit.SetActive(false);
        }
        else
        {
            Credit.SetActive(true);
        }
    }

    public void TurnSetting()
    {
        if (Setting.activeSelf)
        {
            Setting.SetActive(false);
        }
        else
        {
            Setting.SetActive(true);
        }
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
        MouseXText.text = X.ToString("N0");
    }

    public void MouseSensitivityY(float Y)
    {
        GI.MouseY = Y;
        MouseYText.text = Y.ToString("N0");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Intro Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}