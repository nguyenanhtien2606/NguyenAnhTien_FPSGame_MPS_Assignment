using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAndCasingCollection : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10;

    [Space]
    [SerializeField] List<GameObject> bigBulletLst;
    [SerializeField] List<GameObject> bigCasingLst;
    [SerializeField] List<GameObject> smallBulletLst;
    [SerializeField] List<GameObject> smallCasingLst;
    [SerializeField] List<GameObject> bulletImpactParticle;

    int currentBulletIndex = 0;
    int currentCasingIndex = 0;
    int currentImpactParticleIndex = 0;

    public List<GameObject> BulletImpactParticle
    {
        get { return bulletImpactParticle; }
        private set { bulletImpactParticle = value; }
    }

    private void OnEnable()
    {
        FireParticleController.SpawnBullet += SpawnBullet;
        FireParticleController.SpawnCasing += SpawnCasing;
    }

    private void OnDisable()
    {
        FireParticleController.SpawnBullet -= SpawnBullet;
        FireParticleController.SpawnCasing -= SpawnCasing;
    }
    void SpawnBullet(WeaponType weaponType, Transform spawnPos)
    {
        Ray centerPoint = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        List<GameObject> bulletGOs = (weaponType == WeaponType.Rifle) ? bigBulletLst : smallBulletLst;

        bulletGOs[currentBulletIndex].SetActive(true);
        bulletGOs[currentBulletIndex].transform.position = spawnPos.position;
        bulletGOs[currentBulletIndex].transform.rotation = spawnPos.rotation;

        //add force
        BulletBehavior bulletBehavior = bulletGOs[currentBulletIndex].GetComponent<BulletBehavior>();
        bulletBehavior.Rb.AddForce(centerPoint.direction * bulletSpeed, ForceMode.VelocityChange);
        bulletBehavior.ImpactParticle = bulletImpactParticle[currentImpactParticleIndex];

        if (currentBulletIndex < bulletGOs.Count - 1)
        {
            currentBulletIndex++;
        }
        else
        {
            currentBulletIndex = 0;
        }

        if (currentImpactParticleIndex < bulletImpactParticle.Count - 1)
        {
            currentImpactParticleIndex++;
        }
        else
        {
            currentImpactParticleIndex = 0;
        }
    }

    void SpawnCasing(WeaponType weaponType, Transform spawnPos)
    {
        List<GameObject> casingGOs = (weaponType == WeaponType.Rifle) ? bigCasingLst : smallCasingLst;

        casingGOs[currentCasingIndex].SetActive(true);
        casingGOs[currentCasingIndex].transform.position = spawnPos.position;
        casingGOs[currentCasingIndex].transform.rotation = spawnPos.rotation;

        //add force
        CasingBehavior casingBehavior = casingGOs[currentCasingIndex].GetComponent<CasingBehavior>();
        casingBehavior.Rb.AddForce(spawnPos.right * 2, ForceMode.Impulse);

        if (currentCasingIndex < casingGOs.Count - 1)
        {
            currentCasingIndex++;
        }
        else
        {
            currentCasingIndex = 0;
        }
    }
}
