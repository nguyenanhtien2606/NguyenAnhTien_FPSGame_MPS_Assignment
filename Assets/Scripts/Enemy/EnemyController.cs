using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int enemyHealth = 100;

    public int EnemyHealth
    {
        get { return enemyHealth; }
        private set { enemyHealth = value; }
    }

    public void Damaged(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth > 0)
        {
            Debug.Log(string.Format("{0} heath reamin: {1}", gameObject.name, enemyHealth));
        }
        else
        {
            //die
            Destroy(gameObject);
        }
    }
}
