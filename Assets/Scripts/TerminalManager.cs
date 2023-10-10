using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerminalManager : MonoBehaviour {
    public static TerminalManager Instance;
    [SerializeField] private string[] words;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI guessesText;
    [SerializeField] private GameObject leftScramble, rightScramble;
    [SerializeField] private Transform consolePromptContainer;
    [SerializeField] private TerminalButtonAnswer answerPrefab;
    [SerializeField] private CommandPrompt commandPromptPrefab;
    private string answer = "";
    
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

    private void Start() {
        tickTimer = true;
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

    private void SelectAnswer(string _click) {
        answer = SelectRandomWordNotEqualTo(_click);
    }
    
    private string SelectRandomWordNotEqualTo(string notEqualWord) {
        if (words == null || words.Length == 0) {
            Debug.LogWarning("The 'words' array is empty or null.");
            return null;
        }
        
        List<string> validWords = new List<string>();

        foreach (string word in words) {
            if (!word.Equals(notEqualWord)) {
                validWords.Add(word);
            }
        }
        
        if (validWords.Count == 0) {
            Debug.LogWarning("No valid words found.");
            return null;
        }
        
        int randomIndex = Random.Range(0, validWords.Count);
        return validWords[randomIndex];
    }

    public void ProcessGuess(string _value) {
        if (answer == "") {
            SelectAnswer(_value);
        }
        
        //Check Answer
        int _matchingText = CheckAnswer(_value);

        //Change Guesses
        guesses--;
        string _guessesText = "";
        for (int i = 0; i < guesses; i++) {
            _guessesText += "■";
        }

        guessesText.text = _guessesText;

        if (guesses <= 0) {
            Lose();
        }
        if(AudioManager.Instance) AudioManager.Instance.PlaySound("CPUSubmit", .25f);
        CommandPrompt _commandPrompt2 = Instantiate(commandPromptPrefab, consolePromptContainer);
        _commandPrompt2.SetText($"Similarity {_matchingText}/{answer.Length}");
        CommandPrompt _commandPrompt = Instantiate(commandPromptPrefab, consolePromptContainer);
        _commandPrompt.SetText($"{_value}");
        if (_matchingText == answer.Length) {
            Win();
        }
    }

    private void UpdateTimer(float _newValue) {
        timer = _newValue;
        timeText.text = FormatTime(timer);
    }

    private int CheckAnswer(string _guess) {
        if (_guess.Length != answer.Length) {
            Debug.Log("Strings are not of equal length.");
            return 0; // Return -1 to indicate an error.
        }

        int matchingCount = 0;

        for (int i = 0; i < _guess.Length; i++)
        {
            if (_guess[i] == answer[i])
            {
                matchingCount++;
            }
        }

        return matchingCount;
    }

    public void Win() {
        Debug.Log("Win!");
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
