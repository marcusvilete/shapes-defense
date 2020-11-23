using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{

    private Enemy target;
    private Rigidbody rb;
    private float acceleration;
    private float speed;

    public float damage;
    public float turnSpeed;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Explode();
            return;
        }
        else
        {
            //TODO: follow the target
            HandleFollowing();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }


    private void HandleFollowing()
    {
        rb.velocity = transform.forward * speed;
        speed = speed + (acceleration * Time.deltaTime);


        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed));


        //rb.rotation = Quaternion.LookRotation(GetLookRotation());

        //rb.velocity = transform.forward * rb.velocity.magnitude;
        //rb.velocity += transform.forward * acceleration * Time.deltaTime;

    }

    //private Vector3 GetLookRotation()
    //{
    //    return (target.transform.position - transform.position).normalized;
    //}

    public void Launch(Enemy target, float speed, float acceleration, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.acceleration = acceleration;
        this.damage = damage;
    }

    public void Explode()
    {
        //TODO: damage area
        if (target != null)
        {
            target.TakeDamage(damage);
            target = null;
        }
        Destroy(gameObject);
    }
}
