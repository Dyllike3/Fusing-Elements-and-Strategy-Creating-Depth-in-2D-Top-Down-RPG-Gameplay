using UnityEngine;
using Pathfinding;

public class RushEnemy : EnemyController
{
    public float accelerationRate = 0.5f; // Speed increase per second
    private AIPath aiPath;

    private void Start()
    {
        aiPath = GetComponent<AIPath>(); // Reference to A* Pathfinding movement script
        if (aiPath == null)
        {
            Debug.LogError("AIPath component not found!");
        }
    }

    private void Update()
    {
        if (aiPath != null)
        {
            aiPath.maxSpeed += accelerationRate * Time.deltaTime; // Gradually increase speed
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name} collided with the player and dealt {damage} damage.");
            PlayerController.Instance.TakeDamage(damage);
        }
    }
}

