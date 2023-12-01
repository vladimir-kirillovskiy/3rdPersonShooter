using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent nma;
    [HideInInspector]
    public Transform playerTransform;

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

        // remove spotted from Enemy and use in EnemyFOV
        // add playerToShoot bool?
        if (playerSpotted)
        {

            CheckPlayeriInSight();

            if (playerInSight)
            {
                // if player in sight - stop and shoot. Check with a raycast
                transform.LookAt(playerTransform.position);
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
        //if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask) { }
    }

    private void AttackPlayer()
    {
        // stop run animation
        Debug.Log("Attack");
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
