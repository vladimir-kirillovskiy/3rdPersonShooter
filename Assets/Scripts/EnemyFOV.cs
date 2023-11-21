using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 100f;

    public LayerMask targetMask; // a player
    public LayerMask obstacleMask; // everything but a player

    // list of all targets to overlap a sphere
    private List<Transform> visibleTargets = new List<Transform>();

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }


    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    private void FindVisibleTargets()
    {
        if (enemy.playerSpotted) return;

        visibleTargets.Clear();

        // should only have one match as it is single player
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        // check every found target
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;


            // if within the FOV
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                
                // if within a distance
                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    // double check if this is our player
                    // as an option find a player by tag and get name to avoid hardcode
                    if (targetsInViewRadius[i].name == "Character_MercenaryMale_01") 
                    {
                        enemy.playerSpotted = true;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Draw field of view cone
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int rayCount = 1; // Number of rays to simulate the field of view
        float angleStep = viewAngle / rayCount;

        Gizmos.color = Color.red;

        // Draw the rays for the field of view
        for (float i = -viewAngle / 2; i <= viewAngle / 2; i += angleStep)
        {
            float angleRad = Mathf.Deg2Rad * i;
            Vector3 direction = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));

            Gizmos.DrawRay(transform.position, transform.TransformDirection(direction) * viewRadius);
        }
    }
}
