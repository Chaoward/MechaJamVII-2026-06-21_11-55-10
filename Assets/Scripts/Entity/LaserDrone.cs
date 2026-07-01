using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserDrone : MechEntity
{
    //public const float STOP_TIME = 10f;

    public bool followPath = false;
    public Transform[] path;
    public float moveSpeed = 10f;
    public float randMoveRange = 4f;
    public float stayDelay = 0.12f;
    public Projectile laserProj;
    public Transform attackOrigin;


    private bool isMoving = false;
    private float _time = 0f;
    private int i = 0;
    private CircleCollider2D _collide;

    // [Space(10)]
    // [SerializeField]
    // private SpriteRenderer _render;

    protected override void Start()
    {
        base.Start();
        _collide = GetComponent<CircleCollider2D>();
    } 


    void Update()
    {
        //look at player
        if (GameManager.Player)
        {
            transform.right = -(GameManager.Player.transform.position - transform.position);
            //transform.LookAt(GameManager.Player.transform, Vector3.forward);
        }

        if (isMoving) return;

        //stay
        if (_time > 0f)
        {
            _time -= Time.deltaTime;
        }
        //move
        else if (followPath)
        {
            if (i < path.Length)
            {
                MoveTo(path[i].position);
                i += 1;
            }
            else
                i = 0;
        }
        //random move
        else
        {
            Vector2 movePos = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1, 1)
            );

            movePos = movePos.normalized * randMoveRange;
            RaycastHit2D hit = Physics2D.CircleCast(transform.position,
                _collide ? _collide.radius : 0.2f,
                movePos,
                movePos.magnitude,
                LayerMask.GetMask("ground")
            );

            MoveTo( hit ? 
                transform.position * movePos.normalized * (hit.distance / 2f) :
                transform.position + (Vector3)movePos
            );
        }
    }


    public void Shoot()
    {
        if (!laserProj) return;

        Projectile temp = Instantiate(
            laserProj,
            attackOrigin ? attackOrigin.position : transform.position,
            attackOrigin ? attackOrigin.rotation : transform.rotation
        );
        temp.owner = this;
        temp.GetComponent<Rigidbody2D>().velocity = -transform.right;
    }


    public void MoveTo(Vector3 newPos)
    {
        StartCoroutine(_Move(newPos));
    }

    private IEnumerator _Move(Vector3 trgtPos)
    {
        isMoving = true;
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
        isMoving = false;
        Shoot();
    }
}