using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TerminalButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image background;
    [SerializeField] private Color overColor;
    [SerializeField] private Color standardColor;
    
    private string characterPool = "!@#$%^:.;'=_-+*()|*{}[]<>?^";

    [SerializeField] private TerminalButtonAnswer parentButton;
    
    public void SetupCharacter(string _value, TerminalButtonAnswer _parent) {
        text.text = _value;
        parentButton = _parent;
    }

    private void Start() {
        if (!parentButton) {
            int randomIndex = Random.Range(0, characterPool.Length);
            char randomCharacter = characterPool[randomIndex];
            string randomCharacterString = randomCharacter.ToString();
            text.text = randomCharacterString;
        }
    }

    public void MouseOver(bool _distributed) {
        text.color = overColor;
        Color _color = background.color;
        _color.a = 1f;
        background.color = _color;

        if (!_distributed && parentButton) {
            parentButton.PartMousedOver(this);
        } else {
            if (AudioManager.Instance) AudioManager.Instance.PlaySound("MouseEnter", .15f, Random.Range(.9f, 1f));
        }
    }

    public void MouseOff(bool _distributed) {
        text.color = standardColor;
        Color _color = background.color;
        _color.a = 0f;
        background.color = _color;

        if (!_distributed && parentButton) {
            parentButton.PartMousedOff(this);
        }
        /*if (IsParent) {
            foreach (TerminalButton _button in connectedButtons) {
                if(_button != _sender) _button.MouseOff();
            }
        } else {
            parentButton.MouseOff(this);
        }*/
    }
    
    public void MouseDown(bool _distributed) {
        text.color = standardColor;
        Color _color = background.color;
        _color.a = 0f;
        background.color = _color;
        if (!_distributed && parentButton) {
            parentButton.PartMousedDown(this);
        } else {
            if(TerminalManager.Instance) TerminalManager.Instance.ProcessGuess(text.text);
        }
    }
}
