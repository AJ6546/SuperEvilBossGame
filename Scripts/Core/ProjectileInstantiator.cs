using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstantiator : MonoBehaviour
{
    public PoolManager poolManager;
    public Transform instantiatorTransform;
    void Start()
    {
        poolManager = PoolManager.instance;
    }

    public void SpawnProjectile(GameObject player, string projectile)
    {
        Vector3 spawnPos = instantiatorTransform.position;
        poolManager.Spawn(projectile, spawnPos,transform);
    }
    public void SpawnProjectile(GameObject player, string projectile, Vector3 spawnPos, bool rot)
    {
       poolManager.Spawn(projectile, spawnPos, transform, rot);
    }
}

