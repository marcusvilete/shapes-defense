using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUIController : MonoBehaviour
{
    public Text currencyValue;

    void Awake()
    {
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
    }

    private void OnCurrencyChanged(CurrencyChangedData data)
    {
        currencyValue.text = $"$ {data.newBalance}";
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
        }
    }
}
