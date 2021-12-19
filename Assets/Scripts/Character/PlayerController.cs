using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;

    public static PlayerController instance;

    [SerializeField] Camera gunCam;
    [SerializeField] List<Camera> mainCam;

    [SerializeField] int normalFOV;
    [SerializeField] int aimingFOV;

    [Space]
    [SerializeField] GameObject lookAtPoint;

    [Header("Player Settings")]
    [SerializeField] int PlayerHeath = 100;

    Coroutine c_delayChangeFOV;

    public GameObject LookAtPoint
    {
        get { return lookAtPoint; }
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;
        instance = this;
    }

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
        gameManager.P_UIController.DisplayCrossHair(!isAiming);
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

    public void Damaged(int damage)
    {
        if (PlayerHeath > 0)
        {
            PlayerHeath -= damage;
        }
        else
        {
            //die
        }
    }
}
