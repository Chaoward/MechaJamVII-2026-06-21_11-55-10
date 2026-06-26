using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public Core core;
    [Space(3)]
    public UnityEvent<Collider2D> onPickup;

    private bool _triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (!other.TryGetComponent(out PlayerMech player))
            return;

        player.targeting.EquipCore(core);
        onPickup.Invoke(other);
        _triggered = true;
        Destroy(this.gameObject, 0.01f);
    }
}