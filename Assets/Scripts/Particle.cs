using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Particle : MonoBehaviour
{
    public string animationName = "";
    public float lifeTime = 3f;


    void Start()
    {
        if (animationName == "") return;

        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.Play(animationName);
        }
    }
    

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(this.gameObject);
    }
    
    
    private void FinishAnim()
    {
        Destroy(gameObject);
    }
}