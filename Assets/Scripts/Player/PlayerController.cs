using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float moveSpeed = 1f;

    public float maxHealth = 100f; // Player's maximum health
    private float currentHealth;  // Player's current health

    public float invincibilityDuration = 1f; // Duration of invincibility after taking damage
    private float lastDamageTime = -10f; // Tracks when the player last took damage

    private Rigidbody2D rb; // Reference to Rigidbody2D
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Initialize current health to max health at the start of the game
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Get player input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize the input to avoid faster diagonal movement
        moveInput = moveInput.normalized * moveSpeed;

        // Move the player using Rigidbody2D
        rb.velocity = moveInput;
    }

    public void TakeDamage(float damage)
    {
        if (Time.time - lastDamageTime > invincibilityDuration)
        {
            currentHealth -= damage; // Subtract damage from current health
            lastDamageTime = Time.time; // Reset the cooldown timer
            Debug.Log($"Player took {damage} damage! Remaining health: {currentHealth}/{maxHealth}");

            StartCoroutine(InvincibilityEffect()); // Trigger the invincibility effect

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            Debug.Log("Player is invincible!");
        }
    }

    private IEnumerator InvincibilityEffect()
    {
        // Set player opacity to 50% (alpha = 0.5)
        SetPlayerOpacity(0.5f);

        // Wait for the invincibility duration
        yield return new WaitForSeconds(invincibilityDuration);

        // Reset player opacity to fully visible (alpha = 1)
        SetPlayerOpacity(1f);
    }

    private void SetPlayerOpacity(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha; // Set the alpha value
            spriteRenderer.color = color; // Apply the color back to the SpriteRenderer
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add death logic here (e.g., restart level, game over screen)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an enemy
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Collided with enemy: {collision.name}");
            // Get the damage value from the enemy (if it has a script)
            EnemyController enemyController = collision.GetComponent<EnemyController>();
            TakeDamage(enemyController.damage);
        }
    }

    public float GetHealthPercentage()
    {
        // Return the current health as a percentage of max health
        return currentHealth / maxHealth;
    }
}


