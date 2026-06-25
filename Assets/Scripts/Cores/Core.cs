using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Core : MonoBehaviour
{
    public PlayerMech mech;
    public int attack = 10;
    public float attackRate = 0.7f;
    public bool isHitScan = false;
    public Vector2 attackOffset;
    [Space(3)]
    public TrailRenderer trail;
    public Projectile projectile;
    public SpriteRenderer weaponSprite;
    public SpriteRenderer reticleSprite;


    public abstract void Apply();
    public virtual void FireAction()
    {
        throw new System.NotImplementedException();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3)attackOffset, 0.05f);
    }
}