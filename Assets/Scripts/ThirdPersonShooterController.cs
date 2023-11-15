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

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        thirdPersonController= GetComponent<ThirdPersonController>();
         starterAssetsInputs= GetComponent<StarterAssetsInputs>();
    }


    private void Update()
    {
        if(starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.Sensitivity = aimSensitivity;
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.Sensitivity = normalSensitivity;
        }
    }
}
