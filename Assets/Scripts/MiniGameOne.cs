using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameOne : MonoBehaviour
{
    public float speed;

	public GameObject miniGame;
	public Transform spawnPos;
	public GameObject spider;


	private Animator camAnim;

	private void Start()
	{
		camAnim = Camera.main.GetComponent<Animator>();
	}


	private void Update()
	{
		if(Input.GetMouseButton(1)){
			Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = Vector2.MoveTowards(transform.position, cursorPos, speed * Time.deltaTime);
		}
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("hahaha");
		if(other.CompareTag("Cog")){ 
			camAnim.SetTrigger("shake");
			Debug.Log("goof");
			Instantiate(spider, spawnPos.position, Quaternion.identity);
			miniGame.SetActive(false);
		}
	}
}
