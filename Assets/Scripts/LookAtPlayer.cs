using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private ZombieScript zombie;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!zombie.dead)
        {
            if (agent.velocity != Vector3.zero)
            {
                transform.up = Vector3.Slerp(transform.up, transform.position - new Vector3(PlayerScript.Instance.transform.position.x, PlayerScript.Instance.transform.position.y), rotationSpeed * Time.deltaTime);
            }
            else
            {
                // do nothing
            }
        }

    }
}
