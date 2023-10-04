using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class CogTwo : MonoBehaviour
{


	public GameObject[] objectsToCheck; // Array of objects to check for the boolean variable
	public float checkInterval = 5.0f; // Interval (in seconds) for checking the boolean variable
	public string boolVariableName = "myBoolVariable"; // Specify the name of the bool variable to check

	private void Start()
	{
		InvokeRepeating("CheckBoolVariablePeriodically", 0.0f, checkInterval);
	}

	private void CheckBoolVariablePeriodically()
	{
		bool allTrue = true;

		foreach (var obj in objectsToCheck)
		{
			// Check if the object has a boolean variable
			bool boolValue = GetBoolVariableValue(obj);

			// Check the boolean variable
			if (!boolValue)
			{
				allTrue = false;
				break; // Exit the loop early if a false value is found
			}
		}

		// Check the result
		if (allTrue)
		{
			// Perform your action here when the bool variable is true for all objects
			Debug.Log("All objects have the boolean variable set to true.");
		}
		else
		{
			Debug.Log("Not all objects have the boolean variable set to true.");
		}
	}

	private bool GetBoolVariableValue(GameObject obj)
	{
		// Use reflection to get the boolean variable value by name
		var component = obj.GetComponent<MonoBehaviour>();
		if (component != null)
		{
			var type = component.GetType();
			var field = type.GetField(boolVariableName);
			if (field != null && field.FieldType == typeof(bool))
			{
				return (bool)field.GetValue(component);
			}
		}

		return false; // Return false if the bool variable doesn't exist or is of the wrong type
	}

}
