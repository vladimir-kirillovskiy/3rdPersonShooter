using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private float normalSensitivity = 1.0f;
    [SerializeField]
    private float aimSensitivity = 0.5f;
    [SerializeField]
    private LayerMask aimColliderLayerMask;
    [SerializeField] 
    private Transform vfxHit;
    [SerializeField] 
    private Transform vfxMiss;

    [SerializeField]
    private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private float aimRigWeight;
    private Vector3 hitPosition;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {


        // hit scan shooting
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Transform hitTransform = null;

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
            hitPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = ray.GetPoint(10);
        }


        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.Sensitivity = aimSensitivity;
            thirdPersonController.SetRotateOnMove(false);

            Vector3 wordAimTarget = mouseWorldPosition;
            wordAimTarget.y = transform.position.y;
            Vector3 aimDirection = (wordAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            aimRigWeight = 1f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.Sensitivity = normalSensitivity;
            thirdPersonController.SetRotateOnMove(true);

            aimRigWeight = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }


        if (starterAssetsInputs.shoot)
        {

            if (hitTransform != null)
            {
                
                Enemy enemy = hitTransform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // hit target
                    enemy.TakeDamage(2);
                    Transform vfxBlood = Instantiate(vfxHit, hitPosition, Quaternion.identity);
                    Destroy(vfxBlood.gameObject, 0.5f);
                }
                else
                {
                    // hit something else
                    // rotate - calc angle between vertice and player(shot direction) or just towards the player
                    Transform vfxBlood = Instantiate(vfxMiss, hitPosition, Quaternion.identity);
                    Destroy(vfxBlood.gameObject, 0.5f);
                }
                

            }

            starterAssetsInputs.shoot = false;
        }
    }
}
