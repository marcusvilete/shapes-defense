using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintController : MonoBehaviour
{

    public LayerMask buildableLayer;
    public GameObject toBuild;
    public int price;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if its in a buildable area, snap to it
        if (Physics.Raycast(r, out hit, float.MaxValue, buildableLayer.value))
        {
            var buildable = hit.collider.GetComponent<BuildableGrid>();
            //transform.position = buildable.SnapToGrid(hit.transform.position, new Vector2Int(1,1));
            transform.position = buildable.GetNearestPointOnGrid(hit.point);

            //HandleInstantiate
            if (Input.GetMouseButtonDown(0))
            {
                var buidableCell = buildable.GetNearestBuildableCell(transform.position);
                if (!buidableCell.Occupied)
                {
                    if (CurrencyManager.Instance.TrySpend(price))
                    {
                        buidableCell.TryBuild(toBuild);
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(r, out hit))
            {
                //if not buildable, check for any collision and update the position
                Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                transform.position = position;
            }
        }

    }
}
