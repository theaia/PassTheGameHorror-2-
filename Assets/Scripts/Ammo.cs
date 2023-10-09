
using System;
using UnityEngine;

public class Ammo : MonoBehaviour {
    [SerializeField] private int ammoValue;
    private AudioSource audioSource;
    private Collider2D trigger;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        trigger = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerScript>().gunManager.ChangeAmmoAmount(ammoValue);
            audioSource.Play();
            transform.GetChild(0).gameObject.SetActive(false);
            trigger.enabled = false;
        }
    }
}
