using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryThroughObject : MonoBehaviour
{
    private static CarryThroughObject instance;

	private void Awake()
	{
		if(instance != null){ 
			Destroy(this);

		} else{ 
			instance = this;
			DontDestroyOnLoad(instance);
		}
	}
}
