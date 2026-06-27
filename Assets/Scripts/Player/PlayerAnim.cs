using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    // [Header("Render Ref")]
    // public SpriteRenderer

    // [Space(3)]
    public PlayerMech mech;
    public Animator anim;


    void Update()
    {
        anim.SetBool("moving", mech.move.isMoving);
        anim.SetBool("inAir", !mech.move.isGrounded);
    } 
}