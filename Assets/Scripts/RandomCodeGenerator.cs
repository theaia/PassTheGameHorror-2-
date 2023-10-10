using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class RandomCodeGenerator : MonoBehaviour
{
	private TextMeshProUGUI codeText;

	private void Awake() {
		codeText = GetComponent<TextMeshProUGUI>();
	}

	private void Start() {
		// Generate a random digit from 0 to 9
		int randomDigit = Random.Range(0, 10);

		// Generate 6 random alphanumeric characters with random capitalization
		string alphanumericChars = GenerateRandomAlphanumeric(3);

		// Create the random code by combining the random digit, 'x', and alphanumeric characters
		string randomCode = randomDigit + "x" + alphanumericChars;

		// Set the TextMeshPro text component to display the random code
		codeText.text = randomCode;
	}

	private string GenerateRandomAlphanumeric(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyz0123456789";
		char[] result = new char[length];

		for (int i = 0; i < length; i++)
		{
			char randomChar = chars[Random.Range(0, chars.Length)];
			
			result[i] = randomChar;
		}

		return new string(result);
	}
}