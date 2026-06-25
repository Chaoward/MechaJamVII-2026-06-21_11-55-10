using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseCore : Core
{   
    public override void Apply()
    {
        //===== MOVEMENT =================
        mech.move.canDoubleJump = false;
        mech.move.canWarpDash = false;
        mech.move.canDirDash = false;        //directional dash
        mech.move.canfly = false;
        mech.move.isHovering = false;

        mech.move.speed = 15f;
        mech.move.jumpSpeed = 25f;
        mech.move.airAccelPerc = 2.7f;   //percent of speed to accelerate in air

        mech.move.dashSpeed = 60f;
        mech.move.dashTime = 0.14f;
        mech.move.dashDelay = 0.14f;
        mech.move.conserveDashPerc = 0.4f;
        mech.move.dashCost = 200;

        mech.move.flightForce = 60f;
        mech.move.maxFlightSpeed = 30f;
        mech.move.conserveFlightPerc = 0.7f;
        mech.move.flightCost = 100;
    }
}