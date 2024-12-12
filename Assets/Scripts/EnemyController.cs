using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health; // Enemy's health
    public float knockBackTime = 0.5f; // Duration of knockback effect
    private float knockBackCounter; // Timer for knockback
    private bool isKnockedBack = false; // Track if the enemy is in knockback state

    public event Action<EnemyController> OnDeath;

    private Rigidbody2D rigidBody2D;

    private Vector3 PositiveVectorOne = new Vector3(1, 1, 1);
    private Vector3 NegativeVectorOne = new Vector3(-1, 1, 1);

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnockedBack)
        {
            HandleKnockback();
        }
        else
        {
            FlipBasedOnDirection();
        }
    }

    private void FlipBasedOnDirection()
    {
        if (rigidBody2D.velocity.x < 0)
        {
            transform.localScale = PositiveVectorOne;
        }
        else if (rigidBody2D.velocity.x > 0)
        {
            transform.localScale = NegativeVectorOne;
        }
    }

    private void HandleKnockback()
    {
        knockBackCounter -= Time.deltaTime;

        if (knockBackCounter <= 0)
        {
            isKnockedBack = false; // Exit knockback state
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        Debug.Log($"{gameObject.name} took {damageToTake} damage!");

        if (health <= 0)
        {
            Die(); // Destroy the enemy when health is 0
        }
    }

    public void TakeDamage(float damageToTake, bool shouldKnockback)
    {
        TakeDamage(damageToTake);

        if (shouldKnockback)
        {
            isKnockedBack = true; // Enter knockback state
            knockBackCounter = knockBackTime;

            // Apply knockback force (optional customization)
            rigidBody2D.velocity = Vector2.zero; // Stop current movement
        }
    }

    private void Die()
    {
        // Trigger the OnDeath event
        OnDeath?.Invoke(this);

        Destroy(gameObject); // Destroy the enemy
    }
}

