using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    [SerializeField] GameObject Click;
    [SerializeField] GameObject ClickLoc;

    public void GotoGameScene()
    {
        SceneManager.LoadScene("Level Scene");
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
}