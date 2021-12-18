using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactEffect : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] float durationAppear = 3;
    [SerializeField] List<AudioClip> impactAudioClips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayImpactEffectAudio()
    {
        audioSource.PlayOneShot(impactAudioClips[Random.Range(0, impactAudioClips.Count)]);
        StartCoroutine(C_Disable());
    }

    IEnumerator C_Disable()
    {
        yield return new WaitForSeconds(durationAppear);
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
}
