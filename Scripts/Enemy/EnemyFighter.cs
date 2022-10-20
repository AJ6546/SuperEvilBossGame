using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFighter : MonoBehaviour
{
    [SerializeField]
    float dmgFactor01 = 50, dmgFactor02 = 25, dmgFactor03 = 20,
      strength = 100, distanceToTarget, damagingDistance = 1f, damage, iWaitTime=1f;
    Animator animator;
    Health targetHealth;
    CoolDownTimer cdTimer;
    Vector3 targetPos;
    [SerializeField] int[] minmax = new int[8];
    [SerializeField] int number = 1,j,n_phase = 3, prefab_x_max=50,prefab_z_max=50;
    [SerializeField] string prefab;
    [SerializeField] float cooldown = 5, nextAttack = 0;
    public PoolManager poolManager;
    [SerializeField] bool[] phase_flags;
    [SerializeField] float[] phase_health;
    Vector3 tempPos;
    [SerializeField] bool canAttack=true;
    void Start()
    {
        poolManager = PoolManager.instance;
        animator = GetComponent<Animator>();
        cdTimer = GetComponent<CoolDownTimer>();
        phase_flags = new bool[n_phase];
        StartCoroutine(jFinder());
    }
    #region EnemyAttack
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
        if (Time.time > cdTimer.nextAttackTime["Attack03"] && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack03");
            cdTimer.nextAttackTime["Attack03"] = (int)Time.time + cdTimer.coolDownTime["Attack03"];
        }
        if (Time.time > cdTimer.nextAttackTime["Attack02"] && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack02");
            cdTimer.nextAttackTime["Attack02"] = (int)Time.time + cdTimer.coolDownTime["Attack02"];
        }
        if (Time.time > cdTimer.nextAttackTime["Attack01"] && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack02");
            cdTimer.nextAttackTime["Attack01"] = (int)Time.time + cdTimer.coolDownTime["Attack01"];
        }
    }
    #endregion
    #region BossEnemyAttack
    public void BossAttackBehaviour(Health targetHealth)
    {
        this.targetHealth = targetHealth;
        float hf = GetComponent<Health>().GetHealthFraction();

        for (int i = 0; i < n_phase; i++)
        {
            if (hf <= phase_health[i])
            {
                if (!phase_flags[i])
                {
                    Phase();
                    phase_flags[i] = true;
                }
            }
        }
        if (nextAttack >= Time.time || !canAttack) return;
        if (j <= 10)
        {
            damage = Random.Range(minmax[0], minmax[1]);
            canAttack = false;
            animator.SetTrigger("Attack01");
            
        }
        else if (j > 10 && j <= 20)
        {
            damage = Random.Range(minmax[2], minmax[3]);
            canAttack = false;
            animator.SetTrigger("Attack02");
        }
        else if (j > 20 && j <= 30)
        {
            damage = Random.Range(minmax[4], minmax[5]);
            canAttack = false;
            animator.SetTrigger("Attack03");
        }
        else
        {
            damage = Random.Range(minmax[6], minmax[7]);
            canAttack = false;
            animator.SetTrigger("Attack04");
        }
        nextAttack = cooldown + Time.time;
    }
    #endregion
    #region Damage
    void Hit()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(gameObject, damage);
    }
    void Hit01()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(gameObject, strength / dmgFactor01);
    }
    void Hit02()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(gameObject, strength / dmgFactor02);
    }
    void Hit03()
    {
        if (AtDamagingDistance())
            targetHealth.TakeDamage(gameObject,strength / dmgFactor03);
    }
    void Phase()
    {
        animator.ResetTrigger("Attack01");
        animator.ResetTrigger("Attack02");
        animator.ResetTrigger("Attack03");
        animator.ResetTrigger("Attack04");
        canAttack = false;
        animator.SetTrigger("Attack05");
    }
    void Phase_Hit()
    {
        switch (number)
        {
            case 1:
                {
                  GetComponent<EnemyController>().CAttack(false);
                  StartCoroutine(Delay(2f));
                    for (int i = 0; i < 100; i++)
                    {

                    Vector3 spawnPos = new Vector3(Random.Range(0, prefab_x_max), 1, Random.Range(0, prefab_x_max));
                    poolManager.Spawn(prefab, spawnPos, transform);
                    }
                }
                break;
            case 2:
                {
                    tempPos = transform.position;
                    transform.position = transform.position + transform.forward * -5f; 
                    GetComponent<EnemyController>().CAttack(false);
                    StartCoroutine(Delay(5f));
                    Vector3 spawnPos = new Vector3(Random.Range(0, prefab_x_max), 5, Random.Range(0, prefab_x_max));
                    poolManager.Spawn(prefab, spawnPos, transform);
                }
                break;
            case 3:
                {
                    GetComponent<EnemyController>().CAttack(false);
                    StartCoroutine(Delay(2f));
                    poolManager.Spawn(prefab, transform.position + transform.right * 2f, transform);
                    poolManager.Spawn(prefab, transform.position + transform.right * -2f, transform);
                }
                break;
            case 4:
                {
                    GetComponent<EnemyController>().CAttack(false);
                    StartCoroutine(Delay(2f));
                    for (int i = 0; i < 100; i++)
                    {
                        Vector3 spawnPos = new Vector3(Random.Range(0, prefab_x_max), 0.1f, Random.Range(0, prefab_x_max));
                        poolManager.Spawn(prefab, spawnPos, transform);
                    }
                }
                break;
            case 5:
                {
                    GetComponent<EnemyController>().CAttack(false);
                    StartCoroutine(Delay(2f));
                    GetComponent<StatsModifier>().UpdateHealth(0, GetComponent<Health>().GetHealth()*0.5f,false,true);
                    Vector3 spawnPos = transform.position + transform.right * 2f;
                    poolManager.Spawn(prefab, spawnPos, transform);
                    poolManager.Spawn(prefab, spawnPos, transform);
                    spawnPos = transform.position + transform.right * -2f;
                    poolManager.Spawn(prefab, spawnPos, transform);
                    poolManager.Spawn(prefab, spawnPos, transform);
                    spawnPos = transform.position + transform.forward * 2f;
                    poolManager.Spawn(prefab, spawnPos, transform);
                    poolManager.Spawn(prefab, spawnPos, transform);
                    spawnPos = transform.position + transform.forward * -2f;
                    poolManager.Spawn(prefab, spawnPos, transform);
                    poolManager.Spawn(prefab, spawnPos, transform);
                }
                break;
        }
    }
    #endregion

    public void UpdateTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    public bool AtDamagingDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPos);
        return distanceToTarget <= damagingDistance;
    }
    IEnumerator jFinder()
    {
        j= Random.Range(1, 41);
        yield return new WaitForSeconds(iWaitTime);
        StartCoroutine(jFinder());
    }
    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<EnemyController>().CAttack(true);
        if(number==2)
            transform.position = tempPos;
    }
    void CanAttack()
    {
        canAttack = true;
    }
}
