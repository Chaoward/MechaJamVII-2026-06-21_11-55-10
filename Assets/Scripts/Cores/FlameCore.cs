using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlameCore : Core
{   
    [Header("Flame Jump AOE")]
    public AoeDamage aoePrefab;
    public float downwardHeight = 2f;
    
    public override void Apply()
    {
        //===== MOVEMENT =================
        mech.move.canDoubleJump = true;
        mech.move.gravFallingScale *= 1.6f;
        mech.move.gravScale *= 1.2f;

        mech.en.rechargeDelay = 1f;
        mech.en.Recharge(250);

        mech.move.OnDoubleJump.AddListener(_FlameJump);
    }

    void OnDestroy()
    {
        if (mech)
            mech.move.OnDoubleJump.RemoveListener(_FlameJump);
    }

    private void _FlameJump()
    {
        AoeDamage temp = Instantiate(
            aoePrefab, 
            transform.position + (Vector3.down * downwardHeight), 
            Quaternion.identity
        );
        temp.teamTag = mech.tag;
        temp.gameObject.SetActive(true);
    }
}