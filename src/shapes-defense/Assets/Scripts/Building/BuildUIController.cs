using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIController : MonoBehaviour
{
    public BluePrintController HitScanTurretBluePrint;
    public Text HitScanTurretPrice;
    public Button HitScanTurretBuyButton;

    public BluePrintController MissileTurretBluePrint;
    public Text MissileTurretPrice;
    public Button MissileTurreBuyButton;




    void Awake()
    {
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
        HitScanTurretPrice.text = $"$ {HitScanTurretBluePrint.price}";
        MissileTurretPrice.text = $"$ {MissileTurretBluePrint.price}";
    }

    private void OnCurrencyChanged(CurrencyChangedData data)
    {
        if (data.newBalance < HitScanTurretBluePrint.price)
        {
            HitScanTurretBuyButton.interactable = false;
        }
        else
        {
            HitScanTurretBuyButton.interactable = true;
        }


        if (data.newBalance < MissileTurretBluePrint.price)
        {
            MissileTurreBuyButton.interactable = false;
        }
        else
        {
            MissileTurreBuyButton.interactable = true;
        }
    }

    public void HitScanTurretClick()
    {
        BuildManager.Instance.SetBluePrint(HitScanTurretBluePrint);
    }

    public void MissileTurretClick()
    {
        BuildManager.Instance.SetBluePrint(MissileTurretBluePrint);
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
        }
    }
}
