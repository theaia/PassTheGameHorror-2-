using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class TerminalManager : MonoBehaviour {
    public static TerminalManager Instance;
    public int Corruption;
    [SerializeField] private string LoseScene;
    [SerializeField] private string WinScene;
    [SerializeField] private string[] words;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI guessesText;
    [SerializeField] private GameObject leftScramble, rightScramble;
    [SerializeField] private Transform consolePromptContainer;
    private List<TerminalButtonAnswer> answersLeft = new List<TerminalButtonAnswer>();
    private List<TerminalButtonAnswer> answersRight = new List<TerminalButtonAnswer>();
    [SerializeField] private TerminalButtonAnswer answerPrefab;
    [SerializeField] private TerminalButton randomButtonPrefab;
    [SerializeField] private CommandPrompt commandPromptPrefab;
    [SerializeField] private GameObject happyGameObject;
    [SerializeField] private GameObject sadGameObject;
    private string answer = "";
    
    private float timer = 45;
    private bool tickTimer;
    private int guesses = 5;

    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        PopulateGuesses();
        UpdateRemainingGuesses();
    }

    private void Start() {
        tickTimer = true;
    }

    private void PopulateGuesses() {
        int _halfWords = words.Length / 2;

        // Shuffle the words using Fisher-Yates shuffle
        System.Random rng = new System.Random();
        for (int i = words.Length - 1; i > 0; i--) {
            int j = rng.Next(i + 1);
            string temp = words[i];
            words[i] = words[j];
            words[j] = temp;
        }
        
        // Instantiate answers after adding random characters
        for (int i = 0; i < words.Length; i++) {
            bool _isLeft = i < _halfWords;
            TerminalButtonAnswer _termAns = Instantiate(answerPrefab, _isLeft ? leftScramble.transform : rightScramble.transform);
            _termAns.CreateTerminalAnswer(words[i]);
            if (i < _halfWords) {
                answersLeft.Add(_termAns);
            } else {
                answersRight.Add(_termAns);
            }
        }
        
        FillScrambleWithRandom(true);
        FillScrambleWithRandom(false);
    }

    private void FillScrambleWithRandom(bool _isLeft) {
        int _halfWords = words.Length / 2;
        int _characterCount = words[0].Length;
        int _charactersNeeded = 120 - (_characterCount * _halfWords); // Subtract the characters in answers

        Transform _parent = _isLeft ? leftScramble.transform : rightScramble.transform;
        List<TerminalButtonAnswer> _answers = _isLeft ? answersLeft : answersRight;
        
        for (int i = 0; i < _charactersNeeded; i++) {
            GameObject _randomButton = Instantiate(randomButtonPrefab.gameObject, _parent);
            _randomButton.transform.localScale = Vector3.one;
            
            // Get random value from _answers
            int randomAnswerIndex = Random.Range(0, _answers.Count);
            TerminalButtonAnswer randomAnswer = _answers[randomAnswerIndex];

            // Get random bool representing beginning/end
            bool isBeforeAnswer = Random.value < 0.5f;

            // Set the index of _randomButton relative to _answers Sibling index
            if (isBeforeAnswer) {
                _randomButton.transform.SetSiblingIndex(randomAnswer.transform.GetSiblingIndex());
            } else {
                _randomButton.transform.SetSiblingIndex(randomAnswer.transform.GetSiblingIndex() + _answers.Count);
            }
        }
    }

    public void Update() {
        if (tickTimer) {
            UpdateTimer(timer - Time.deltaTime);
            if (timer <= 0f) {
                tickTimer = false;
                UpdateTimer(0);
                StartCoroutine(Lose());
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
        UpdateRemainingGuesses();

        if (guesses <= 0) {
            StartCoroutine(Lose());
        }
        if(AudioManager.Instance) AudioManager.Instance.PlaySound("CPUSubmit", .25f);
        CommandPrompt _commandPrompt = Instantiate(commandPromptPrefab, consolePromptContainer);
        _commandPrompt.SetText($"{_value} (Match {_matchingText}/{answer.Length})");
        if (_matchingText == answer.Length) {
            StartCoroutine(Win());
        }
    }

    private void UpdateRemainingGuesses() {
        string _guessesText = "";
        for (int i = 0; i < guesses; i++) {
            _guessesText += "■";
        }

        guessesText.text = _guessesText;
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

    IEnumerator Win() {
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(false);
        happyGameObject.SetActive(true);
        if(AudioManager.Instance) AudioManager.Instance.PlaySound("CPUWin", .25f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(WinScene);
    }
    
    IEnumerator Lose() {
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(false);
        sadGameObject.SetActive(true);
        if(AudioManager.Instance) AudioManager.Instance.PlaySound("CPUFail", .25f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(LoseScene);
    }

    string FormatTime(float seconds) {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        return formattedTime;
    }
}
