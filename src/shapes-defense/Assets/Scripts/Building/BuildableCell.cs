using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableCell : MonoBehaviour
{
    public bool Occupied;
    GameObject turret;

    public bool TryBuild(GameObject prefab)
    {
        if (!Occupied)
        {

            turret = Instantiate(prefab, transform);
            Vector3 pos = turret.transform.localPosition;
            turret.transform.localPosition = pos;
            Occupied = true;
            return true;
        }
        return false;
    }

    public bool TryClear()
    {
        if (Occupied)
        {
            Destroy(turret);
            Occupied = false;
            return true;
        }
        return false;
    }
}
