using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

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
    private MultiAimConstraint aimRig;
    [SerializeField]
    private ParticleSystem muzzleFlush;
    [SerializeField]
    private Transform casingSpawnPoint;
    [SerializeField]
    private GameObject bulletCasing;

    [SerializeField]
    private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private float aimRigWeight = 0f;
    private Vector3 hitPosition;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        // refactor:
        // HandleMouseWorldPosition()
        // Handle Shooting()
        // HandleAiming()


        // hit scan shooting
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Transform hitTransform = null;

        // Note: would "shoot" from the center of the camera, which can ignore obstacles between player and target
        // Shooting from muzzle position offset destination depending on the distance - makes less acurate.
        // In theory can make another ray to check for obstacles

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
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.Sensitivity = normalSensitivity;
            thirdPersonController.SetRotateOnMove(true);

            aimRigWeight = 0f;
        }

        // don't need if replace all animation to a rifle animations
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), aimRigWeight, Time.deltaTime * 10f));


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
                    Transform vfxDust = Instantiate(vfxMiss, hitPosition, Quaternion.identity);
                    vfxDust.forward = raycastHit.normal; // rotate to match hit object normal
                    Destroy(vfxDust.gameObject, 0.1f);
                }
                

            }
            muzzleFlush.Play();

            GameObject casing = Instantiate(bulletCasing, casingSpawnPoint.position, transform.rotation);
            casing.GetComponent<BulletCasing>().AddForce(Vector3.up + transform.right);


            starterAssetsInputs.shoot = false;
        }
    }
}
