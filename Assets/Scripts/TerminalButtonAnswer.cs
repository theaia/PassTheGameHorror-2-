using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalButtonAnswer : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Image background;
	[SerializeField] private Color overColor;
	[SerializeField] private Color standardColor;
	
	[SerializeField] private string representativeString;
	[SerializeField] private TerminalButton prefab;
	private List<TerminalButton> connectedButtons = new List<TerminalButton>();
	
	public void CreateTerminalAnswer(string _value) {
		representativeString = _value;
		text.text = _value[0].ToString();
		for (int i = 1; i < representativeString.Length; i++) {
			TerminalButton _terminalButton = Instantiate(prefab, transform.parent);
			connectedButtons.Add(_terminalButton);
			_terminalButton.transform.SetSiblingIndex(transform.GetSiblingIndex()+i);
			_terminalButton.SetupCharacter(representativeString[i].ToString(), this);
		}
	}

	public int Length() {
		return representativeString.Length;
	}

	public void PartMousedOver(TerminalButton _sender = null) {
		text.color = overColor;
		Color _color = background.color;
		_color.a = 1f;
		background.color = _color;
		if (AudioManager.Instance) AudioManager.Instance.PlaySound("MouseEnter", .15f, Random.Range(.9f, 1f));
		foreach (TerminalButton _button in connectedButtons) {
			if(_button != _sender) _button.MouseOver(true);
		}
	}
	
	public void PartMousedOff(TerminalButton _sender = null) {
		text.color = standardColor;
		Color _color = background.color;
		_color.a = 0f;
		background.color = _color;
		foreach (TerminalButton _button in connectedButtons) {
			if(_button != _sender) _button.MouseOff(true);
		}
	}
	
	public void PartMousedDown(TerminalButton _sender = null) {
		text.color = standardColor;
		Color _color = background.color;
		_color.a = 0f;
		background.color = _color;

		TerminalManager.Instance.ProcessGuess(representativeString);
		
		foreach (TerminalButton _button in connectedButtons) {
			if(_button != _sender) _button.MouseOff(true);
		}
	}

	public void MouseOver() {
		text.color = overColor;
		Color _color = background.color;
		_color.a = 1f;
		background.color = _color;
		foreach (TerminalButton _button in connectedButtons) {
			_button.MouseOver(true);
		}
	}

	public void MouseOff() {
		text.color = standardColor;
		Color _color = background.color;
		_color.a = 0f;
		background.color = _color;
		foreach (TerminalButton _button in connectedButtons) {
			_button.MouseOff(true);
		}
	}
    
	public void MouseDown() {
		text.color = standardColor;
		Color _color = background.color;
		_color.a = 0f;
		background.color = _color;
		
		if(TerminalManager.Instance) TerminalManager.Instance.ProcessGuess(representativeString);
		/*foreach (TerminalButton _button in connectedButtons) {
		    OnMouseDown();
		}*/
	}
}