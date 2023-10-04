using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMiniGame : MonoBehaviour
{
	public GameObject miniGame;
	public GameObject wall;


	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player")){
			camAnim.SetTrigger("shake");
			miniGame.SetActive(true);
			Destroy(wall.gameObject);
		}
	}
}
