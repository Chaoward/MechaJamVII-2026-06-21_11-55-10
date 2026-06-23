using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    public MechMove move;
    public MechEnergy en;
    public MechTargeting targeting;

    void Awake()
    {
        if (move == null)
            move = GetComponent<MechMove>();
        if (en == null)
            en = GetComponent<MechEnergy>();
        if (targeting == null)
            targeting = GetComponent<MechTargeting>();

        //apply ref
        move.en = en;
        move.aim = targeting;
    }
}