using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MechaMove : MonoBehaviour
{
    [Header("Move Abilitis")]
    public bool canDoubleJump;
    public bool canWarpDash;
    public bool canDirDash;        //directional dash
    public bool canfly;

    [Space(5)]

    [Header("Move Stats")]
    public float speed = 25f;
    public float dashSpeed = 80f;
    public int charge = 1000;


    public bool isDashing {get; private set;}
    public bool hasDoubleJumped {get; private set;}


    [Serializable]
    private Transform groundCheck;
    private Rigidbody2D rb;
    private Vector2 _moveDir;

    void Start()
    {
        InputHandler.BindInput("Jump", JumpAction);
        InputHandler.BindInput("Dash", DashAction);
    }

    // Update is called once per frame
    void Update()
    {
        //poll movement
        _moveDir.SetX( InputHandler.moveInput.x * speed );
        _moveDir.SetY( InputHandler.moveInput.y * speed );
    }


    void FixedUpdate()
    {
        //dash
        if (isDashing)
        {
            if (canDirDash && _moveDir.magnitude > 0f)
            {
                
            }
            else
            {
                
            }
        }

        //double jump

        //jump

        //move
    }


    #region Move Logic

    private void Move(bool isAdditive)
    {
        //rb.velocity = 
    }

    private bool Jump(bool isAdditive)
    {
        return false;
    }

    private bool Dash(bool isAdditive)
    {
        return false;
    }


    private bool Warp()
    {
        return false;
    }

    #endregion


    #region Input

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() &&
            !isDashing
        )
        {
            
        }
    }


    private void DashAction(InputAction.CallbackContext context)
    {
        
    }

    #endregion
}
