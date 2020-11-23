using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemyMover : MonoBehaviour, IEnemyMover
{

    public Transform[] waypoints;
    public Transform toMove;
    private int wayPointIndex;
    private bool isEnd;
    public Action endOfPathing;
    private bool isFlipping = false;
    [SerializeField]
    private float flipDuration = 1;
    private Vector3 currentWaypoint;

    void Start()
    {
        if (waypoints?.Length == 0)
        {
            Debug.LogError("[SphereEnemyMover] Waypoints not set!");
            isEnd = true;
            return;
        }
        currentWaypoint = new Vector3(waypoints[wayPointIndex].position.x, 0.5f, waypoints[wayPointIndex].position.z);
        toMove.LookAt(currentWaypoint);
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
        if (!isFlipping)
        {
            if (Vector3.Distance(toMove.position, currentWaypoint) < 0.4f)
            {
                //Increment the waypoint index until the last one
                if (wayPointIndex < waypoints.Length - 1)
                {
                    wayPointIndex++;
                    currentWaypoint = new Vector3(waypoints[wayPointIndex].position.x, 0.5f, waypoints[wayPointIndex].position.z);
                    toMove.LookAt(toMove, currentWaypoint);
                }
                else
                {
                    isEnd = true;
                    endOfPathing?.Invoke();
                    return;
                }
            }

            Vector3 direction = (currentWaypoint - toMove.position).normalized;
            StartCoroutine(Flip(direction));
        }
    }

    IEnumerator Flip(Vector3 direction)
    {
        isFlipping = true;
        //yield return new WaitForSeconds(1f);

        float flipCurrentTime = 0f;
        float centerPoint = toMove.localScale.x / 2;
        Vector3 rotationPoint = toMove.position + (direction * centerPoint) + (centerPoint * Vector3.down);
        Vector3 rollAxis = Vector3.Cross(Vector3.up, direction);

        float lastAngle = 0;
        while (flipCurrentTime < flipDuration)
        {
            yield return new WaitForEndOfFrame();
            
            flipCurrentTime += Time.deltaTime;

            float newAngle = (flipCurrentTime / flipDuration) * 90;
            toMove.RotateAround(rotationPoint, rollAxis, (newAngle - lastAngle));
            lastAngle = newAngle;

            //update parent position, then reset the cube local position
            //so we can move the whole hierarchy together
            transform.position = toMove.transform.position;
            toMove.transform.localPosition = Vector3.zero;
        }

        isFlipping = false;
    }

    public void SetWayPoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }
}
