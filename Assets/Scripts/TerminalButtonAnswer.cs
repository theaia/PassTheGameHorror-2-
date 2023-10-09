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

	private void Awake() {
		CreateTerminalAnswer();
	}

	private void CreateTerminalAnswer() {
		for (int i = 1; i < representativeString.Length; i++) {
			TerminalButton _terminalButton = Instantiate(prefab, transform.parent);
			connectedButtons.Add(_terminalButton);
			_terminalButton.transform.SetSiblingIndex(transform.GetSiblingIndex()+i);
			_terminalButton.SetupCharacter(representativeString[i].ToString(), this);
		}
	}

	public void PartMousedOver(TerminalButton _sender = null) {
		text.color = overColor;
		Color _color = background.color;
		_color.a = 1f;
		background.color = _color;
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
		/*foreach (TerminalButton _button in connectedButtons) {
		    OnMouseDown();
		}*/
	}
}