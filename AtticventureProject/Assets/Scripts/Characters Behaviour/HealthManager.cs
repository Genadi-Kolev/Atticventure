using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public AudioSource hitSFX;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;
        if (currentHealth <= 0) 
            Die();

        hitSFX.Play();

        print(("damage taken [0] [1]", maxHealth, currentHealth));
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
