using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/*

    fly == jetpacking
    hover == up/down movement
    dash == horizonta(can be directional) burst mov

*/


[RequireComponent(typeof(Rigidbody2D))]
public class MechMove : MonoBehaviour
{
    [Header("Move Abilitis")]
    public bool canDoubleJump;
    public bool canWarpDash;
    public bool canDirDash;        //directional dash
    public bool canfly;
    public bool isHovering;

    [Space(5)]

    [Header("Move Stats")]
    public float speed = 25f;
    public float jumpSpeed = 40f;
    public float dashSpeed = 80f;
    public float dashTime = 0.4f;
    public float dashDelay = 0.1f;
    public float conserveDashPerc = 0.4f;
    public int dashCost = 200;

    [Space(5)]

    [Header("Ref")]
    public MechEnergy en;

    public bool isFacingRight {get; private set;} = true;
    public bool isDashing {get; private set;}
    public bool isFlying {get; private set;}
    public bool hasDoubleJumped {get; private set;}
    public bool isGrounded
    {
        get
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.05f, LayerMask.GetMask("ground"));
        }
    }


    [SerializeField]
    private Transform groundCheck;
    private Rigidbody2D rb;
    private Vector2 _moveDir;
    private float _dashTimestamp;

    void Start()
    {
        InputHandler.BindInput("Jump", JumpAction);
        InputHandler.BindInput("Dash", DashAction);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) {
            if (Time.fixedTime - _dashTimestamp >= dashTime) {
                isDashing = false;
                _dashTimestamp = Time.fixedTime;
            }
        }

        //block polling movement
        if (!isDashing || canWarpDash) {
            //poll movement
            _moveDir.Set(
                InputHandler.moveInput.x * speed,
                InputHandler.moveInput.y * speed 
            );
        }
    }


    void FixedUpdate()
    {
        //dash
        if (isDashing)
        {
            if (canWarpDash)
            {
                
            }
            else
            {
                Dash( !isGrounded && canfly );
            }
            // else if (canDirDash && _moveDir.magnitude > 0f)
            // {
            //     Dash();
            // }
            // else
            // {
                
            // }
        }
        else {
            //fly

            //move
            Move();
        }
    }


    #region Move Logic

    private void Move(bool isAdditive=false)
    {
        if (isGrounded)
        {
            rb.velocity = _moveDir;
            return;
        }
        
        //air decceleration
        if ( Math.Abs(rb.velocity.x) > speed )
        {
            
        }

        //air acceleration
    }

    private bool Jump(bool isAdditive=false)
    {
        if (isGrounded)
            transform.position += 0.1f * Vector3.up;

        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpSpeed
        );

        return true;
    }

    private void Dash(bool isAdditive=false)
    {
        if (isAdditive)
        {
            
        }
        else
        {
            rb.velocity = _moveDir;
            
        }
    }


    private bool Warp()
    {
        return false;
    }

    #endregion


    #region Input

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && (!isDashing || canWarpDash))
        {
            if (canDoubleJump && !hasDoubleJumped && !isGrounded) {
                Jump();
            }
            else if (isGrounded) {
                Jump();
            }

            if (canfly)
            {
                isFlying = true;
            }
        }
        else
        {
            isFlying = false;
        }
    }


    private void DashAction(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() &&
            !isDashing
        )
        {
            if (Time.fixedTime - Time.deltaTime < dashDelay) return;
            if (!en.Drain(dashCost)) return;

            isDashing = true;
            _dashTimestamp = Time.fixedTime;
            if (canDirDash) {
                _moveDir = InputHandler.moveInput.magnitude > 0f ?
                    dashSpeed * InputHandler.moveInput.normalized :
                    (isFacingRight ? dashSpeed * Vector2.right : dashSpeed * Vector2.left);
            }
            else
            {
                _moveDir = InputHandler.moveInput.x != 0f ?
                    InputHandler.moveInput.normalized.x * dashSpeed * Vector2.right :
                    (isFacingRight ? dashSpeed * Vector2.right : dashSpeed * Vector2.left);
            }   
        }
    }

    #endregion
}
