using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent nma;
    private Transform playerTransform; 


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
        nma.SetDestination(playerTransform.position);
    }
}
