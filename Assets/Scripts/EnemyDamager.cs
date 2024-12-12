using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                //print("Zone Weapon Take Damage On Enemy");
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
                    enemy.TakeDamage(damageAmount);
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
                        enemiesInRange[i].TakeDamage(damageAmount, shouldKnockback);
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
}
