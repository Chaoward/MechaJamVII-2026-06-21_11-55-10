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

    [Space(3)]
    [SerializeField]
    private SpriteRenderer render;

    private static Color _colorAdder = new(0.2f, 0.2f, 0.2f, 0.2f);

    protected virtual void Start()
    {
        
    } 

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

        if (render)
        {
            render.color = Color.red;
            StartCoroutine(_DmgRender());
        }
    }

    private IEnumerator _DmgRender()
    {
        while (render.color != Color.white)
        {
            yield return new WaitForSeconds(0.1f);
            render.color += _colorAdder;
        }
    }
}