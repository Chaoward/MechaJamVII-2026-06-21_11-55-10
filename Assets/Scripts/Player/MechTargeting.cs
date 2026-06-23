using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MechTargeting : MonoBehaviour
{
    //public Core core;
    public Transform reticle;

    [Space(5)]

    [Header("Ref")]
    public MechEnergy en;


    void Update()
    {
        reticle.position = Camera.main.ScreenToWorldPoint(InputHandler.aimInput);
    }
}