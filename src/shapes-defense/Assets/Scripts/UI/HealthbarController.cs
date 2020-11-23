using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private Enemy health;

    private void Start()
    {
        SetHealth(health.CurrentHealth, health.MaxHealth);
        health.OnHealthChanged += Health_healthChanged;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
    }

    private void Health_healthChanged(HealthChangedData data)
    {
        SetHealth(data.newHealth, data.maxHealth);
        
    }

    public void SetHealth(float healthPercentage)
    {
        healthBarImage.fillAmount = healthPercentage;
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        SetHealth(currentHealth / maxHealth);
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= Health_healthChanged;
    }

}
