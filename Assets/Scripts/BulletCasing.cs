using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        
        Destroy(gameObject, 1f);
    
        
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }


}
