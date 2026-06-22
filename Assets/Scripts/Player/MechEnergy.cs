using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechEnergy : MonoBehaviour
{
    public int charge;

    [Header("Energy Stats")]
    public int maxCharge = 1000;
    public float rechargeRate = 400;    //per sec
    public float rechargeDelay = 2.5f;  //sec
    
    private float _time;

    void Start()
    {
        charge = maxCharge;
    }

    void Update()
    {
        if (maxCharge >= charge) return;
        if (Time.fixedTime - _time < rechargeDelay) return;

        Recharge( (int)(rechargeRate * Time.deltaTime) );
    }


    public bool Drain(int amount)
    {
        if (charge < 1f)
        {
            return false;
        }

        charge -= amount;
        _time = Time.deltaTime;
        return true;
    }


    public bool Recharge(int amount)
    {
        if (charge >= maxCharge) return false;

        charge += amount;
        if (charge >= maxCharge)
        {
            charge = maxCharge;
        }

        return true;
    }
}
