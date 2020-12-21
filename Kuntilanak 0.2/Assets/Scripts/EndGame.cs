using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject CreditScene;

    [SerializeField] GameObject Click;
    [SerializeField] GameObject ClickLoc;

    void Start()
    {
        StartCoroutine(ToCreditScene());
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

    IEnumerator ToCreditScene()
    {
        yield return new WaitForSeconds(50f);
        CreditScene.SetActive(true);
    }

    public void CreditSceneActive()
    {
        CreditScene.SetActive(true);
    }

    public void ToMainmenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
