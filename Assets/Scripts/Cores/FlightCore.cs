using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlightCore : Core
{
    public override void Apply()
    {
        mech.move.canDirDash = true;
        mech.move.canfly = true;
        mech.move.speed = 20f;
    }
}