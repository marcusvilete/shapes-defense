using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int Balance { get; set; }

    public event Action<CurrencyChangedData> OnCurrencyChanged;

    public void Initialize(int balance)
    {
        Balance = 0;
        ChangeBalance(balance);
    }

    public void IncreaseBalance(int amount)
    {
        ChangeBalance(amount);
    }

    public bool TrySpend(int amount)
    {
        if (Balance >= amount && amount != 0)
        {
            ChangeBalance(-amount);
            return true;
        }
        return false;
    }

    private void ChangeBalance(int amount)
    {
        if (amount != 0)
        {
            CurrencyChangedData currencyData = new CurrencyChangedData();
            currencyData.oldBalance = Balance;
            Balance += amount;
            currencyData.newBalance = Balance;
            OnCurrencyChanged?.Invoke(currencyData);
            Debug.Log($"Current balance: {Balance}");
        }
    }
}
