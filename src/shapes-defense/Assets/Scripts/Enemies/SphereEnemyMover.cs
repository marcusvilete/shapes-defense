using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnemyMover : MonoBehaviour, IEnemyMover
{

    public Transform[] waypoints;
    public Transform toMove;
    private int wayPointIndex;
    public float speed;
    private bool isEnd;
    public Action endOfPathing;

    void Start()
    {
        if (waypoints?.Length == 0)
        {
            Debug.LogError("[SphereEnemyMover] Waypoints not set!");
            return;
        }

        toMove.LookAt(waypoints[wayPointIndex]);
    }

    void Update()
    {
        if (!isEnd)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        toMove.position = Vector3.MoveTowards(toMove.position, waypoints[wayPointIndex].position, speed * Time.deltaTime);
        
        //Check the distance between waypoint and enemy
        if (Vector3.Distance(toMove.position, waypoints[wayPointIndex].position) < 0.1f)
        {
            //Increment the waypoint index until the last one
            if (wayPointIndex < waypoints.Length - 1)
            {
                wayPointIndex++;
                toMove.LookAt(toMove, waypoints[wayPointIndex].position);
            }
            else
            {
                isEnd = true;
                endOfPathing?.Invoke();
            }
        }
    }

    public void SetWayPoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }
}
