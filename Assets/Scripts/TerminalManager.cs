using System;
using TMPro;
using UnityEngine;

public class TerminalManager : MonoBehaviour {
    public static TerminalManager Instance;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI guessesText;
    [SerializeField] private GameObject leftScramble, rightScramble;
    //[SerializeField] private
    private float timer = 30;
    private bool tickTimer;
    private int guesses = 4;

    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    public void Update() {
        if (tickTimer) {
            UpdateTimer(timer - Time.deltaTime);
            if (timer <= 0f) {
                tickTimer = false;
                UpdateTimer(0);
                Lose();
            }
        }
    }

    /*public int ProcessGuess(string _value) {
        
    }*/

    private void UpdateTimer(float _newValue) {
        timer = _newValue;
        timeText.text = FormatTime(timer);
    }

    public void Lose() {
        Debug.Log("Lost!");
    }

    string FormatTime(float seconds) {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        return formattedTime;
    }
}
