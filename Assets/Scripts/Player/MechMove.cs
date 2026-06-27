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
    public float airAccelPerc = 0.3f;   //percent of speed to accelerate in air

    [Space(5)]
    [Header("Dash Stats")]

    public float dashSpeed = 80f;
    public float dashTime = 0.4f;
    public float dashDelay = 0.1f;
    public float conserveDashPerc = 0.4f;
    public int dashCost = 200;

    [Space(5)]
    [Header("Flight Stats")]

    public float flightForce = 10f;
    public float maxFlightSpeed = 50f;
    public float conserveFlightPerc = 0.7f;
    public int flightCost = 30;

    [Space(5)]
    [Header("Gravity Scale")]

    public float gravScale = 2f;
    public float gravFallingScale = 2.8f;

    [Space(5)]
    [Header("Ref")]
    public MechEnergy en;
    public MechTargeting aim;

    public bool isMoving { get {
        return InputHandler.moveInput.x != 0f && (canWarpDash || !isDashing);  
    }}
    public bool isFacingRight { get; private set; } = true;
    public bool isDashing { get; private set; }
    public bool isFlying { get; private set; }
    public bool hasDoubleJumped { get; private set; }
    public bool isGrounded
    {
        get; private set;
        // get
        // {
        //     return Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("ground"));
        // }
    }
    [SerializeField]
    private float _groundcheckradius = 0.15f;


    [SerializeField]
    private Transform groundCheck;
    private Rigidbody2D rb;
    private Vector2 _moveDir;
    private float _dashTimestamp;

    void Start()
    {
        InputHandler.BindInput("Jump", JumpAction);
        InputHandler.BindInput("Dash", DashAction);

        rb = GetComponent<Rigidbody2D>();
        _moveDir = new Vector2();
    }

    #region Updates
    // Update is called once per frame
    void Update()
    {
        // //dash timing
        // if (isDashing) {
        //     if (Time.fixedTime - _dashTimestamp >= dashTime) {
        //         isDashing = false;
        //         _dashTimestamp = Time.fixedTime;
        //     }
        // }

        //polling movement
        if (!isDashing || canWarpDash)
        {
            _moveDir.Set(
                InputHandler.moveInput.x * speed,
                isHovering ? InputHandler.moveInput.y * speed : rb.velocity.y
            );
        }

        // set facing
        isFacingRight = transform.position.x - aim.reticle.position.x < 0f;
        if (isFacingRight ^ transform.lossyScale.x > 0f)
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y, transform.localScale.z
            );
    }


    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, _groundcheckradius, LayerMask.GetMask("ground"));
        if (isGrounded)
            hasDoubleJumped = false;

        //dash
        if (isDashing)
        {
            if (canWarpDash)
            {
                Warp();
            }
            else
            {
                Dash(!isGrounded && canfly);
            }
        }
        else
        {
            //flight
            if (isFlying)
                Fly();

            //move
            Move();
        }

        //double gravity on falls, 150% normal
        if (isFlying)
            rb.gravityScale = 1f;
        else
            rb.gravityScale = rb.velocity.y < 0f ? gravFallingScale : gravScale;
    }

    #endregion


    #region Move Logic

    private void Move(bool isAdditive = false)
    {
        if (isGrounded || isHovering)
        {
            rb.velocity = _moveDir;
            return;
        }

        //overspeed air decceleration by percentage of speed over normal value
        if (Math.Abs(rb.velocity.x) > speed)
        {
            float decelVal = Math.Abs(rb.velocity.x) - speed;
            rb.velocity += (rb.velocity.x > 0f ? -decelVal : decelVal) * Time.fixedDeltaTime * Vector2.right;
        }

        //air acceleration
        if (InputHandler.moveInput.x != 0f)
        {
            //float temp = Math.Clamp((_moveDir.x * airAccelPerc * Time.fixedDeltaTime) + rb.velocity.x, -speed, speed);
            rb.velocity = new Vector2(
                //temp,
                Math.Clamp((_moveDir.x * airAccelPerc * Time.fixedDeltaTime) + rb.velocity.x, -speed, speed),
                rb.velocity.y
            );
        }
    }

    private bool Jump(bool isAdditive = false)
    {
        if (isAdditive && rb.velocity.y > 0f)
        {
            rb.velocity += jumpSpeed * Vector2.up;
        }
        else {
            rb.velocity = new Vector2(
                rb.velocity.x,
                jumpSpeed
            );
        }

        if (isGrounded) 
            transform.position += _groundcheckradius * Vector3.up;    //unGrounded

        // rb.velocity = (isAdditive ? rb.velocity : Vector2.zero) + new Vector2(
        //     rb.velocity.x,
        //     jumpSpeed
        // );

        return true;
    }

    private void Dash(bool isAdditive = false)
    {
        if (Time.fixedTime - _dashTimestamp >= dashTime)
        {
            isDashing = false;
            _dashTimestamp = Time.fixedTime;

            //conserve some movement in air
            if (!isGrounded)
            {
                rb.velocity *= conserveDashPerc;
            }

            return;
        }
        rb.velocity = _moveDir;
    }


    private void Fly()
    {
        if (!en.Drain( (int)Math.Ceiling(flightCost * Time.fixedDeltaTime) )) return;

        float yVelo = 0f;
        // float yVelo = (rb.velocity.y > maxFlightSpeed ? 0.1f : 1f) *    // 10% of flight power if over max flight speed
        //     Math.Clamp(rb.velocity.y + (flightForce * Time.fixedDeltaTime), float.NegativeInfinity, maxFlightSpeed);

        if (rb.velocity.y < 0f)
        {
            yVelo = ((flightForce - rb.velocity.y) * Time.fixedDeltaTime) + rb.velocity.y;
        }
        else
        {
            yVelo = (rb.velocity.y > maxFlightSpeed ? 0.1f : 1f) *    // 10% of flight power if over max flight speed
                Math.Clamp(rb.velocity.y + (flightForce * Time.fixedDeltaTime), 0.1f, maxFlightSpeed);
        }

        rb.velocity = new Vector2(
            rb.velocity.x,
            yVelo
        );
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
            if (canDoubleJump && !hasDoubleJumped && !isGrounded)
            {
                hasDoubleJumped = true;
                Jump(true);
            }
            else if (isGrounded)
            {
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
            if (canDirDash)
            {
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


    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(groundCheck.position, _groundcheckradius);

        // Gizmos.color = !hasDoubleJumped ? Color.green : Color.red; 
        // Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
