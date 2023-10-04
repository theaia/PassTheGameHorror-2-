using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
	public AudioClip[] audioClips; // Array of audio clips to choose from
	public float minPitch = 0.7f; // Minimum pitch value
	public float maxPitch = 1.3f; // Maximum pitch value

	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		if (audioClips.Length > 0)
		{
			// Choose a random audio clip from the array
			int randomIndex = Random.Range(0, audioClips.Length);
			AudioClip randomClip = audioClips[randomIndex];

			// Generate a random pitch value within the specified range
			float randomPitch = Random.Range(minPitch, maxPitch);

			// Set the pitch of the audio source
			audioSource.pitch = randomPitch;

			// Play the random audio clip
			audioSource.PlayOneShot(randomClip);
		}
		else
		{
			Debug.LogWarning("No audio clips assigned.");
		}
	}
}
