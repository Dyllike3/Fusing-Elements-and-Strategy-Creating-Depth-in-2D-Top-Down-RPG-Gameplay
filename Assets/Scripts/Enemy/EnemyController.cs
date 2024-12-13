using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 50f; // Enemy's health
    public float damage = 10f; // Damage dealt to the player on collision
    public float knockBackTime = 0.5f;

    private float knockBackCounter;
    private bool isKnockedBack = false;

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
            isKnockedBack = false;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        Debug.Log($"{gameObject.name} took {damageToTake} damage!");

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damageToTake, bool shouldKnockback)
    {
        TakeDamage(damageToTake);

        if (shouldKnockback)
        {
            isKnockedBack = true;
            knockBackCounter = knockBackTime;

            rigidBody2D.velocity = Vector2.zero;
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}

