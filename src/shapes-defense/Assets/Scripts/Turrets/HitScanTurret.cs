using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanTurret : Turret
{
    public float fireRate;
    public float damage;
    float timeToNextFire;

    public GameObject barrel;

    public GameObject projectilePoint;
    public GameObject shootEffectPrefab;

    Targetter targetter;
    Enemy currentTarget;

    //Idle movement stuff(Maybe put this into a separate behaviour or in a base class to avoid duping this to every turret)
    public float angle;
    public float period;

    private float time;

    void Start()
    {
        targetter = GetComponent<Targetter>();
        targetter.OnTargetAcquired += TargetAcquired;
        targetter.OnTargetLost += TargetLost;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (currentTarget == null)
        {
            IdleMovement();
        }
        else
        {
            barrel.transform.LookAt(currentTarget.transform);
            HandleFiring();
        }
    }

    private void IdleMovement()
    {
        //TODO: review this method so we dont immediately snap from targetable to a random angle.
        float phase = Mathf.Sin(time);
        barrel.transform.localRotation = Quaternion.Euler(new Vector3(0, phase * angle, 0));
    }

    private void TargetLost()
    {
        currentTarget = null;
    }

    private void TargetAcquired(Enemy target)
    {
        currentTarget = target;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        //cleanup
        targetter.OnTargetAcquired -= TargetAcquired;
        targetter.OnTargetLost -= TargetLost;
    }
    
    private void HandleFiring()
    {
        if (timeToNextFire <= 0.0f)
        {
            currentTarget.TakeDamage(damage);
            timeToNextFire = 1 / fireRate;
            FireEffects();
        }
        timeToNextFire -= Time.deltaTime;
    }

    private void FireEffects()
    {
        Instantiate(shootEffectPrefab, projectilePoint.transform.position, projectilePoint.transform.rotation);
    }
}
