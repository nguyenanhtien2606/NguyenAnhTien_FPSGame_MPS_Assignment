using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera gunCam;
    [SerializeField] List<Camera> mainCam;

    [SerializeField] int normalFOV;
    [SerializeField] int aimingFOV;

    [Space]
    [SerializeField] UIController uiController;

    Coroutine c_delayChangeFOV;

    private void OnEnable()
    {
        FireController.IsAim += ChangeFOVAiming;
    }

    private void OnDisable()
    {
        FireController.IsAim -= ChangeFOVAiming;
    }

    void ChangeFOVAiming(bool isAiming)
    {
        if (c_delayChangeFOV != null)
            StopCoroutine(c_delayChangeFOV);

        c_delayChangeFOV = StartCoroutine(C_DelayChangeFOV(isAiming));
        uiController.DisplayCrossHair(!isAiming);
    }

    IEnumerator C_DelayChangeFOV(bool isAiming)
    {
        yield return new WaitForSeconds(0.1f);

        gunCam.fieldOfView = isAiming ? aimingFOV : normalFOV;

        foreach (var cam in mainCam)
        {
            if (cam.gameObject.activeInHierarchy)
                cam.fieldOfView = isAiming ? aimingFOV : normalFOV;
        }
    }
}
