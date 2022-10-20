using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Health target = null;
    [SerializeField] float chaseDistance = 5f, suspicionTime = 5f, timeSinceLastSawTarget = Mathf.Infinity;
    [SerializeField] bool targetLocked = false, isBoss=false, canAttack=true;
    Vector3 targetPos;
    EnemyMover em;
    EnemyFighter ef;
    TargetFinder tf;
    [SerializeField] int enemyBehaviour;
 
    void Start()
    {
        em = GetComponent<EnemyMover>();
        ef = GetComponent<EnemyFighter>();
        tf = GetComponent<TargetFinder>();
        target = tf.GetTarget();
        StartCoroutine(LockTarget());
    }
    void Update()
    {
        timeSinceLastSawTarget += Time.deltaTime;
        if (target != null)
        { targetPos = target.transform.position; }
        if (GetComponent<Health>().IsDead() && GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            return;
        }
        target = tf.GetTarget();
        if (targetLocked && canAttack)
        {
            timeSinceLastSawTarget = 0;
            transform.LookAt(target.transform);
            em.EnemyBehaviour(1, targetPos);
        }
        else if (timeSinceLastSawTarget <= suspicionTime)
        {
            em.EnemyBehaviour(3, targetPos);
        }
        else
        {
            em.EnemyBehaviour(0, targetPos);
        }
        ef.UpdateTargetPos(targetPos);
    }
    IEnumerator LockTarget()
    {
        targetLocked = !GetComponent<Health>().IsDead() && !target.IsDead() && IsInRange() ? true : false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(LockTarget());
    }
    public bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= chaseDistance;
    }

    public Vector3 GetTargetPosition()
    {
        return target.transform.position;
    }

    public void StartAttacking()
    {
        if(isBoss)
            ef.BossAttackBehaviour(target);
        else
            ef.AtackBehaviour(target);
        
    }
    public float GetChaseDist()
    {
        return chaseDistance;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    public void CAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }
    public bool IsBoss()
    {
        return isBoss;
    }
}
