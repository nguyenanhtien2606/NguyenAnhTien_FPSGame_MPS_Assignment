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

    bool isAimingTrigger;
    bool isKnifeTrigger;
    bool isReloadTrigger;
    bool isSwapTrigger;


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


#if UNITY_ANDROID || UNITY_IPHONE
    //Mobile Input
    bool mobile_IsFire;
    bool mobile_IsAiming;
    bool mobile_IsKnife;
    bool mobile_IsReload;
    bool mobile_IsSwapWeapon;
#endif

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

        ItemDropController.IsTakeItem += TakenItem;
    }

    private void OnDisable()
    {
        CheckReloadFinish.IsReloadFinish -= ReloadFinish;
        CheckHolsterFinish.IsHolsterFinish -= HolsterFinish;
        CheckKnifeAttackFinish.IsKnifeAttackFinish -= KnifeAttackFinish;

        ItemDropController.IsTakeItem -= TakenItem;
    }

    private void Update()
    {
        //Change Weapon
#if UNITY_STANDALONE || UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
            isSwapTrigger = true;

#elif UNITY_ANDROID || UNITY_IPHONE

        isSwapTrigger = mobile_IsSwapWeapon;

#endif
        if (isSwapTrigger && !isHolster)
        {
            isSwapTrigger = mobile_IsSwapWeapon = false;

            isHolster = true;
            gunChangeIndex = (gunChangeIndex != 0) ? 0 : 1;
            IsHolster?.Invoke();
        }

        #region Observer
        //Fire
        if (currentGun.CurrentAmmo > 0 && (lastFireTime + smoothFireDelay) < Time.time && !isReloading && !isKnifeAttack)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
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
#elif UNITY_ANDROID || UNITY_IPHONE
            
            IsFire?.Invoke(mobile_IsFire, currentGun);
            
            if (mobile_IsFire)
            {
                ActionWhenFireOneTime();
            }
#endif
        }

        //Aim

        if (!isReloading && !isKnifeAttack)
        {
#if UNITY_STANDALONE || UNITY_EDITOR

            if (Input.GetButtonDown("Fire2"))
                isAimingTrigger = true;
            if (Input.GetButtonUp("Fire2"))
                isAimingTrigger = false;

#elif UNITY_ANDROID || UNITY_IPHONE

            isAimingTrigger = mobile_IsAiming;

#endif
            if (isAimingTrigger && !isAiming && currentGun.CurrentAmmo > 0)
            {
                IsAim?.Invoke(true);
                isAiming = true;
            }
            if (!isAimingTrigger && isAiming)
            {
                IsAim?.Invoke(false);
                isAiming = false;
            }

        }

        //Change Fire Style when aiming
        currentGun.P_WeaponFireType = isAiming ? WeaponFireType.Continuous : WeaponFireType.Once;

#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetButtonDown("Reload"))
            isReloadTrigger = true;

        if (Input.GetButtonDown("KnifeAttack"))
            isKnifeTrigger = true;

#elif UNITY_ANDROID || UNITY_IPHONE 
        isReloadTrigger = mobile_IsReload;
        isKnifeTrigger = mobile_IsKnife;
#endif

        //Reload
        gameManager.P_UIController.SetActiveMobileReload(!isReloading && (currentGun.CurrentAmmo < currentGun.MaxAmmo));

        if (isReloadTrigger && !isReloading && currentGun.CurrentAmmo < currentGun.MaxAmmo && !isKnifeAttack)
        {
            mobile_IsReload = false;
            isReloadTrigger = false;

            if (c_autoReload != null)
                StopCoroutine(c_autoReload);

            Reload();
        }

        //Knife Attack
        if (isKnifeTrigger && !isKnifeAttack)
        {
            mobile_IsKnife = false;
            isKnifeTrigger = false;

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

        gameManager.P_UIController.SetActiveMobileSwapWeapon(true);
        gameManager.P_UIController.SetActiveMobileKnifeAttack(true);
        gameManager.P_UIController.SetActiveMobileReload(true);
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

        gameManager.P_UIController.SetActiveMobileReload(false);
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
        gameManager.P_UIController.SetActiveMobileKnifeAttack(true);
    }

    void TakenItem(Item _item)
    {
        if (_item.itemType == ItemType.RifleAmmo)
        {
            gameManager.P_WeaponController.ResetWeaponAmmo(0, _item.amount);
        }
        else if (_item.itemType == ItemType.HandgunAmmo)
        {
            gameManager.P_WeaponController.ResetWeaponAmmo(1, _item.amount);
        }
        else
        {
            PlayerController.instance.UpdatePlayerHealth(_item.amount);
        }

        UpdateTotalAmmo?.Invoke(currentGun.TotalAmmo.ToString());
        PlayerController.instance.PlayItemTakenShound();
    }

#if UNITY_ANDROID || UNITY_IPHONE
    public void SetFire(bool isFire)
    {
        mobile_IsFire = isFire;
    }

    public void SetAiming(bool isAim)
    {
        mobile_IsAiming = isAim;
    }

    public void SetKnifeAttack(bool isKnifeAttack)
    {
        mobile_IsKnife = isKnifeAttack;
        gameManager.P_UIController.SetActiveMobileKnifeAttack(false);
    }

    public void SetReload(bool isReload)
    {
        mobile_IsReload = isReload;
        gameManager.P_UIController.SetActiveMobileReload(false);
    }

    public void SetSwapWeapon(bool isSwap)
    {
        mobile_IsSwapWeapon = isSwap;

        gameManager.P_UIController.SetActiveMobileSwapWeapon(false);
        gameManager.P_UIController.SetActiveMobileKnifeAttack(false);
        gameManager.P_UIController.SetActiveMobileReload(false);
    }
#endif
}
