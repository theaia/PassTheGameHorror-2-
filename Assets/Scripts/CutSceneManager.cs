using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private bool isFinalCutscene;
    [SerializeField] private float cutsceneDuration;
    [SerializeField] private ScreenFaderAndLoader screenFader;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndCutscene(cutsceneDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            FinishCutscene();
        }
    }

    private IEnumerator EndCutscene(float duration)
    {
        yield return new WaitForSeconds(duration);
        FinishCutscene();
    }

    private void FinishCutscene()
    {
        if (isFinalCutscene)
            screenFader.LoadMainMenu();
        else
            screenFader.LoadNextLevel();
    }
}
