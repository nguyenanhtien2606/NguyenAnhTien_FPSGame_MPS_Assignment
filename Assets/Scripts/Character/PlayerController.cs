using System;
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
    [SerializeField] float PlayerHeath = 100;

    [Space]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip itemTakenSound;

    Coroutine c_delayChangeFOV;
    float maxPlayerHealth;

    public static event Action IsDieAction;

    bool IsDied;

    public GameObject LookAtPoint
    {
        get { return lookAtPoint; }
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;

        if (instance == null)
            instance = this;

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
        if (!IsDied)
        {
            if (PlayerHeath > 0)
            {
                gameManager.P_UIController.UpdatePlayerHearthSlider((PlayerHeath / maxPlayerHealth) - (damage / maxPlayerHealth));
                PlayerHeath -= damage;
            }
            else
            {
                audioSource.PlayOneShot(dieSound);
                gunCam.enabled = false;
                IsDieAction?.Invoke();

                IsDied = true;
            }
        }
    }

    public void UpdatePlayerHealth(int amount)
    {
        gameManager.P_UIController.UpdatePlayerHearthSlider((PlayerHeath / maxPlayerHealth) + (amount / maxPlayerHealth));

        PlayerHeath += amount;

        if (PlayerHeath > maxPlayerHealth)
            PlayerHeath = maxPlayerHealth;
    }

    public void PlayItemTakenShound()
    {
        audioSource.PlayOneShot(itemTakenSound);
    }
}
