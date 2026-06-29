using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Core : MonoBehaviour
{
    public PlayerMech mech;
    public int attack = 10;
    public float attackRate = 0.7f;
    public int ammo = 100;
    public bool canSemiAuto = true;
    public bool isHitScan = false;
    public Transform attackOffset;
    [Space(3)]
    public TrailRenderer trail;
    public Projectile projectile;
    public SpriteRenderer weaponSprite;
    public SpriteRenderer reticleSprite;


    public abstract void Apply();
    public virtual void FireAction(Projectile p, RaycastHit2D h) {}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackOffset.position, 0.05f);
    }
}