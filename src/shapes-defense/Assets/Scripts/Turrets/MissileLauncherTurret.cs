using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherTurret : Turret
{
    public float fireRate;
    float timeToNextFire;
    public GameObject barrel;
    public float damage;

    public GameObject projectilePoint;

    public MissileProjectile missileProjectilePrefab;

    Targetter targetter;
    Enemy currentTarget;


    //Idle movement stuff(Maybe put this into a separate behaviour or in a base class to avoid duping this to every turret)
    public float angle;
    public float period;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        targetter = GetComponent<Targetter>();
        targetter.OnTargetAcquired += TargetAcquired;
        targetter.OnTargetLost += TargetLost;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (currentTarget == null)
        {
            IdleMovement();
        }
        else
        {

            var newRotation = Quaternion.LookRotation(currentTarget.transform.position - barrel.transform.position, Vector3.up);

            barrel.transform.rotation = Quaternion.Euler(-25, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

            //Vector3 point = currentTarget.transform.position;
            //point.y = 10f;
            //barrel.transform.LookAt(point);


            //barrel.transform.LookAt(currentTarget.transform);



            HandleFiring();
        }
    }

    private void IdleMovement()
    {
        //TODO: review this method so we dont immediately snap from targetable to a random angle.
        float phase = Mathf.Sin(time);
        barrel.transform.localRotation = Quaternion.Euler(new Vector3(-25, phase * angle, 0));
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

            LaunchMissile();
            timeToNextFire = 1 / fireRate;
            FireEffects();
        }
        timeToNextFire -= Time.deltaTime;
    }

    private void LaunchMissile()
    {
        var missile = Instantiate(missileProjectilePrefab, projectilePoint.transform.position, projectilePoint.transform.rotation);
        missile.Launch(currentTarget, 5f, 3f, damage);
    }

    private void FireEffects()
    {
        //TODO
        //Instantiate(shootEffectPrefab, projectilePoint.transform.position, projectilePoint.transform.rotation);
    }
}
