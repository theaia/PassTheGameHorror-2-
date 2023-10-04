using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFaderAndLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelAfterFadeIn());
    }

    IEnumerator LoadNextLevelAfterFadeIn()
    {
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Animator>().SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelAfterFadeIn());
    }
    IEnumerator RestartLevelAfterFadeIn()
    {
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Animator>().SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuAfterFadeIn());
    }

    IEnumerator LoadMainMenuAfterFadeIn()
    {
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Animator>().SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
