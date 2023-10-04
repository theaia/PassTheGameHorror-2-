using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
	public float minRotationSpeed = 30.0f; // Minimum rotation speed (degrees per second)
	public float maxRotationSpeed = 180.0f; // Maximum rotation speed (degrees per second)

	private float rotationSpeed;

	private void Start()
	{
		// Generate a random rotation speed within the specified range
		rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

		// Randomly determine the direction of rotation (clockwise or counterclockwise)
		if (Random.value > 0.5f)
		{
			rotationSpeed *= -1.0f; // Reverse direction (counterclockwise)
		}
	}

	private void Update()
	{
		// Rotate the object based on the calculated rotation speed
		transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
	}
}
