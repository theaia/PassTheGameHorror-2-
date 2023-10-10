using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneScript : MonoBehaviour
{
    [SerializeField] private ScreenFaderAndLoader screenFader;
    [SerializeField] private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FinalSceneFunctions());
    }

    // Update is called once per frame
    void Update()
    {
        camera.orthographicSize -= 0.1f * Time.deltaTime;
    }

    private IEnumerator FinalSceneFunctions()
    {

        yield return new WaitForSeconds(19);
        screenFader.LoadNextLevel();
    }
}
