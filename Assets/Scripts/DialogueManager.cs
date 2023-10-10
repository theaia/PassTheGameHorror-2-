using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float[] lineDelays;
    [SerializeField] private AudioSource voiceLinePlayer;
    [SerializeField] private AudioClip voiceLines;
    [SerializeField] private float textSpeed;

    private bool characterTalkingIsCaptain;
    [SerializeField] private List<int> momentsToSwitchCharacters = new List<int>();
    [SerializeField] private RectTransform CharacterPanel1;
    [SerializeField] private RectTransform CharacterPanel2;

    //[SerializeField] private Animator instructorAnimator;
    //[SerializeField] private Animator canvasAnimator;

    //[SerializeField] private AudioSource radioSound;
    [SerializeField] private AudioSource radioNoise;

    private int index;
    private float animatorTime;
    private bool isTalking;
    private bool hasSkipped;

    //[SerializeField] private List<GameObject> objectsToEnableOnStart = new List<GameObject>();

    void Start()
    {
        textComponent.text = string.Empty;
        StartCoroutine(StartingCall());
    }

    private IEnumerator StartingCall()
    {
        yield return new WaitForSeconds(0.5f);
        //radioSound.Play();
        //yield return new WaitForSeconds(0.7f);
        StartDialogue();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    if (textComponent.text == lines[index])
        //        NextLine();
        //    else
        //    {
        //        StopAllCoroutines();

        //        textComponent.text = lines[index];
        //    }
        //}

        if (!isTalking)
            return;

        animatorTime -= Time.deltaTime;

        if (animatorTime <= 0)
        {
            //instructorAnimator.SetTrigger("Stop");
            isTalking = false;
        }
    }

    private void StartDialogue()
    {
        index = 0;
        //instructorAnimator.ResetTrigger("Stop");
        //instructorAnimator.SetTrigger("Start");
        voiceLinePlayer.clip = voiceLines;
        voiceLinePlayer.Play();
        isTalking = true;
        StartCoroutine(TypeLine(lineDelays[index]));
    }

    private IEnumerator TypeLine(float timeAfterDone)
    {
        foreach (char character in lines[index].ToCharArray())
        {
            textComponent.text += character;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(timeAfterDone);

        NextLine();
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine(lineDelays[index]));
            if (momentsToSwitchCharacters.Contains(index))
            {
                if (characterTalkingIsCaptain)
                {
                    CharacterPanel1.gameObject.SetActive(true);
                    CharacterPanel2.gameObject.SetActive(true);
                    CharacterPanel1.localScale = new Vector2(2f, 2f);
                    //CharacterPanel1.GetChild(0).sizeDelta = new Vector2(0.7f, 0.7f);
                    CharacterPanel1.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    CharacterPanel1.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    CharacterPanel2.localScale = new Vector2(3f, 3f);
                    CharacterPanel2.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                    CharacterPanel2.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                    characterTalkingIsCaptain = !characterTalkingIsCaptain;
                }
                else
                {
                    CharacterPanel1.gameObject.SetActive(true);
                    CharacterPanel2.gameObject.SetActive(true);
                    CharacterPanel2.localScale = new Vector2(2f, 2f);
                    CharacterPanel2.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    CharacterPanel2.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    CharacterPanel1.localScale = new Vector2(3f, 3f);
                    CharacterPanel1.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                    CharacterPanel1.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                    characterTalkingIsCaptain = !characterTalkingIsCaptain;
                }
            }
            //voiceLinePlayer.Stop();
            //voiceLinePlayer.clip = voiceLines;
            //voiceLinePlayer.Play();
            //if (mouthAnimDurations[index] > 0)
            //{
            //    instructorAnimator.ResetTrigger("Stop");
            //    instructorAnimator.SetTrigger("Start");
            //    isTalking = true;
            //    //animatorTime = mouthAnimDurations[index];
            //}
            //else
            //{
            //    instructorAnimator.ResetTrigger("Start");
            //    instructorAnimator.SetTrigger("Stop");
            //    spaceButton.SetActive(true);
            //    isTalking = false;
            //}
        }
        else
        {
            voiceLinePlayer.Stop();
            radioNoise.Stop();
            gameObject.SetActive(false);
            //StartCoroutine(FinishIntro());
        }
    }

    //public void SkipIntro()
    //{
    //    if (hasSkipped)
    //        return;
    //    hasSkipped = true;
    //    voiceLinePlayer.Stop();
    //    StartCoroutine(SkipDialogue());
    //    StartCoroutine(FinishIntro());
    //}

    //private IEnumerator SkipDialogue()
    //{
    //    voiceLinePlayer.clip = voiceLineSkip;
    //    voiceLinePlayer.Play();
    //    skipDialogue.SetActive(true);
    //    yield return new WaitForSeconds(0.5f);
    //    skipDialogue.GetComponent<TextMeshProUGUI>().text = "Really?";
    //    yield return new WaitForSeconds(0.3f);
    //    skipDialogue.GetComponent<TextMeshProUGUI>().text = "I- I'm just trying to help.";
    //}

    //private IEnumerator FinishIntro()
    //{
    //    radioSound.Play();
    //    canvasAnimator.SetTrigger("EndCall");

    //    instructorAnimator.SetTrigger("Stop");

    //    yield return new WaitForSeconds(0.5f);

    //    speedUpAudio.Play();
    //    particlesSpeedUp.SetActive(true);


    //    yield return new WaitForSeconds(1.3f);

    //    SceneManager.LoadScene(2);
    //}
}
