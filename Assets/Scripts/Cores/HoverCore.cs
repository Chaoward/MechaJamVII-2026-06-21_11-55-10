using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverCore : Core
{   
    [Header("Spread Stat")]
    public Vector2 yRange;
    public Vector2 xRange;
    
    public override void Apply()
    {
        //===== MOVEMENT =================
        mech.move.canDirDash = true;        //directional dash
        mech.move.isHovering = true;

        mech.move.speed = 25f;
        mech.move.dashSpeed = 75f;
        mech.move.dashTime = 0.1f;
        mech.move.dashDelay = 0.1f;
        mech.move.dashCost = 200;

        mech.en.rechargeDelay = 1f;
        mech.en.rechargeRate = 350f;
        mech.en.Recharge(250);
    }

    public override void FireAction(Projectile p, RaycastHit2D h)
    {
        if (!p) return;
        Rigidbody2D temp = p.GetComponent<Rigidbody2D>();
        temp.velocity = new Vector2(
            temp.velocity.x + Random.Range(xRange.x, xRange.y),
            temp.velocity.y + Random.Range(yRange.x, yRange.y)
        ).normalized * p.speed;
    }
}