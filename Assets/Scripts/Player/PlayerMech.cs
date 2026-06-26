using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMech : MechEntity
{
    public MechMove move;
    public MechEnergy en;
    public MechTargeting targeting;
    public BaseCore coreBase;

    void Awake()
    {
        //coreBase.Apply();

        if (move == null)
            move = GetComponent<MechMove>();
        if (en == null)
            en = GetComponent<MechEnergy>();
        if (targeting == null)
            targeting = GetComponent<MechTargeting>();

        //apply ref
        if (move) {
            move.en = en;
            move.aim = targeting;
        }

        if (targeting)
        {
            targeting.mech = this;
            targeting.en = en;
        }
    }
}