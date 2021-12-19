using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] List<Animator> animObjLst;

    int currentActiveAnim = -1;

    #region Observer Of Gun Sound
    public static event Action PlayShootSound;
    public static event Action PlayAimSound;
    public static event Action PlayHolsterSound;
    public static event Action PlayTakeOutSound;
    public static event Action PlayReloadOutOfAmmoSound;
    public static event Action PlayReloadLeftAmmoSound;
    public static event Action StopReloadAmmoSound;
    #endregion

    private void OnEnable()
    {
        MovementController.IsRunning += RunningAnim;
        MovementController.IsWaking += WalkingAnim;

        FireController.ChangeAnimatorRuntime += SetCurrentAnimator;
        FireController.IsFire += FireAnim;
        FireController.IsAim += AimAnim;
        FireController.IsReloadOutOfAmmo += ReloadOutOfAmmoAnim;
        FireController.IsReloadLeftAmmo += ReloadLeftAmmoAnim;
        FireController.IsHolster += HolsterAnim;
        FireController.IsKnifeAttack += KnifeAttack;

        CheckReloadFinish.IsReloadFinish += CheckReloadActionFinish;
    }

    private void OnDisable()
    {
        MovementController.IsRunning -= RunningAnim;
        MovementController.IsWaking -= WalkingAnim;

        FireController.ChangeAnimatorRuntime -= SetCurrentAnimator;
        FireController.IsFire -= FireAnim;
        FireController.IsAim -= AimAnim;
        FireController.IsReloadOutOfAmmo -= ReloadOutOfAmmoAnim;
        FireController.IsReloadLeftAmmo -= ReloadLeftAmmoAnim;
        FireController.IsHolster -= HolsterAnim;
        FireController.IsKnifeAttack -= KnifeAttack;

        CheckReloadFinish.IsReloadFinish -= CheckReloadActionFinish;
    }

    void RunningAnim(bool isRun)
    {
        animObjLst[currentActiveAnim].SetBool("IsRunning", isRun);
    }
    void WalkingAnim(bool isWalk)
    {
        animObjLst[currentActiveAnim].SetBool("IsWalking", isWalk);
    }

    void FireAnim(bool isFire, Gun currentGun)
    {
        if (currentGun.P_WeaponFireType == WeaponFireType.Once && isFire)
        {
            animObjLst[currentActiveAnim].SetTrigger("IsFireOnce");
        }
        else
        {
            animObjLst[currentActiveAnim].SetBool("IsFireContinuous", isFire);
        }

        if (isFire)
            PlayShootSound?.Invoke();
    }

    void AimAnim(bool isAim)
    {
        animObjLst[currentActiveAnim].SetBool("IsAim", isAim);
    }

    void ReloadOutOfAmmoAnim()
    {
        animObjLst[currentActiveAnim].SetTrigger("IsReloadOutOfAmmo");
        PlayReloadOutOfAmmoSound?.Invoke();
    }

    void ReloadLeftAmmoAnim()
    {
        animObjLst[currentActiveAnim].SetTrigger("IsReloadLeftAmmo");
        PlayReloadLeftAmmoSound?.Invoke();
    }

    void CheckReloadActionFinish(bool isFinish)
    {
        if (!isFinish)
            StopReloadAmmoSound?.Invoke();
    }

    void HolsterAnim()
    {
        animObjLst[currentActiveAnim].SetTrigger("IsHolster");
        PlayHolsterSound?.Invoke();
    }

    void KnifeAttack()
    {
        //rnadom attack type
        animObjLst[currentActiveAnim].SetInteger("KnifeRanNum", UnityEngine.Random.Range(0, 2));
        animObjLst[currentActiveAnim].SetTrigger("IsKnife");
    }

    public void SetCurrentAnimator(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.HandGun:
                ActiveAnimObj(1); //Pistol
                break;

            default:
                ActiveAnimObj(0); //Rifle
                break;
        }
    }

    void ActiveAnimObj(int index)
    {
        foreach (var animObj in animObjLst)
        {
            animObj.gameObject.SetActive(false);
        }

        animObjLst[index].gameObject.SetActive(true);
        currentActiveAnim = index;
    }
}
