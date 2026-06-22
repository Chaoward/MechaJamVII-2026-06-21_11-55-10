using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    public MechMove move;
    public MechEnergy en;

    void Start()
    {
        if (move == null)
            move = GetComponent<MechMove>();
        if (en == null)
            en = GetComponent<MechEnergy>();

        //apply ref
        move.en = en;
    }
}