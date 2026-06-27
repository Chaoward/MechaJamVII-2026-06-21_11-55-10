using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SeekingProj : Projectile
{
    public float turnRate = 10f;
    public Transform target;

    //private float _turnAngle;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        //_turnAngle = Vector2.SignedAngle(rb.velocity, (Vector2)target.position);

        //rotate to target, local x axis is the front
        if (target != null) {
            rb.velocity = Vector2.Lerp(rb.velocity, (Vector2)target.position - (Vector2)transform.position, 
                turnRate * Time.fixedDeltaTime //* Vector2.SignedAngle(rb.velocity, (Vector2)target.position)
            ).normalized * speed;
            transform.right = (Vector3)rb.velocity;
        }
    }
}