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
        AnimationController.PlayReloadSound += PlayReloadAudio;
    }

    private void OnDisable()
    {
        AnimationController.PlayShootSound -= PlayShootAudio;
        AnimationController.PlayReloadSound -= PlayReloadAudio;
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

    void PlayReloadAudio()
    {
        audioSource.PlayOneShot(audioClipLst.reloading);
    }
}


[Serializable]
public class AudioClipLst
{
    public AudioClip shoot;
    public AudioClip aim;
    public AudioClip holster;
    public AudioClip takeOut;
    public AudioClip reloading;
}