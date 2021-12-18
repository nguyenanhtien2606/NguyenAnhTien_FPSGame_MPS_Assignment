using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float smoothFireDelay = 0.25f;

    float lastFireTime;

    Gun currentGun;
    WeaponType currentWeaponType;
    WeaponFireType currentWeaponFireType;

    #region Enable Observer
    //Change Anim Object
    public static event Action<WeaponType> ChangeAnimatorRuntime;

    //Fire Action
    public static event Action<bool, Gun> IsFire;
    public static event Action IsReload;
    #endregion

    private void Start()
    {
        gameManager = GameManager.GLOBAL;

        //default Weapon
        currentGun = new Gun();
        currentGun = gameManager.P_WeaponController.GunLst[0];
        SetCurrentWeapon(currentGun.P_WeaponType, currentGun.P_WeaponFireType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = gameManager.P_WeaponController.GunLst[0];
            SetCurrentWeapon(currentGun.P_WeaponType, currentGun.P_WeaponFireType);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGun = gameManager.P_WeaponController.GunLst[1];
            SetCurrentWeapon(currentGun.P_WeaponType, currentGun.P_WeaponFireType);
        }

        //Change Rifle Fire Style
        if (currentWeaponType == WeaponType.Rifle)
        {
            if (Input.GetButtonDown("ChangeFireStyle"))
            {
                currentGun.P_WeaponFireType = (currentGun.P_WeaponFireType == WeaponFireType.Continuous) ? WeaponFireType.Once : WeaponFireType.Continuous;

                currentWeaponFireType = currentGun.P_WeaponFireType;
            }
        }

        #region Observer
        //Fire
        if ((lastFireTime + smoothFireDelay) < Time.time)
        {
            if (currentWeaponFireType == WeaponFireType.Continuous)
            {
                IsFire?.Invoke(Input.GetButton("Fire1"), currentGun);
                if (Input.GetButton("Fire1"))
                {
                    lastFireTime = Time.time;
                }
            }
            else
            {
                IsFire?.Invoke(Input.GetButtonDown("Fire1"), currentGun);
                if (Input.GetButtonDown("Fire1"))
                {
                    lastFireTime = Time.time;
                }
            }  
        }

        //Reload
        if (Input.GetButtonDown("Reload"))
        {
            IsReload?.Invoke();
        }
        #endregion

    }

    void SetCurrentWeapon(WeaponType weaponType, WeaponFireType weaponFireType)
    {
        currentWeaponType = weaponType;
        currentWeaponFireType = weaponFireType;

        #region Observer Change Anim Object
        ChangeAnimatorRuntime?.Invoke(weaponType);
        #endregion
    }
}
