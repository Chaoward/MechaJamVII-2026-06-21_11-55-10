using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AoeDamage : MonoBehaviour
{
    public int damage = 10;
    public float radius = 1f;
    public string teamTag;

    [Space(5)]
    public UnityEvent OnExplode;

    void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("entity"));
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(teamTag)) continue;
            if (hit.TryGetComponent(out MechEntity entity))
            {
                entity.Damage(damage);
            }
        }
        OnExplode.Invoke();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}