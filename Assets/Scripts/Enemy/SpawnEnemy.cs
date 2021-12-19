using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyModel;
    [SerializeField] int delaySpawnTime;
    [SerializeField] int maxSpawn;

    [Header("Enemy Setting")]
    [SerializeField] int enemyHealth = 100;
    [SerializeField] int enemyDamage = 10;
    [SerializeField] int enemyPoint = 10;

    float randomOffset = 10;

    public int SpawnCount { get; set; }

    private void Start()
    {
        StartCoroutine(C_SpawnEnemy());
    }

    IEnumerator C_SpawnEnemy()
    {
        yield return new WaitForSeconds(delaySpawnTime);

        GameObject enemy = Instantiate(enemyModel, transform.position, Quaternion.identity);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.EnemyHealth = enemyHealth;
        enemyController.EnemyDamage = enemyDamage;
        enemyController.EnemyPoint = enemyPoint;

        enemyController.ParentSpawnEnemy = this;

        enemyController.OriginPos = new Vector3(transform.position.x + Random.Range(-randomOffset, randomOffset), transform.position.y + Random.Range(-randomOffset, randomOffset), transform.position.z + Random.Range(-randomOffset, randomOffset));

        enemyController.Target = enemyController.OriginPos;
        enemyController.IsReturnOriginPos = true;

        SpawnCount += 1;

        if (SpawnCount < maxSpawn)
            StartCoroutine(C_SpawnEnemy());
    }

    public void CheckChildLst()
    {
        if (SpawnCount <= 0)
        {
            StartCoroutine(C_SpawnEnemy());
        }
    }
}
