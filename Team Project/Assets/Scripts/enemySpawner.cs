using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float spawnInterval;
    [SerializeField] int maxEnemies;
    //[SerializeField] Transform[] spawnPoints;

    [SerializeField] int currentEnemyCount;
    [SerializeField] float spawnRadius;
    //[SerializeField] int totalEnemiesSpawned;
  
    private float spawnTimer;
    void Update()
    {
        //if (totalEnemiesSpawned < maxEnemies)
        //{
            if (currentEnemyCount < maxEnemies)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    spawnEnemy();
                    spawnTimer = spawnInterval;
                }
            }
        //}
        //else if(totalEnemiesSpawned >= maxEnemies)
        //{
        //    return;
        //}
    }
    void spawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefab.Length);
        GameObject selectedEnemyPrefab = enemyPrefab[randomIndex];
        // Transform ranSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 randomSpawnPosition = transform.position + Random.insideUnitSphere * spawnRadius; ;
        randomSpawnPosition.y = 0;

        GameObject enemy = Instantiate(selectedEnemyPrefab, randomSpawnPosition, Quaternion.identity);
       
        currentEnemyCount++;
       // totalEnemiesSpawned++;
    }
    public void EnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
