using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : BulletAndCasing
{
    public GameObject ImpactParticle
    {
        get;
        set;
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        //Spawn Bullet Imapct Particle
        ImpactParticle.SetActive(true);
        ImpactParticle.transform.position = contact.point;
        ImpactParticle.transform.forward = contact.normal;
        ImpactParticle.GetComponent<BulletImpactEffect>().PlayImpactEffectAudio();

        if (collision.transform.CompareTag("Enemy"))
        {
            Debug.Log("damage: " + CurrentGun.Damage);
            collision.gameObject.GetComponent<EnemyController>().Damaged(CurrentGun.Damage);
        }

        Disable();
    }
}
