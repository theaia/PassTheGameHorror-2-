using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : MonoBehaviour
{
	public bool isRotating = false;
	public float rotationSpeed = 60.0f; // Adjust the speed of rotation as needed

	private Collider2D myCollider;

	public bool noCollisionsDetected;

	private void Start()
	{
		myCollider = GetComponent<Collider2D>();
		
	}

	private void Update()
	{
		// Check for mouse click on the object's collider
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// Check if the mouse click hits the object's collider
			if (myCollider.OverlapPoint(mousePosition))
			{
				// Toggle rotation
				isRotating = !isRotating;
			}
		}

		// Rotate the object if isRotating is true
		if (isRotating)
		{
			transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		noCollisionsDetected = false;

	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		noCollisionsDetected = true;
	}
}
