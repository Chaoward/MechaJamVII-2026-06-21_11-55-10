using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RocketTank : MechEntity
{
    public Transform[] path;
    public float moveSpeed = 10f;
    public float trackTime = 1.7f;
    public float stayDelay = 0.12f;
    public SeekingProj rocket;

    [Space(3)]
    [Header("REF")]
    public Transform attackOrigin;
    public Transform tankHead;

    private int i = 0;
    private bool _isMoving = false;
    private bool _isTargeting = false;
    private float _time;
    private Rigidbody2D rb;



    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        if (_isTargeting)
        {  
            //track time
            if (_time <= 0f)
            {
                _isTargeting = false;
                _time = stayDelay;
                SeekingProj temp = Projectile.Shoot(rocket,
                    attackOrigin.position,
                    GameManager.Player.transform.position - transform.position
                ) as SeekingProj;
                temp.owner = this;
                temp.target = GameManager.Player.transform;
            }
            //track player
            // else
            // {
            //     transform.right = -(GameManager.Player.transform.position - transform.position);
            // }
            _time -= Time.deltaTime;
        }
        //stay
        else if (!_isMoving)
        {
            _time -= Time.deltaTime;
            if (_time <= 0f)
            {
                _isMoving = true;
                i += 1;
                if (i >= path.Length)
                    i = 0;
            }
        }
        //moving
        else
        {
            if (path.Length < 1)
            {
                _isTargeting = true;
                _time = trackTime;
                rb.velocity *= Vector2.up;
                return;
            }

            rb.velocity = new Vector2(
                GameManager.Player.transform.position.x - transform.position.x > 0f ? moveSpeed : -moveSpeed,
                rb.velocity.y
            );

            if (Math.Abs(path[i].position.x - transform.position.x) <= 0.1f)
            {
                _isTargeting = true;
                _time = trackTime;
                rb.velocity *= Vector2.up;
            }
        }
    }


    public void MoveTo(Vector3 newPos)
    {
        StartCoroutine(_Move(newPos));
    }

    private IEnumerator _Move(Vector3 trgtPos)
    {
        _isMoving = true;
        float distance = (trgtPos - transform.position).magnitude;
        while ( distance > 0.1f ) {
            transform.position = Vector3.Lerp(
                transform.position,
                trgtPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
            distance = (trgtPos - transform.position).magnitude;
        }

        _time = stayDelay;
        _isMoving = false;
    }
}