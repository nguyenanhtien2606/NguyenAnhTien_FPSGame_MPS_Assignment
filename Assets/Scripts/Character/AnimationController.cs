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
    public static event Action PlayReloadSound;
    #endregion

    private void OnEnable()
    {
        MovementController.IsRunning += RunningAnim;
        MovementController.IsWaking += WalkingAnim;

        FireController.ChangeAnimatorRuntime += SetCurrentAnimator;
        FireController.IsFire += FireAnim;
        FireController.IsReload += ReloadAnim;
    }

    private void OnDisable()
    {
        MovementController.IsRunning -= RunningAnim;
        MovementController.IsWaking -= WalkingAnim;

        FireController.ChangeAnimatorRuntime -= SetCurrentAnimator;
        FireController.IsFire -= FireAnim;
        FireController.IsReload -= ReloadAnim;
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

    void ReloadAnim()
    {
        animObjLst[currentActiveAnim].SetTrigger("IsReload");
        PlayReloadSound?.Invoke();
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
