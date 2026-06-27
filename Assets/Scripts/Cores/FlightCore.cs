using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlightCore : Core
{
    public Vector2 lockOnZone;
    public int lockCount = 5;
    
    private Collider2D[] _locks;
    private int _i = 0;

    void Start()
    {
        _locks = new Collider2D[lockCount];
    }

    void Update()
    {
        if (!mech) return;
        Physics2D.OverlapBoxNonAlloc(
            (Vector2)mech.targeting.reticle.transform.position,
            lockOnZone,
            0f,
            _locks,
            LayerMask.GetMask("entity")
        );
    }


    public override void Apply()
    {
        mech.move.canDirDash = true;
        mech.move.canfly = true;
        mech.move.speed = 20f;

        mech.en.rechargeDelay = 1f;
        mech.en.rechargeRate = 700f;
        mech.en.Recharge(400);
    }


    public override void FireAction(Projectile proj, RaycastHit2D hit)
    {
        if (!projectile) return;
        if (projectile.GetType() != typeof(SeekingProj)) return;
        //make one loop to find non-null entry
        for (int j = 0; j < _locks.Length; j++) {
            if (_locks[_i] != null) {
                (proj as SeekingProj).target = _locks[_i].transform;
                _i += 1;
                if (_i >= _locks.Length)
                    _i = 0;
                return;
            }
            else
            {
                _i += 1;
                if (_i >= _locks.Length)
                    _i = 0;
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(
            mech ? mech.targeting.reticle.transform.position : transform.localPosition,
            (Vector3)lockOnZone
        );
    }
}