using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject coin;
    [SerializeField] float spawnRadius = 1f;
    [SerializeField] int initialSpawnCount =10, numberOfCoinsToSpawn=5;
    [SerializeField] List<GameObject> spawnedCoins = new List<GameObject>();
    [SerializeField] Vector3 spawnPos;
    void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            Spawn();
        }
    }

    void Update()
    {
        if(spawnedCoins.Count<=2)
        {
            Spawn(numberOfCoinsToSpawn);
        }
    }

    void Spawn(int number=1)
    {
        for (int i = 0; i < number; i++)
        {
            spawnPos = new Vector3(Random.Range(0, 50), 1, Random.Range(0, 50));
            foreach (GameObject coin in spawnedCoins)
            {
                if (FindDistance(spawnPos, coin.transform.position) <= spawnRadius)
                {
                    Spawn();
                    return;
                }
            }
            GameObject coinInstance = Instantiate(coin, spawnPos, coin.transform.rotation);
            spawnedCoins.Add(coinInstance);
        }
    }

    float FindDistance(Vector3 spawnPos, Vector3 coinPos)
    {
        return Vector3.Distance(spawnPos, coinPos);
    }

    public void RemoveFromList(GameObject coin)
    {
        spawnedCoins.Remove(coin);
    }
}
