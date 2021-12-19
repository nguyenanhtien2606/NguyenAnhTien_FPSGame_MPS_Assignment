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

    bool isAiming;
    bool isReloading;
    bool isKnifeAttack;

    bool isHolster;
    int gunChangeIndex = 0;
    Coroutine c_autoReload;

    #region Enable Observer
    //Change Anim Object
    public static event Action<WeaponType> ChangeAnimatorRuntime;

    //Fire Action
    public static event Action<bool, Gun> IsFire;

    //Aim Action
    public static event Action<bool> IsAim;

    //Reload Action
    public static event Action IsReloadOutOfAmmo;
    public static event Action IsReloadLeftAmmo;

    //Knife Action
    public static event Action IsKnifeAttack;

    //Holster
    public static event Action IsHolster;

    //UI update Action
    public static event Action<string> UpdateWeaponName;
    public static event Action<Sprite> UpdateWeaponAvatar;
    public static event Action<string> UpdateCurrentAmmo;
    public static event Action<string> UpdateTotalAmmo;
    #endregion

    private void Start()
    {
        gameManager = GameManager.GLOBAL;

        //default Weapon
        currentGun = new Gun();
        currentGun = gameManager.P_WeaponController.GunLst[0];
        SetCurrentWeapon();
    }

    private void OnEnable()
    {
        CheckReloadFinish.IsReloadFinish += ReloadFinish;
        CheckHolsterFinish.IsHolsterFinish += HolsterFinish;
        CheckKnifeAttackFinish.IsKnifeAttackFinish += KnifeAttackFinish;
    }

    private void OnDisable()
    {
        CheckReloadFinish.IsReloadFinish -= ReloadFinish;
        CheckHolsterFinish.IsHolsterFinish -= HolsterFinish;
        CheckKnifeAttackFinish.IsKnifeAttackFinish -= KnifeAttackFinish;
    }

    private void Update()
    {
        //Change Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1) && gunChangeIndex != 0 && !isHolster)
        {
            isHolster = true;
            gunChangeIndex = 0;

            IsHolster?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && gunChangeIndex != 1 && !isHolster)
        {
            isHolster = true;
            gunChangeIndex = 1;

            IsHolster?.Invoke();
        }

        #region Observer
        //Fire
        if (currentGun.CurrentAmmo > 0 && (lastFireTime + smoothFireDelay) < Time.time && !isReloading && !isKnifeAttack)
        {
            if (currentGun.P_WeaponFireType == WeaponFireType.Continuous)
            {
                IsFire?.Invoke(Input.GetButton("Fire1"), currentGun);
                if (Input.GetButton("Fire1"))
                {
                    ActionWhenFireOneTime();
                }
            }
            else
            {
                IsFire?.Invoke(Input.GetButtonDown("Fire1"), currentGun);
                if (Input.GetButtonDown("Fire1"))
                {
                    ActionWhenFireOneTime();
                }
            }
        }

        //Aim
        if (!isReloading && !isKnifeAttack)
        {
            if (Input.GetButtonDown("Fire2") && !isAiming && currentGun.CurrentAmmo > 0)
            {
                IsAim?.Invoke(true);
                isAiming = true;
            }
            if (Input.GetButtonUp("Fire2") && isAiming)
            {
                IsAim?.Invoke(false);
                isAiming = false;
            }
        }

        //Change Fire Style when aiming
        currentGun.P_WeaponFireType = isAiming ? WeaponFireType.Continuous : WeaponFireType.Once;

        //Reload
        if (Input.GetButtonDown("Reload") && !isReloading && currentGun.CurrentAmmo < currentGun.MaxAmmo && !isKnifeAttack)
        {
            if (c_autoReload != null)
                StopCoroutine(c_autoReload);

            Reload();
        }

        //Knife Attack
        if (Input.GetButtonDown("KnifeAttack") && !isKnifeAttack)
        {
            KnifeAttack();
        }

        //Off Fire, Aiming action
        if (currentGun.CurrentAmmo <= 0 || isReloading || isKnifeAttack)
        {
            IsFire?.Invoke(false, currentGun);

            if (isAiming)
            {
                IsAim?.Invoke(false);
                isAiming = false;
            }
        }
        #endregion

    }

    void ActionWhenFireOneTime()
    {
        //Update Fire moment
        lastFireTime = Time.time;

        //Update current ammo remain
        currentGun.CurrentAmmo -= 1;

        //Auto reload ammo
        if (currentGun.CurrentAmmo <= 0)
        {
            if (c_autoReload != null)
                StopCoroutine(c_autoReload);

            c_autoReload = StartCoroutine(C_AutoReload());
        }

        #region Observer
        //Update UI
        UpdateCurrentAmmo?.Invoke(currentGun.CurrentAmmo.ToString());
        #endregion
    }

    IEnumerator C_AutoReload()
    {
        yield return new WaitForSeconds(0.5f);
        Reload();
    }

    void Reload()
    {
        if (currentGun.TotalAmmo == 0)
        {
            return;
        }

        isReloading = true;

        //Anim Action
        if (currentGun.CurrentAmmo > 0)
            IsReloadLeftAmmo?.Invoke();
        else
            IsReloadOutOfAmmo?.Invoke();
    }

    void KnifeAttack()
    {
        isKnifeAttack = true;
        IsKnifeAttack?.Invoke();
    }

    void SetCurrentWeapon()
    {
        isReloading = false;

        //Clear old coroutine
        if (c_autoReload != null)
            StopCoroutine(c_autoReload);

        //Auto reload ammo
        if (currentGun.CurrentAmmo <= 0)
            c_autoReload = StartCoroutine(C_AutoReload());

        #region Observer
        //Change Anim Object
        ChangeAnimatorRuntime?.Invoke(currentGun.P_WeaponType);

        //Update UI
        UpdateWeaponName?.Invoke(currentGun.GunName);
        UpdateWeaponAvatar?.Invoke(currentGun.GunAvatar);
        UpdateCurrentAmmo?.Invoke(currentGun.CurrentAmmo.ToString());
        UpdateTotalAmmo?.Invoke(currentGun.TotalAmmo.ToString());
        #endregion
    }

    void ReloadFinish(bool isReloadFinish)
    {
        isReloading = false;

        if (isReloadFinish)
        {
            //Update amount Ammo
            if ((currentGun.TotalAmmo - currentGun.MaxAmmo) > 0)
            {
                int amountAmmoPlus = currentGun.MaxAmmo - currentGun.CurrentAmmo;
                currentGun.CurrentAmmo += amountAmmoPlus;
                currentGun.TotalAmmo -= amountAmmoPlus;
            }
            else
            {
                currentGun.CurrentAmmo += currentGun.TotalAmmo;
                currentGun.TotalAmmo = 0;
            }

            //Update UI
            UpdateCurrentAmmo?.Invoke(currentGun.CurrentAmmo.ToString());
            UpdateTotalAmmo?.Invoke(currentGun.TotalAmmo.ToString());
        }
    }

    void HolsterFinish()
    {
        isHolster = false;
        currentGun = gameManager.P_WeaponController.GunLst[gunChangeIndex];

        SetCurrentWeapon();
    }

    void KnifeAttackFinish()
    {
        isKnifeAttack = false;
    }
}
