using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    private enum State
    {
        Idle,
        Attacking,
        HeardGunshot,
        Retreating
    }

    private State state;

    public BloodSprayer bloodSprayer;

    public int damage = 1;
    public int health = 3;
    public bool dead = false;

    private NavMeshAgent navMeshAgent;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float playerSightRange;
    [SerializeField] private float gunshotSightRange;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask lastGunshotPositionMask;

    private float enemyIdleTimer;
    private float enemyIdleTimerMax = 5f;

    private bool didEnemyMove = false;

    [SerializeField] private Transform enemySpawnTransform;
    private Collider2D mainCollider;

    private bool wasGunshotFired = false;

    private bool lastFiredGunshotPosInSightRange;

    private AudioSource source;
    public AudioClip[] clips;

    private Animator camAnim;

    public Animator anim;

    private void Start()
    {
        camAnim = Camera.main.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        source.clip = clips[Random.Range(0, clips.Length)];
        enemySpawnTransform.parent = null;
        state = State.Idle;
        navMeshAgent = GetComponent<NavMeshAgent>();
        mainCollider = GetComponent<Collider2D>();
        enemyIdleTimer = enemyIdleTimerMax;

        PlayerScript.Instance.gunManager.OnGunFired += GunManager_OnGunFired;
    }

    private void GunManager_OnGunFired(object sender, System.EventArgs e)
    {
        wasGunshotFired = true;
    }

    public void Hurt(int amnt)
    {
        if (dead)
            return;
        bloodSprayer.SprayBlood();
        health -= amnt;
        if (health <= 0)
        {
            camAnim.SetTrigger("shake");
            GetComponent<Animator>().SetTrigger("OnDeath");
            source.clip = clips[Random.Range(4, 6)];
            source.Play();
            dead = true;
        }
    }

    private void Update()
    {
        if (dead)
        {
            if(anim != null){ 
                anim.speed = 0.05f;
            }
            mainCollider.enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        bool playerInLightSightRange = Physics2D.OverlapCircle(transform.position, playerSightRange, playerMask);
        lastFiredGunshotPosInSightRange = Physics2D.OverlapCircle(transform.position, gunshotSightRange, lastGunshotPositionMask);

        switch (state)
        {
            case State.Idle:
                if (playerInLightSightRange && PlayerScript.Instance.GetIsPlayerInLight())
                {
                    bool once = false;
                    if(once == false){
						source.Play();
                        once = true;
					}
                    
                    state = State.Attacking;
                }
                else if (lastFiredGunshotPosInSightRange && wasGunshotFired)
                {
                    state = State.HeardGunshot;
                }
                else
                {
                    navMeshAgent.SetDestination(navMeshAgent.transform.position);
                }

                if (didEnemyMove)
                {
                    enemyIdleTimer -= Time.deltaTime;
                    if (enemyIdleTimer < 0)
                    {
                        enemyIdleTimer = enemyIdleTimerMax;
                        state = State.Retreating;
                        didEnemyMove = false;
                    }
                }
                break;
            case State.HeardGunshot:

                if (playerInLightSightRange && PlayerScript.Instance.GetIsPlayerInLight())
                {
                    state = State.Attacking;
                }
                else if (!lastFiredGunshotPosInSightRange)
                {
                    didEnemyMove = true;
                    wasGunshotFired = false;
                    state = State.Idle;
                }
                else if (navMeshAgent.hasPath)
                {
                    // Walks toward the gunshot
                    RotateTransformToward(PlayerScript.Instance.gunManager.GetLastGunshotTransform());
                    navMeshAgent.SetDestination(PlayerScript.Instance.gunManager.GetLastGunshotTransform().position);
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 1f)
                    {
                        didEnemyMove = true;
                        wasGunshotFired = false;
                        state = State.Idle;
                    }
                } 
                else
                {
                    // Gets the path toward the gunshot
                    navMeshAgent.SetDestination(PlayerScript.Instance.gunManager.GetLastGunshotTransform().position);
                }

                break;
            case State.Attacking:
                if (!PlayerScript.Instance.GetIsPlayerInLight())
                {
                    didEnemyMove = true;
                    state = State.Idle;
                }
                else
                {
                    RotateTransformToward(PlayerScript.Instance.transform);
                    navMeshAgent.SetDestination(PlayerScript.Instance.transform.position);
                }

                break;
            case State.Retreating:

                if (playerInLightSightRange && PlayerScript.Instance.GetIsPlayerInLight())
                {
                    state = State.Attacking;
                }
                else if (lastFiredGunshotPosInSightRange && wasGunshotFired)
                {
                    state = State.HeardGunshot;
                }
                else if (navMeshAgent.hasPath)
                {
                    RotateTransformToward(enemySpawnTransform);
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        state = State.Idle;
                    }
                }
                else
                {
                    RotateTransformToward(enemySpawnTransform);
                    navMeshAgent.SetDestination(enemySpawnTransform.position);
                }

                break;
        }

    }

    private void RotateTransformToward(Transform objTransform)
    {
        if (navMeshAgent.velocity != Vector3.zero)
        {
            transform.up = Vector3.Slerp(transform.up, transform.position - new Vector3(objTransform.position.x, objTransform.position.y), rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerSightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gunshotSightRange);
    }

}
