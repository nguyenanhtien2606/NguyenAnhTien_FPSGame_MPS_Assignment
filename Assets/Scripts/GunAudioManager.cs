using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClipLst audioClipLst;

    private void OnEnable()
    {
        AnimationController.PlayShootSound += PlayShootAudio;
        AnimationController.PlayReloadOutOfAmmoSound += PlayReloadOutOfAmmoAudio;
        AnimationController.PlayReloadLeftAmmoSound += PlayReloadLeftAmmoAudio;
        AnimationController.PlayHolsterSound += PlayHolsterAudio;
        AnimationController.StopReloadAmmoSound += StopAudio;
    }

    private void OnDisable()
    {
        AnimationController.PlayShootSound -= PlayShootAudio;
        AnimationController.PlayReloadOutOfAmmoSound -= PlayReloadOutOfAmmoAudio;
        AnimationController.PlayReloadLeftAmmoSound -= PlayReloadLeftAmmoAudio;
        AnimationController.PlayHolsterSound -= PlayHolsterAudio;
        AnimationController.StopReloadAmmoSound -= StopAudio;
    }

    void PlayShootAudio()
    {
        audioSource.PlayOneShot(audioClipLst.shoot);
    }

    void PlayAimAudio()
    {
        audioSource.PlayOneShot(audioClipLst.aim);
    }

    void PlayHolsterAudio()
    {
        audioSource.PlayOneShot(audioClipLst.holster);
    }

    void PlayTakeOutAudio()
    {
        audioSource.PlayOneShot(audioClipLst.takeOut);
    }

    void PlayReloadOutOfAmmoAudio()
    {
        audioSource.PlayOneShot(audioClipLst.reloading_OutOfAmmo);
    }

    void PlayReloadLeftAmmoAudio()
    {
        audioSource.PlayOneShot(audioClipLst.reloading_LeftAmmo);
    }

    void PlayThudAudio()
    {
        audioSource.PlayOneShot(audioClipLst.thud);
    }

    void StopAudio()
    {
        audioSource.Stop();
    }
}


[Serializable]
public class AudioClipLst
{
    public AudioClip shoot;
    public AudioClip aim;
    public AudioClip holster;
    public AudioClip takeOut;
    public AudioClip reloading_OutOfAmmo;
    public AudioClip reloading_LeftAmmo;
    public AudioClip thud;
}