using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeBaseUIController : MonoBehaviour
{

    public Text healthValueText;
    private HomeBase homeBase;

    void Start()
    {
        homeBase = FindObjectOfType<HomeBase>();
        homeBase.OnDamageTaken += HomeBase_OnDamageTaken;

        var data = homeBase.GetHealthData();
        updateHealthUI(data.newHealth, data.maxHealth);
    }

    private void HomeBase_OnDamageTaken(HealthChangedData data)
    {
        updateHealthUI(data.newHealth, data.maxHealth);
    }

    private void updateHealthUI(float curreantHealth, float maxHealth)
    {
        healthValueText.text = $"{curreantHealth}/{maxHealth}";
    }
}
