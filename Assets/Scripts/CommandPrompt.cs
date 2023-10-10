using System.Collections;
using TMPro;
using UnityEngine;

public class CommandPrompt : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI lineText;
    private float typingSpeed = 0.05f;

    public void SetText(string _value) {
        StartCoroutine(TypeText(_value));
    }

    IEnumerator TypeText(string _value) {
        foreach (char letter in _value) {
            lineText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
