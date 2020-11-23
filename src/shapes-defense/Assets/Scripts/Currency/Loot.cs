using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int currencyDropAmount = 1;
    Enemy health;

    void Start()
    {
        health = GetComponent<Enemy>();
        health.OnDeath += OnDeath;
    }

    private void OnDeath(Enemy h)
    {
        CurrencyManager.Instance.IncreaseBalance(currencyDropAmount);
    }
}
