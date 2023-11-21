using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent nma;
    private Transform playerTransform;

    // if spotted chase or shoot
    public bool playerSpotted = false;
    public bool playerInSight = false;


    public void TakeDamage(int damage)
    {
        //
        Debug.Log("TakeDamage");
    }

    private void Awake()
    {
        nma= GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {

        // patrol by default?


        if (playerSpotted)
        {
            if(playerInSight)
            {
                // if player in sight - stop and shoot. Check with a raycast
                AttackPlayer();
            } 
            else
            {
                // if gone out of sight - chase
                ChasePlayer();
            }
        }


        
    }

    private void CheckPlayeriInSight()
    {
        // ray in direction of player, if no obstacles - stop and shoot
        // if there are - chase
    }

    private void AttackPlayer()
    {
        // stop run animation
    }

    private void ChasePlayer()
    {
        if (playerTransform != null)
        {
            // start run animation
            nma.SetDestination(playerTransform.position);
        }
    }
}
