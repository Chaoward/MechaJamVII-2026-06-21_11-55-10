using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    public float lifeTime = 5f;
    public MechEntity owner;

    [Space(5)]

    public UnityEvent<Projectile, Collider2D> onHit;

    public Rigidbody2D rb {get; private set;}

    private static string[] _collideLayers = {"ground", "entity"};
    // private bool _isPooled = false;
    // private static Queue<Projectile> _pool = new();
    // private const int _poolSize = 100;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = rb.velocity.normalized * speed;
    }

    protected void Update()
    {
        if (lifeTime <= 0f)
        {
            Destroy(this.gameObject);
            return;
        }

        lifeTime -= Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(LayerMask.GetMask( LayerMask.LayerToName(collision.gameObject.layer) ));
        // Debug.Log(LayerMask.GetMask(_collideLayers));
        if ((LayerMask.GetMask( LayerMask.LayerToName(collision.gameObject.layer) ) & 
            LayerMask.GetMask(_collideLayers)) == 0) 
            return;

        if (collision.TryGetComponent(out MechEntity hit)) {
            if (hit == owner) return;
            if (damage > 0)
                hit.Damage(damage);
            onHit.Invoke(this, collision);
        }

        // if (damage > 0) {
        //     if (collision.TryGetComponent(out MechEntity hit)) {
        //         hit.Damage(damage);
        //     }
        // }

        Destroy(this.gameObject);
    }
}