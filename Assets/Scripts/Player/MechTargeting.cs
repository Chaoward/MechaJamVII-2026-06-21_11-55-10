using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private Core baseCore;


    void Awake()
    {
        baseCore = mech.coreBase;
    }

    void Start()
    {
        InputHandler.BindInput("Fire", FireAction);
    }


    void Update()
    {
        reticle.position = Camera.main.ScreenToWorldPoint(InputHandler.aimInput);
        _aimDirection = (reticle.position - (core ? core.attackOffset.position : transform.position)).normalized;

        //rotate weapon render
        core.weaponSprite.transform.Rotate(0f, 0f,
            Vector2.SignedAngle(mech.move.isFacingRight ? core.weaponSprite.transform.right : -core.weaponSprite.transform.right, _aimDirection),
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

    #region Equiping
    public bool EquipCore(Core newCore, bool deleteCur=true)
    {
        try
        {
            if (core != baseCore && core != null && deleteCur)
                Destroy(core.gameObject);
            else if (core == baseCore)
                core.gameObject.SetActive(false);
            core = newCore;
            if (core == null) { 
                core = baseCore;
            }
            core.gameObject.SetActive(true);
            core.mech = mech;

            baseCore.Apply();
            core.Apply();

            core.transform.parent = this.transform;
            core.transform.localPosition = Vector3.zero;
            reticle = core.reticleSprite.transform;
            
            return true;
        } catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }

    public bool RemoveCore()
    {
        EquipCore(baseCore);
        return true;
    }
    #endregion

    #region Fire
    private void Fire()
    {
        //hitscan fire
        if (core.isHitScan)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                (Vector2)core.attackOffset.position,
                _aimDirection,
                1000f
            );

            //render
            if (core.trail)
            {
                //trail effect here
                TrailRenderer temp = Instantiate(
                    core.trail, core.attackOffset.position,
                    Quaternion.identity
                );
                temp.transform.Rotate(0f, 0f,
                    Vector2.SignedAngle(-temp.transform.up, _aimDirection),
                    Space.World
                );
                StartCoroutine(_ScheduleTrail(temp, hit));
            }

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

            Projectile proj = Instantiate(core.projectile, core.attackOffset.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = _aimDirection;
            proj.owner = mech;
            proj.damage = core.attack;
        }

        core.FireAction();
        _fireTime = core.attackRate;

        //ammo check
        if (core.ammo > 0)
        {
            core.ammo -= 1;
            if (core.ammo < 1)
            {
                RemoveCore();
            }
        }
    }

    private IEnumerator _ScheduleTrail(TrailRenderer render, RaycastHit2D hit)
    {
        yield return new WaitForSeconds(0.05f);
        if (!hit)
            render.transform.localPosition = render.transform.right * 100f;
        else
            render.transform.position = hit.transform.position;
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