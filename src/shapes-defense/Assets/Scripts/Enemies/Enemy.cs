using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float startingHealth;
    [SerializeField]
    private float maxHealth;


    public GameObject hitEffectPrefab;

    public float damage;

    public float CurrentHealth { get; protected set; }

    public float MaxHealth { get { return maxHealth; } }
    public bool IsDead { get { return CurrentHealth <= 0f; } }

    public Vector3 Velocity { get; private set; }
    private Vector3 position;
    private Vector3 previousPosition;
    private IEnemyMover enemyMover;

    public event Action<Enemy> OnDeath;
    public event Action<HealthChangedData> OnHealthChanged;

    

    private void Awake()
    {
        CurrentHealth = startingHealth;
        position = transform.position;
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        position = transform.position;
        Velocity = (position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
    }

    public void Heal(float health)
    {
        ChangeHealth(health);
    }

    public void TakeDamage(float health)
    {
        ShowHitEffect();
        ChangeHealth(-health);
    }

    public void ChangeHealth(float health)
    {
        if (IsDead)
        {
            return;
        }

        var eventData = new HealthChangedData
        {
            maxHealth = maxHealth,
            oldHealth = CurrentHealth
        };

        CurrentHealth += health;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);
        eventData.newHealth = CurrentHealth;
        OnHealthChanged?.Invoke(eventData);

        if (IsDead)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }


    }

    private void ShowHitEffect()
    {

        Instantiate(hitEffectPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }

    public void SetWayPoints(Transform[] waypoints)
    {
        enemyMover = GetComponent<IEnemyMover>();
        enemyMover.SetWayPoints(waypoints);
    }

}
