using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!other.CompareTag("Player") || other.gameObject.layer != LayerMask.GetMask("entity")) return;
        if (!other.CompareTag("Player")) return;

        try
        {
            onTrigger.Invoke();
        } catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        Destroy(this.gameObject);
    }
}