using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehavior : MonoBehaviour
{
    [SerializeField] int damage;

    bool isAttack;

    private void OnEnable()
    {
        FireController.IsKnifeAttack += KnifeAttack;
    }

    private void OnDisable()
    {
        FireController.IsKnifeAttack -= KnifeAttack;
    }

    void KnifeAttack()
    {
        isAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isAttack)
        {
            Debug.Log("damage: " + damage);
            other.gameObject.GetComponent<EnemyController>().Damaged(damage);

            isAttack = false;
        }
    }
}
