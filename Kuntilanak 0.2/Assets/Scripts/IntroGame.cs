using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class IntroGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GotoMainMenu());
    }

    IEnumerator GotoMainMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu");
    }
}
