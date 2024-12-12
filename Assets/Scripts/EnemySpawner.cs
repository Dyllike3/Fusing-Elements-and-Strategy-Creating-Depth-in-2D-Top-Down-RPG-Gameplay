using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of different enemy prefabs
    public Transform[] spawnPoints; // Spawn points in the area

    private List<EnemyController> activeEnemies = new List<EnemyController>();
    public bool isActive = false; // Determine if this spawner is active

    public float spawnRate = 5f; // Time in seconds between spawns
    private float spawnTimer;

    public void SpawnEnemy()
    {
        if (!isActive) return; // Prevent spawning if spawner is inactive

        if (enemyPrefabs.Count == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs or spawn points assigned!");
            return;
        }

        // Randomly select a spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Randomly select an enemy prefab
        GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // Instantiate enemy
        GameObject enemyObj = Instantiate(selectedEnemyPrefab, spawnPoint.position, Quaternion.identity);

        // Get EnemyController component
        EnemyController enemyController = enemyObj.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            // Add to the active enemies list
            activeEnemies.Add(enemyController);
        }
    }

    private void Update()
    {
        if (!isActive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnRate;
        }
    }

    public void ClearEnemies()
    {
        Debug.Log("888");
        if (!isActive) return;
        Debug.Log("999");
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        activeEnemies.Clear();
    }
}
