using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawner : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    [SerializeField] string blood;
    private void Start()
    {
        poolManager = PoolManager.instance;
    }
    public void Spawn(float damage)
    {
        if (damage > 0)
        {
            poolManager.Spawn(blood, transform.position, transform,true);
        }
    }
}
