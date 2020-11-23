using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{

    public float buyingCost;

    public float SellingCost { get { return buyingCost / 2; } }
    
    protected virtual void OnDestroy()
    {
        //if this was built by build manager
        this.GetComponentInParent<BuildableCell>()?.TryClear();
    }
}
