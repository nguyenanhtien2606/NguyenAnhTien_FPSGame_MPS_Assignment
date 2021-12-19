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

    [Space]
    [SerializeField] Text limitTimeSurvivalTxt;
    [SerializeField] Text timeSurvivalRecordTxt;
    [SerializeField] Text roundTxt;

    private void OnEnable()
    {
        FireController.UpdateWeaponName += UpdateWeaponName;
        FireController.UpdateWeaponAvatar += UpdateWeaponAvatar;
        FireController.UpdateCurrentAmmo += UpdateBulletRemain;
        FireController.UpdateTotalAmmo += UpdateTotalBullet;
    }

    private void OnDisable()
    {
        FireController.UpdateWeaponName -= UpdateWeaponName;
        FireController.UpdateWeaponAvatar -= UpdateWeaponAvatar;
        FireController.UpdateCurrentAmmo -= UpdateBulletRemain;
        FireController.UpdateTotalAmmo -= UpdateTotalBullet;
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

    public void UpdateTimeSurvivalRecord(string timeTxt)
    {
        timeSurvivalRecordTxt.text = timeTxt;
    }

    public void UpdatePlayingRound(string playingRoundTxt)
    {
        roundTxt.text = playingRoundTxt;
    }
}
