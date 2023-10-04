using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System;

public class GunManager : MonoBehaviour
{
    public event EventHandler OnGunFired;

    public bool hasPistol = true;
    public int pistolAmmoCount = 10;
    public int pistolDamage = 1;

    public GameObject pistolGraphic;
    public LayerMask gunLayerMask;

    public GameObject bulletHitSparks;

    public TextMeshProUGUI ammoDisplay;

    private bool wasGunFired = false;

    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Transform lastGunshotTransform;

    public GameObject gunSound;


    void Start()
    {
        lastGunshotTransform.parent = null;

        ammoDisplay.text = pistolAmmoCount.ToString();
        if (!hasPistol)
        {
            pistolGraphic.SetActive(false);
            ammoDisplay.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        ammoDisplay.rectTransform.position = Input.mousePosition;
    }

    public void Shoot()
    {
        if (pistolAmmoCount <= 0 || !hasPistol)
            return;
        pistolAmmoCount--;
        if(gunSound != null){
			Instantiate(gunSound);
		}
        
        ammoDisplay.text = pistolAmmoCount.ToString();

        OnGunFired?.Invoke(this, EventArgs.Empty);

        lastGunshotTransform.localPosition = PlayerScript.Instance.transform.localPosition;

        GunshotLightAppear();

        RaycastHit2D hit = Physics2D.Raycast(pistolGraphic.transform.position, -pistolGraphic.transform.up, 1000f, gunLayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var zombie = hit.collider.GetComponent<ZombieScript>();
                if (zombie != null)
                {
                    zombie.Hurt(pistolDamage);
                }
            }
            else
            {
                GameObject hitEffect = Instantiate(bulletHitSparks, hit.point, Quaternion.identity);
                Destroy(hitEffect, 2.0f);
            }
        }
    }

    private void GunshotLightAppear()
    {
        pistolAnimator.SetTrigger("GunShot");
    }

    public Transform GetLastGunshotTransform()
    {
        return lastGunshotTransform;
    }
}
