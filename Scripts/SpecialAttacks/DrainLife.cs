using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainLife : MonoBehaviour
{
    [SerializeField] Transform instantiator = null;
    [SerializeField] Projectiles proj;
    [SerializeField] float radius=5f,damage, nextFireTime, cooldownTimer=5f;
    [SerializeField] private List<Health> enemiesWitinRadius;
    public PoolManager poolManager;
    [SerializeField] string burstEffect;
    [SerializeField] int multiplier = 10;
    private void Start()
    {
        poolManager = PoolManager.instance;
        if (GetComponent<Projectiles>())
        {
            proj = GetComponent<Projectiles>();
            instantiator = proj.GetInstantiator();
        }
        enemiesWitinRadius = new List<Health>();
    }
    private void Update()
    {
        GetEnemiesWitinRadius();
        if (enemiesWitinRadius.Count > 0)
        {
            if (Time.time > nextFireTime)
            {
                foreach (Health enemy in enemiesWitinRadius)
                {
                   enemy.TakeDamage(instantiator.gameObject, damage);
                   instantiator.GetComponent<StatsModifier>().UpdateHealth(0, multiplier * enemiesWitinRadius.Count, false, true);
                   poolManager.Spawn(burstEffect, transform.position, instantiator);
                }
                nextFireTime = Time.time + cooldownTimer;
            }
        }
    }

   
    #region GetEnemies
    public List<Health> GetEnemies()
    {
        List<Health> temp = new List<Health>();

        foreach (Health t in FindObjectsOfType<Health>())
        {
            if (t.GetComponent<Health>().GetHealth() > 0)
                temp.Add(t);
        }
        temp.Remove(instantiator.GetComponent<Health>());
        return temp;
    }
    #endregion
    #region EnemiesWithinRadius
    void GetEnemiesWitinRadius()
    {
        foreach (Health enemy in GetEnemies())
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= radius && !enemiesWitinRadius.Contains(enemy))
            {
                enemiesWitinRadius.Add(enemy);
            }
            else if (Vector3.Distance(enemy.transform.position, transform.position) > radius)
            {
                enemiesWitinRadius.Remove(enemy);
            }
        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
