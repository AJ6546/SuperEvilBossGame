using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] float speed = 10f, deactivateAfter = 5f;
    [SerializeField] int damage;
    [SerializeField] Transform instantiator = null;
    [SerializeField] Vector3 targetPos, randomPos;
    [SerializeField] bool deactivateOnTouch=true;
    [SerializeField] bool followTarget=false, spawnAntherOnHit=false;
    public PoolManager poolManager;
    [SerializeField] string projName;
    [SerializeField] string destroyEffect = null;
    [SerializeField] bool randomMovement;
    [SerializeField] float xMax,zMax;

    void Start()
    {
        if (randomMovement)
            StartCoroutine(RandomMovement());
        poolManager = PoolManager.instance;
    }
    public void SetInstantiator(Transform instantiator)
    {
        this.instantiator = instantiator;
        targetPos = instantiator.position + instantiator.forward * 100 + transform.up + transform.right;
    }
    public GameObject GetInstantiatore()
    {
        return instantiator.gameObject;
    }
    void Update()
    {
        StartCoroutine(Deactivate(deactivateAfter));
        if (followTarget)
        {
            Vector3 tPos = instantiator.GetComponent<TargetFinder>().GetTarget().transform.position;
            targetPos = new Vector3(tPos.x, 1f, tPos.z);
            transform.LookAt(targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            return;
        }
        
        if (instantiator != null && !randomMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        if(randomMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);
        }
        if(GetComponent<Health>() && GetComponent<Health>().IsDead())
        {
            StartCoroutine(Deactivate(3f));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != instantiator.tag)
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().TakeDamage(instantiator.gameObject,damage);
                if (deactivateOnTouch)
                {
                    if(spawnAntherOnHit)
                    {
                        Vector3 spawnPos = new Vector3(Random.Range(0, 50), 5, Random.Range(0,50));
                        poolManager.Spawn(projName, spawnPos, instantiator);
                    }
                    StartCoroutine(Deactivate(0.1f));
                }
            }
        }
    }
    IEnumerator Deactivate(float time)
    {
        yield return new WaitForSeconds(time);
        if(destroyEffect != "")
        {
            poolManager.Spawn(destroyEffect, transform.position, instantiator);
        }
        if (GetComponent<Health>() && GetComponent<Health>().IsDead())
        {
            GetComponent<Health>().Live();
        }
        gameObject.SetActive(false);
    }
    public Transform GetInstantiator()
    {
        return instantiator;
    }
    IEnumerator RandomMovement()
    {
        float xPoint = Random.Range(0, xMax);
        float zPoint = Random.Range(0, zMax);
        randomPos = new Vector3(xPoint, transform.position.y, zPoint);
        yield return new WaitForSeconds(5f);
        StartCoroutine(RandomMovement());
    }
}
