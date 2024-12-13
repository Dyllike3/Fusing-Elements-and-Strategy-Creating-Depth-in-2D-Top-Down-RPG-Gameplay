using UnityEngine;

public class SelfExplodedEnemy : EnemyController
{
    public float explosionRadius = 2f; // Radius of the explosion
    public float explosionDamage = 30f; // Damage dealt on explosion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name} collided with the player and triggered an explosion.");
            Explode();
        }
    }

    public override void Die()
    {
        Explode();
        base.Die(); // Call the base `Die` method to destroy the enemy
    }

    private void Explode()
    {
        // Find all objects within the explosion radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} exploded and dealt {explosionDamage} damage to the player.");
                PlayerController.Instance.TakeDamage(explosionDamage);
            }
        }

        Debug.Log($"{gameObject.name} exploded!");
        base.Die();
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

