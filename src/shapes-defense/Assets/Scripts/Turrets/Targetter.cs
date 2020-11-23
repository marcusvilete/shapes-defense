using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Targetting component requires a sphere collider
[RequireComponent(typeof(SphereCollider))]
public class Targetter : MonoBehaviour
{
    private Enemy currentTarget;
    private List<Enemy> targetsInRange = new List<Enemy>();
    public event Action<Enemy> OnTargetAcquired;
    public event Action OnTargetLost;
    public event Action<Enemy> OnTargetInRange;
    public event Action<Enemy> OnTargetOutOfRange;

    SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            currentTarget = GetNearestTarget();

            if (currentTarget != null)
            {
                OnTargetAcquired?.Invoke(currentTarget);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: maybe check for tag is faster?
        var targetable = other.GetComponent<Enemy>();
        if (targetable == null)
        {
            return;
        }
        targetsInRange.Add(targetable);

        targetable.OnDeath += TargetRemoved;
        OnTargetInRange?.Invoke(targetable);
    }

    private void TargetRemoved(Enemy target)
    {
        target.OnDeath -= TargetRemoved;
        if (currentTarget == target)
        {
            currentTarget = null;
            OnTargetLost?.Invoke();
        }
        targetsInRange.Remove(target);
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO: maybe check for tag is faster?
        var targetable = other.GetComponent<Enemy>();
        if (targetable == null)
        {
            return;
        }

        targetsInRange.Remove(targetable);
        //targetable.OnEnemyDeath -= TargetRemoved;

        TargetRemoved(targetable);
        OnTargetOutOfRange?.Invoke(targetable);
    }

    private Enemy GetNearestTarget()
    {
        int length = targetsInRange.Count;

        if (length == 0)
        {
            return null;
        }

        Enemy nearest = null;
        float distance = float.MaxValue;
        for (int i = length - 1; i >= 0; i--)
        {
            Enemy targetable = targetsInRange[i];
            //cleanup
            if (targetable == null)
            {
                targetsInRange.RemoveAt(i);
                continue;
            }
            float currentDistance = Vector3.Distance(transform.position, targetable.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                nearest = targetable;
            }
        }

        return nearest;
    }
}
