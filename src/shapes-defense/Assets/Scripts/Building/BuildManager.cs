using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    BluePrintController bluePrintPrefab;
    BluePrintController bluePrintObject;

    public void SetBluePrint(BluePrintController prefab)
    {
        if (bluePrintObject != null)
        {
            Destroy(bluePrintObject.gameObject);
        }

        bluePrintPrefab = prefab;
        bluePrintObject = Instantiate(bluePrintPrefab);
    }
}