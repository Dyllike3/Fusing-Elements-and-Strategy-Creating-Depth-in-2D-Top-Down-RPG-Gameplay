using UnityEngine;

public class NormalEnemy : EnemyController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name} collided with the player and dealt {damage} damage.");
            PlayerController.Instance.TakeDamage(damage);
        }
    }
}