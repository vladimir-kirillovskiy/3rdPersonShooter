using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : MonoBehaviour
{
    [SerializeField] TrailRenderer bulletTrail;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 point)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        // isShooting = false
        trail.transform.position = point;
        // instanciate hit particle
        Destroy(trail.gameObject, trail.time);
    }
}
