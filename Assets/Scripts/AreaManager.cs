using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public List<EnemySpawner> enemySpawners; // List of all enemy spawners in this area
    public GameObject areaEnvironment; // Environment objects (e.g., tilemaps, walls)
    public bool isActive = true; // Tracks if the area is currently active

    private void Start()
    {
        // Initially deactivate the area
        DeactivateArea();
    }

    public void ActivateArea()
    {
        if (isActive) return; // Prevent double activation

        isActive = true;
        areaEnvironment.SetActive(true); // Enable the area environment
        Debug.Log("333");
        // Start spawning enemies
        foreach (var spawner in enemySpawners)
        {
            spawner.gameObject.SetActive(true); // Activate the spawner
            spawner.SpawnEnemy(); // Start spawning
        }

        Debug.Log($"Area {gameObject.name} activated!");
    }

    public void DeactivateArea()
    {
        if (!isActive) return; // Prevent double deactivation

        isActive = false;
        areaEnvironment.SetActive(false); // Disable the area environment

        // Clear all active enemies in the area
        foreach (var spawner in enemySpawners)
        {
            spawner.ClearEnemies();
            spawner.gameObject.SetActive(false); // Deactivate the spawner
        }

        Debug.Log($"Area {gameObject.name} deactivated!");
    }
}
