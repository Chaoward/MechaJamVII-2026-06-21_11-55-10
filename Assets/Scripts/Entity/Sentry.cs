using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MechEntity
{
    public const float DETECTION_DELAY = 1f;

    public float detectradius = 7f;
    public float attackDelay = 3f;
    public float burstRate = 0.35f;
    public int burstAmount = 3;
    [Space(3)]
    [Header("REF")]
    public Transform attackAim;
    public Transform attackOrigin;
    public Projectile projectile;


    public bool isAttacking {get; private set;} = false;

    private float _time = 0f;
    private int _fireCount = 0;

    protected virtual void Update()
    {
        if (!attackAim || !projectile || !GameManager.Player) return;

        //detection mode
        if (!isAttacking)
        {
            if (_time > 0f)
            {
                _time -= Time.deltaTime;
                return;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackAim.position, detectradius, LayerMask.GetMask("entity"));
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    isAttacking = true;
                    break;
                }
            }
            _time = DETECTION_DELAY;
        }
        //aim at player
        else
        {
            attackAim.right = -(GameManager.Player.transform.position - transform.position);
        }

        //===== ATTACKING =================
        
        //burst
        if (_fireCount > 0)
        {
            if (_time <= 0f)
            {
                Projectile temp = Projectile.Shoot(projectile, 
                    attackOrigin.position, 
                    GameManager.Player.transform.position - attackOrigin.position
                );
                temp.owner = this;
                temp.damage = attack;
                _fireCount -= 1;
                _time = burstRate;
            }
            if (_fireCount < 1) 
                _time = attackDelay;
        }
        //delay
        else if (_time <= 0f)
        {
            _fireCount = burstAmount;
        }

        _time -= Time.deltaTime;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(attackAim.position, detectradius);
    }
}