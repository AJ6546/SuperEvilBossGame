using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] Health target = null;
    [SerializeField] float chaseDistance = 5f, suspicionTime = 5f, timeSinceLastSawTarget = Mathf.Infinity;
    [SerializeField] bool targetLocked = false, isBoss = false, canAttack = true;
    Vector3 targetPos;
    NPCMover nm;
    NPCFighter nf;
    [SerializeField] int enemyBehaviour;

    void Start()
    {
        nm = GetComponent<NPCMover>();
        nf = GetComponent<NPCFighter>();
        StartCoroutine(LockTarget());
    }
    void Update()
    {
        target = GetComponent<TargetFinder>().GetTarget();
        timeSinceLastSawTarget += Time.deltaTime;
        if (target != null)
        { targetPos = target.transform.position; }
        if (GetComponent<Health>().IsDead())
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            return;
        }

        if (targetLocked && canAttack)
        {
            timeSinceLastSawTarget = 0;
            transform.LookAt(target.transform);
            nm.NPCBehaviour(1, targetPos);
        }
        else if (timeSinceLastSawTarget <= suspicionTime)
        {
            nm.NPCBehaviour(3, targetPos);
        }
        else
        {
            nm.NPCBehaviour(0, targetPos);
        }
        nf.UpdateTargetPos(targetPos);
    }
    IEnumerator LockTarget()
    {

        targetLocked = IsInRange() && !target.IsDead() ? true : false;
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
       nf.AtackBehaviour(target);
    }
    public float GetChaseDist()
    {
        return chaseDistance;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
