using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Projectile speed
    public float lifeTime = 5f; // Duration before destruction
    public Transform target; // Target for the projectile to follow

    private void Start()
    {
        // Automatically find the closest enemy if no target is set
        if (target == null)
        {
            EnemyController closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                target = closestEnemy.transform;
            }
        }
    }

    private void Update()
    {
        // Move toward the target
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optional: Rotate the projectile to face its target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Decrease the lifetime and destroy the projectile if expired
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private EnemyController FindClosestEnemy()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        EnemyController closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
