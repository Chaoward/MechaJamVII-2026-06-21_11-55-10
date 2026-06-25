using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MechEntity : MonoBehaviour
{
    public int hp = 100;
    public int attack = 10;
    public UnityEvent<MechEntity, int> onDamage;
    public UnityEvent<MechEntity> onHpZero;

    public void Damage(int amount)
    {
        onDamage.Invoke(this, amount);
        hp -= amount;
        if (hp <= 0f)
        {
            onHpZero.Invoke(this);
            Destroy(this.gameObject);
            return;
        }
    }
}