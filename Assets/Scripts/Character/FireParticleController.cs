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
    public static event Action<WeaponType, Transform> SpawnBullet;
    public static event Action<WeaponType, Transform> SpawnCasing;
    #endregion

    private void OnEnable()
    {
        FireController.IsFire += ActiveGunParticle;
    }

    private void OnDisable()
    {
        FireController.IsFire -= ActiveGunParticle;
    }

    void ActiveGunParticle(bool isActive, Gun current)
    {
        ActiveMuzzleParcticle(isActive);

        #region Observer
        if (isActive)
        {
            SpawnBullet?.Invoke(current.P_WeaponType, bulletSpawnPointObj.transform);
            SpawnCasing?.Invoke(current.P_WeaponType, casingSpawnPointObj.transform);
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
