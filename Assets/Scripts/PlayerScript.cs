using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;

    public Camera playerCam;
    public ScreenFaderAndLoader screenFader;
    public Animator DeathScreenAnimator;
    public GunManager gunManager;
    public BloodSprayer bloodSprayer;

    public Transform ArmHandMarkerR;
    public Transform ArmHandMarkerL;

    private Rigidbody2D rb;

    public float pushOffSpeed = 30.0f;
    private bool holdingWall = true;
    private Vector2 handGrabPosition;

    float leftWallTime = 0.0f;
    const float cantGrabTime = 0.05f;

    Vector2 wallNormal;
    const float pushingIntoWallDotLimit = -0.1f;

    public int health = 5;
    bool invulnerable = false;
    bool dead = false;

    [SerializeField] private float pushOffSpeedDecreaseMultiplier = 0.9f;
    [SerializeField] private float pushOffSpeedMin = 5f;
    private bool didJump = false;

    // Lights

    private bool isPlayerInLight = false;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private Light2D defaultlight;

    [SerializeField] private float defaultLightIntensity = .1f;
    [SerializeField] private float flashlightIntensity = .2f;
    [SerializeField] private float flashlightIntensityMax = .5f;

    public GameObject jumpSound;
    public GameObject jumpEffect;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        flashlight.gameObject.SetActive(false);
        flashlight.intensity = 0f;

        rb = GetComponent<Rigidbody2D>();
        ArmHandMarkerR.position = gunManager.gameObject.transform.position;
    }

    void Update()
    {
        if (dead)
        {
            if (Input.GetButtonDown("Restart"))
            {
                screenFader.RestartLevel();
            }
            return;
        }
            
        FaceMouse();
        playerCam.transform.rotation = Quaternion.Euler(Vector2.zero);
        ArmHandMarkerL.position = handGrabPosition;
        if (Input.GetButtonDown("Fire1"))
        {
            gunManager.Shoot();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            PushOffWall();
        }

        if (didJump)
        {
            if (rb.velocity.magnitude > pushOffSpeedMin)
            {
               
                rb.velocity *= pushOffSpeedDecreaseMultiplier;   
            }
            else
            {
                // do nothing
            }
            //Debug.Log(rb.velocity.magnitude);
        }
        // TODO: Implement a flashlight feature
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    LightSwitch(false, true);
        //}
    }

    private void LightSwitch(bool isDefaultEnabled, bool isFlashlightEnabled)
    {
        defaultlight.gameObject.SetActive(isDefaultEnabled);
        flashlight.gameObject.SetActive(isFlashlightEnabled);
    }

    void Hurt(int amnt)
    {
        if (invulnerable || dead)
            return;

        health -= amnt;
        if (health <= 0)
        {
            dead = true;
            DeathScreenAnimator.SetTrigger("ShowDeathScreen");
        }
        bloodSprayer.SprayBlood();
    }

    void FaceMouse()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = rb.position - new Vector2(worldPosition.x, worldPosition.y);
    }

    void PushOffWall()
    {
        if (!holdingWall)
            return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2DPosition = new Vector2(worldPosition.x, worldPosition.y);
        Vector2 moveDirection = mouse2DPosition - rb.position;
        moveDirection = moveDirection.normalized;
        if (Vector2.Dot(moveDirection, wallNormal) < pushingIntoWallDotLimit)
            return;

        rb.AddForce(moveDirection * pushOffSpeed, ForceMode2D.Impulse);
        if(jumpSound != null){
			Instantiate(jumpSound);
		}
		if (jumpEffect != null)
		{
			Instantiate(jumpEffect, transform.position, transform.rotation);
		}


		holdingWall = false;
        didJump = true;
        leftWallTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GrabWall(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (holdingWall || (Time.time - leftWallTime) < cantGrabTime)
            return;
        GrabWall(collision);
    }

    void GrabWall(Collision2D collision)
    {
		if (jumpSound != null)
		{
			Instantiate(jumpSound);
		}
		if (jumpEffect != null)
		{
			Instantiate(jumpEffect, transform.position, transform.rotation);
		}
		rb.velocity = Vector2.zero;
        handGrabPosition = collision.GetContact(0).point;
        wallNormal = collision.GetContact(0).normal;
        holdingWall = true;
        didJump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelExit"))
        {
            screenFader.LoadNextLevel();
            rb.velocity = rb.velocity * 0.1f;
            invulnerable = true;
        }
        if (collision.CompareTag("Enemy"))
        {
            ZombieScript zombie = collision.GetComponent<ZombieScript>();
            if (zombie != null && !zombie.dead)
            {
                Hurt(zombie.damage);
            }
        }
        if (collision.gameObject.layer == 8)
        {
            isPlayerInLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isPlayerInLight = false;
        }
    }

    public bool GetIsPlayerInLight()
    {
        return isPlayerInLight;
    }
}
