using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    public float lifeTime = 5f;

    [Space(5)]

    public UnityEvent<Projectile, Collider2D> onHit;

    public Rigidbody2D rb {get; private set;}

    private static string[] _collideLayers = {"ground", "entity"};


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = rb.velocity.normalized * speed;
    }

    void Update()
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
        if ((collision.gameObject.layer & LayerMask.GetMask(_collideLayers)) == 0) 
            return;

        onHit.Invoke(this, collision);

        if (damage > 0) {
            if (collision.TryGetComponent(out MechEntity hit))
                hit.Damage(damage);
        }

        Destroy(this.gameObject);
    }
}