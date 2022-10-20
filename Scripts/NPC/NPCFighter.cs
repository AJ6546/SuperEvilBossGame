using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFighter : MonoBehaviour
{
    [SerializeField]
    float dmgFactor01 = 50, dmgFactor02 = 25, dmgFactor03 = 20,
     strength = 100, distanceToTarget, damagingDistance = 1f, damage, iWaitTime = 1f, projectileAttackingDistance=5f;
    Animator animator;
    [SerializeField] Health targetHealth;
    CoolDownTimer cdTimer;
    Vector3 targetPos;
    [SerializeField] string prefab;
    public PoolManager poolManager;
    [SerializeField] string projectile;
    [SerializeField] bool canAttack=true;
    [SerializeField] GameObject attacker;
    void Start()
    {
        poolManager = PoolManager.instance;
        animator = GetComponent<Animator>();
        cdTimer = GetComponent<CoolDownTimer>();
    }
    #region NPCAttack
    public void AtackBehaviour(Health targetHealth)
    {
        this.targetHealth = targetHealth;
        
        if (targetHealth.IsDead())
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            return;
        }
        attacker = GetComponent<Projectiles>() ? GetComponent<Projectiles>().GetInstantiatore() : gameObject;
        if (Time.time > cdTimer.nextAttackTime["Attack03"] && canAttack)
        {
            if (projectile != "")
            {
                if (ProjectileDamagingDistance())
                {
                    canAttack = false;
                    animator.SetTrigger("Attack03");
                    cdTimer.nextAttackTime["Attack03"] = (int)Time.time + cdTimer.coolDownTime["Attack03"];
                }
            }
            else
            {
                canAttack = false;
                animator.SetTrigger("Attack03");
                cdTimer.nextAttackTime["Attack03"] = (int)Time.time + cdTimer.coolDownTime["Attack03"];
            }
        }
        if (Time.time > cdTimer.nextAttackTime["Attack02"] && canAttack)
        {
            if (!AtDamagingDistance()) return;
            canAttack = false;
            animator.SetTrigger("Attack02");
            cdTimer.nextAttackTime["Attack02"] = (int)Time.time + cdTimer.coolDownTime["Attack02"];
        }
        if (Time.time > cdTimer.nextAttackTime["Attack01"] && canAttack)
        {
            if (!AtDamagingDistance()) return;
            canAttack = false;
            animator.SetTrigger("Attack01");
            cdTimer.nextAttackTime["Attack01"] = (int)Time.time + cdTimer.coolDownTime["Attack01"];
        }
    }
    #endregion
    #region Damage
    void Hit01()
    {
        
        if (AtDamagingDistance())
            targetHealth.TakeDamage(attacker, strength / dmgFactor01);
    }
    void Hit02()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(attacker, strength / dmgFactor02);
    }
    void Hit03()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(attacker, strength / dmgFactor03);
    }
    void Hit04()
    {
        GetComponent<ProjectileInstantiator>().SpawnProjectile(gameObject, projectile);
    }
    #endregion

    public void UpdateTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
    bool ProjectileDamagingDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        return distanceToTarget >= projectileAttackingDistance;
    }
    public bool AtDamagingDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        return distanceToTarget <= damagingDistance;
    }
    void CanAttack()
    {
        Debug.Log("Called");
        canAttack = true;
    }
}
