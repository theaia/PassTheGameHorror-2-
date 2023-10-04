using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameGoal : MonoBehaviour
{
    public GameObject miniGame;

	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Dot")){ 
			camAnim.SetTrigger("shake");
			miniGame.SetActive(false);
		}
	}
}
