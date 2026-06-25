using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MechTargeting : MonoBehaviour
{
    //public Core core;
    public Transform reticle;
    public Core core;

    [Space(5)]

    [Header("Ref")]
    public MechEnergy en;
    public PlayerMech mech;

    public bool isFiring {get; private set;}


    private Vector2 _aimDirection;
    private float _fireTime = 0f;


    void Start()
    {
        InputHandler.BindInput("Fire", FireAction);
    }


    void Update()
    {
        reticle.position = Camera.main.ScreenToWorldPoint(InputHandler.aimInput);
        _aimDirection = (reticle.position - transform.position).normalized;

        //rotate weapon render
        core.weaponSprite.transform.Rotate(0f, 0f,
            Vector2.SignedAngle(-core.weaponSprite.transform.up, _aimDirection),
            Space.World
        );
    
        if (_fireTime > 0f)
        {
            _fireTime -= Time.deltaTime;
        }
        else if (isFiring)
        {
            Fire();
        }
    }


    public bool EquipCore(Core newCore)
    {
        try
        {
            Destroy(core.gameObject);
            core = newCore;

            mech.coreBase.Apply();
            if (core != null)
                core.Apply();

            core.transform.parent = this.transform;
            core.transform.localPosition = Vector3.zero;
            
            return true;
        } catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }

    #region Fire
    private void Fire()
    {
        //hitscan fire
        if (core.isHitScan)
        {
            //render
            if (core.trail)
            {
                //trail effect here
            }

            RaycastHit2D hit = Physics2D.Raycast(
                (Vector2)mech.transform.position + core.attackOffset,
                _aimDirection,
                1000f
            );

            if (!hit) return;
            if (hit.transform.TryGetComponent(out MechEntity trgt))
            {
                trgt.Damage(core.attack);
            }
        }
        //projectile fire
        else
        {
            if (!core.projectile) return;

            Projectile proj = Instantiate(core.projectile, mech.transform.position, Quaternion.identity);
            proj.rb.velocity = _aimDirection;
        }
    }
    #endregion


    #region Input Actions

    private void FireAction(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (core == null) return;
            isFiring = true;
        }
        else
        {
            isFiring = false;
            _fireTime = core ? core.attackRate : 0f;
        }
    }

    #endregion
}