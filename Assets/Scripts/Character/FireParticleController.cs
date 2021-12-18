using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleController : MonoBehaviour
{
    [Header("Particle")]
    [SerializeField] ParticleSystem muzzleflash;
    [SerializeField] ParticleSystem spark;
    [SerializeField] Light muzzleLight;

    [Header("Bullet")]
    [SerializeField] GameObject bulletSpawnPointObj;

    [Header("Casing")]
    [SerializeField] GameObject casingSpawnPointObj;

    #region Observer
    public static event Action<Gun, Transform> SpawnBullet;
    public static event Action<Gun, Transform> SpawnCasing;
    #endregion

    private void OnEnable()
    {
        FireController.IsFire += ActiveGunParticle;
    }

    private void OnDisable()
    {
        FireController.IsFire -= ActiveGunParticle;
    }

    void ActiveGunParticle(bool isActive, Gun currentGun)
    {
        ActiveMuzzleParcticle(isActive);

        #region Observer
        if (isActive)
        {
            SpawnBullet?.Invoke(currentGun, bulletSpawnPointObj.transform);
            SpawnCasing?.Invoke(currentGun, casingSpawnPointObj.transform);
        }
        #endregion
    }

    void ActiveMuzzleParcticle(bool isActive)
    {
        muzzleLight.enabled = isActive;

        if (isActive)
        {
            muzzleflash.Play();
            spark.Play();
        }
        else
        {
            muzzleflash.Stop();
            spark.Stop();
        }
    }
}
