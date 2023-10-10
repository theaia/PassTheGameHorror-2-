using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using Random = UnityEngine.Random;

namespace TMPCorruptor {
	[RequireComponent(typeof (TextMeshProUGUI))]
	public class TMPCorruptor: MonoBehaviour {
			[Header("TextMeshPro Corruptor By PancakeBoiii")]
			public bool PauseCorruption = false;
			public bool CorruptFont;
			public bool ChangeColours;
			public bool CorruptCharacters;
			public bool CorruptAlphas;
			[Header("Make sure the font you want to use is in TMP's default Font resources folder")]
			public List < string > Fonts;
			public List < string > ReplacementCorruptedCharacters;
			private TextMeshProUGUI TextObject;
			[Header("Corruption Ranges")]
			[Range(0, 1000)]
			public int SpeedOfCorruption = 1;
			[Header("NOTE : Make sure the Character Replacement Corruption Chance\nis more than the amount in the list\nor it could cause issues")]
			[Range(0, 100)]
			public int CharacterReplacementCorruptionChance = 1;
			[TextArea(15, 20)]
			private string Message;
			// Start is called before the first frame update
			void Start() {
				TextObject = this.GetComponent < TextMeshProUGUI > ();
				Message = TextObject.text;
				
				if (TerminalManager.Instance) SpeedOfCorruption = TerminalManager.Instance.Corruption;
				if (SpeedOfCorruption == 0) {
					enabled = false;
				}
			}

			public void Refresh() {
				TextObject = this.GetComponent < TextMeshProUGUI > ();
				Message = TextObject.text;
			}
			public void Reset() {
				TextObject.text = Message;
			}

			// Update is called once per frame
			void LateUpdate() {
				if (PauseCorruption == false) {
					int SOCRNG = Mathf.RoundToInt(Random.Range(1, 420));
					int Chance = Mathf.RoundToInt(Random.Range(1, 3));
					if (SOCRNG < SpeedOfCorruption / Chance) {
						string CorruptedText = "";
						if (CorruptFont == false) {
							for (int i = 0; i < Message.Length; i++) {
								string CurrentChar = "" + Message[i];
								if (CorruptCharacters == true) {
									int RR = Random.Range(0, CharacterReplacementCorruptionChance);
									if (ReplacementCorruptedCharacters.Count <= CharacterReplacementCorruptionChance) {
										if (RR != -1) {
											if (RR == 1) {
												CurrentChar = ReplacementCorruptedCharacters[Random.Range(0, Random.Range(0, ReplacementCorruptedCharacters.Count))];
											} else {
												CurrentChar = CurrentChar;
											}
										}

									} else {
										CurrentChar = CurrentChar;
									}
								}
								if (CorruptAlphas == true) {
									string Alpha = GetRandomHexAlpha();
									if (ChangeColours == true) {
										CorruptedText = CorruptedText + "<color=#" + GetRandomHexColor() + "><alpha=#" + Alpha + ">" + CurrentChar + "</color>";
									} else {
										CorruptedText = CorruptedText + "<alpha=#" + Alpha + ">" + CurrentChar + "";
									}
									
								} else {
									if (ChangeColours == true) {
										CorruptedText = CorruptedText + "<color=#" + GetRandomHexColor() + ">" + CurrentChar + "</color>";
									} else {
										CorruptedText = CorruptedText + "" + CurrentChar;
									}
								}
							}
						}
						if (CorruptFont == true) {
							for (int i = 0; i < Message.Length; i++) {
								string CurrentChar = "" + Message[i];
								if (Message[i] != null) {
									if (CorruptCharacters == true) {
										int RR = Random.Range(0, CharacterReplacementCorruptionChance);
										if (ReplacementCorruptedCharacters.Count <= CharacterReplacementCorruptionChance) {
											if (RR != -1) {
												if (RR == 1) {
													CurrentChar = ReplacementCorruptedCharacters[Random.Range(0, Random.Range(0, ReplacementCorruptedCharacters.Count))];
												} else {
													CurrentChar = CurrentChar;
												}
											}

										} else {
											CurrentChar = CurrentChar;
										}
									}
									if (CorruptAlphas == true) {
									string Alpha = GetRandomHexAlpha();
										CurrentChar = CurrentChar.ToLower();
										if (ChangeColours == true) {
											CorruptedText = CorruptedText + "<color=#" + GetRandomHexColor() + "><alpha=#" + Alpha + ">" + "<font=\"" + Fonts[Random.Range(0, Fonts.Count)] + "\">" + CurrentChar + "</font></color>";
										} else {
											CorruptedText = CorruptedText + "<font=\"" + Fonts[Random.Range(0, Fonts.Count)] + "\"><alpha=#" + Alpha + ">" + CurrentChar + "</font>";
										}
										
									} else {
										CurrentChar = CurrentChar.ToLower();
										if (ChangeColours == true) {
											CorruptedText = CorruptedText + "<color=#" + GetRandomHexColor() + ">" + "<font=\"" + Fonts[Random.Range(0, Fonts.Count)] + "\">" + CurrentChar + "</font></color>";
										} else {
											CorruptedText = CorruptedText + "<font=\"" + Fonts[Random.Range(0, Fonts.Count)] + "\">" + CurrentChar + "</font>";
										}

									}
								}
							}
						}
						TextObject.text = "" + CorruptedText;

					}
				}
			}
			public string GetRandomHexColor() {
				string result = "";
				try {
					int num = Mathf.RoundToInt(Random.Range(100000, 5592405) * 16777215);
					string hexString = num.ToString("X");
					result = hexString.Substring(1, 6);

				} catch {
					result = "FFFFFF";
				}
				return result;
			}
			
			public string GetRandomHexAlpha() {
				string result = "";
				try {
					int num = Mathf.RoundToInt(Random.Range(100000, 5592405) * 255);
					string hexString = num.ToString("X");
					result = hexString.Substring(1, 2);

				} catch {
					result = "FF";
				}
				return result;
			}
		}
		[CustomEditor(typeof (TMPCorruptor))]
	public class RefreshInput: Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			//Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Assets/Textures/GaryHimself.png", typeof(Texture));
			//GUILayout.Box(banner);

			TMPCorruptor tmpc = (TMPCorruptor) target;
			if (GUILayout.Button("Get New Input")) {
				tmpc.Refresh();
			}
			if (GUILayout.Button("Reset Input")) {
				tmpc.Reset();
			}
		}
	}
}