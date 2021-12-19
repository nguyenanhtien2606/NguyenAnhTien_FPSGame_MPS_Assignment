using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;

    public static PlayerController instance;

    CapsuleCollider col;

    [SerializeField] Camera gunCam;
    [SerializeField] List<Camera> mainCam;

    [SerializeField] int normalFOV;
    [SerializeField] int aimingFOV;

    [Space]
    [SerializeField] GameObject lookAtPoint;

    [Header("Player Settings")]
    [SerializeField] float PlayerHeath = 100;

    Coroutine c_delayChangeFOV;
    float maxPlayerHealth;

    public static event Action IsDie;

    public GameObject LookAtPoint
    {
        get { return lookAtPoint; }
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;

        if (instance == null)
            instance = this;

        col = GetComponent<CapsuleCollider>();
        maxPlayerHealth = PlayerHeath;
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

    public void Damaged(float damage)
    {
        if (PlayerHeath > 0)
        {
            gameManager.P_UIController.UpdatePlayerHearthSlider((PlayerHeath / maxPlayerHealth) - (damage / maxPlayerHealth));
            PlayerHeath -= damage;
        }
        else
        {
            gunCam.gameObject.SetActive(false);
            IsDie?.Invoke();
        }
    }

    public void UpdatePlayerHealth(int amount)
    {
        gameManager.P_UIController.UpdatePlayerHearthSlider((PlayerHeath / maxPlayerHealth) + (amount / maxPlayerHealth));

        PlayerHeath += amount;

        if (PlayerHeath > maxPlayerHealth)
            PlayerHeath = maxPlayerHealth;
    }
}
