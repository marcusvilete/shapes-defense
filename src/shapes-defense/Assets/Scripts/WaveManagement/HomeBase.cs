using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour
{

    [SerializeField]
    private float startingHealth;
    [SerializeField]
    private float currentHealth;

    public event Action<HealthChangedData> OnDamageTaken;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Enemy")
        {
            var eventData = new HealthChangedData
            {
                maxHealth = startingHealth,
                oldHealth = currentHealth
            };
            var enemy = c.GetComponent<Enemy>();
            currentHealth -= enemy.damage;
            eventData.newHealth = currentHealth;
            
            OnDamageTaken?.Invoke(eventData);
            enemy.TakeDamage(float.MaxValue);

            if (currentHealth <= 0f)
            {
                //TODO: we lost =/
            }

        }
    }

    public HealthChangedData GetHealthData()
    {
        return new HealthChangedData
        {
            maxHealth = startingHealth,
            oldHealth = currentHealth,
            newHealth = currentHealth
            
        };
    }
}
