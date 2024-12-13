using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float damageAmount;
    public string mainElement;

    public bool shouldKnockback;

    public bool damageOverTime;
    public float timeBetweenDamage;
    private float damageCounter;
    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    public float lifeTime = 5f;
    public bool destroyOnContact;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageOverTime)
        {
            if (collision.CompareTag("Enemy"))
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }
        else
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    float adjustedDamage = AdjustDamageBasedOnArea();
                    enemy.TakeDamage(adjustedDamage);
                }

                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (damageOverTime)
        {
            if (collision.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
        if (damageOverTime)
        {
            damageCounter -= Time.deltaTime;

            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;

                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] != null)
                    {
                        float adjustedDamage = AdjustDamageBasedOnArea();
                        enemiesInRange[i].TakeDamage(adjustedDamage, shouldKnockback);
                    }
                    else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private float AdjustDamageBasedOnArea()
    {
        // Find the active area
        AreaManager activeArea = FindObjectOfType<AreaManager>();

        if (activeArea == null)
        {
            Debug.LogWarning("No active AreaManager found. Using default damage.");
            return damageAmount; // Return default damage if no active area is found
        }

        string areaElement = activeArea.areaElement;

        // Adjust damage based on element relationship
        if (mainElement == areaElement)
        {
            Debug.Log("Spell element matches the area element. Increasing damage by 25%.");
            return damageAmount * 1.25f; // Increase damage by 25%
        }
        else if (IsConflict(mainElement, areaElement))
        {
            Debug.Log("Spell element conflicts with the area element. Decreasing damage by 25%.");
            return damageAmount * 0.75f; // Decrease damage by 25%
        }

        return damageAmount; // Neutral element, no adjustment
    }

    private bool IsConflict(string elementA, string elementB)
    {
        Dictionary<string, string[]> conflicts = new Dictionary<string, string[]>
        {
            { "Metal", new string[] { "Wood", "Fire" } },
            { "Wood", new string[] { "Earth", "Metal" } },
            { "Water", new string[] { "Fire", "Earth" } },
            { "Fire", new string[] { "Metal", "Water" } },
            { "Earth", new string[] { "Wood", "Water" } }
        };

        if (conflicts.ContainsKey(elementA))
        {
            return System.Array.Exists(conflicts[elementA], conflict => conflict == elementB);
        }
        return false;
    }
}

