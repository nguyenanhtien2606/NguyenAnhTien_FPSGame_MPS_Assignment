using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text weaponName; 
    [SerializeField] Image weaponAvatar; 
    [SerializeField] Text bulletRemain; 
    [SerializeField] Text totalBullet;
    [SerializeField] Image crossHair;
    [SerializeField] Slider playerHearthSlider;

    [Space]
    [SerializeField] Text limitTimeSurvivalTxt;
    [SerializeField] Text targetReleaseTxt;

    [Space]
    [SerializeField] Image targetProcessImg;

    [Header("Result UI")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultTxt;

    [Header("Mobile UI")]
    [SerializeField] Button mobileKnifeAttackBtn;
    [SerializeField] Button mobileReloadBtn;
    [SerializeField] Button mobileSwapWeaponBtn;
    [SerializeField] Button mobileJumpBtn;

    public static event Action SrpintBtnDown;

    private void OnEnable()
    {
        FireController.UpdateWeaponName += UpdateWeaponName;
        FireController.UpdateWeaponAvatar += UpdateWeaponAvatar;
        FireController.UpdateCurrentAmmo += UpdateBulletRemain;
        FireController.UpdateTotalAmmo += UpdateTotalBullet;

        TargetReleaseController.UpdateReleaseTargetProcess += UpdateReleaseTargetProcess;
    }

    private void OnDisable()
    {
        FireController.UpdateWeaponName -= UpdateWeaponName;
        FireController.UpdateWeaponAvatar -= UpdateWeaponAvatar;
        FireController.UpdateCurrentAmmo -= UpdateBulletRemain;
        FireController.UpdateTotalAmmo -= UpdateTotalBullet;

        TargetReleaseController.UpdateReleaseTargetProcess -= UpdateReleaseTargetProcess;
    }

    public void UpdateWeaponName(string _weaponName)
    {
        weaponName.text = _weaponName;
    }

    public void UpdateWeaponAvatar(Sprite _weaponAvatar)
    {
        weaponAvatar.sprite = _weaponAvatar;
    }

    public void UpdateBulletRemain(string _bulletRemain)
    {
        bulletRemain.text = _bulletRemain;
    }

    public void UpdateTotalBullet(string _totalBullet)
    {
        totalBullet.text = _totalBullet;
    }

    public void DisplayCrossHair(bool isDisplay)
    {
        crossHair.gameObject.SetActive(isDisplay);
    }

    public void UpdateLimitTimeSurvival(string timeTxt)
    {
        limitTimeSurvivalTxt.text = timeTxt;
    }

    public void UpdateTargetPointRelease(string targetCount)
    {
        targetReleaseTxt.text = targetCount;
    }

    void UpdateReleaseTargetProcess(bool isActive, float process)
    {
        targetProcessImg.gameObject.SetActive(isActive);

        if (isActive)
            targetProcessImg.fillAmount = process;
    }

    public void SetActiveMobileKnifeAttack(bool isActive)
    {
        mobileKnifeAttackBtn.interactable = isActive;
    }

    public void SetActiveMobileReload(bool isActive)
    {
        mobileReloadBtn.interactable = isActive;
    }

    public void SetActiveMobileSwapWeapon(bool isActive)
    {
        mobileSwapWeaponBtn.interactable = isActive;
    }

    public void SetActiveMobileJump(bool isActive)
    {
        mobileJumpBtn.interactable = isActive;
    }

    public void DisplayResultPanel(string _resultTxt)
    {
        resultPanel.SetActive(true);
        resultTxt.text = _resultTxt;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UpdatePlayerHearthSlider(float value)
    {
        playerHearthSlider.value = value;
    }
}
