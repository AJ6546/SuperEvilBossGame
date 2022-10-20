using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public Dictionary<string, int> damageTickTimer = new Dictionary<string, int>();
    Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }

    public void ApplyDamageOverTime(int ticks, float damage, float inBetweenTime, string type)
    {
        if (!damageTickTimer.ContainsKey(type))
        {
            damageTickTimer.Add(type, ticks);
            StartCoroutine(SetDamageOverTime(damage, inBetweenTime, type));
        }
        else
        {
            damageTickTimer[type] += ticks;
        }
    }
    IEnumerator SetDamageOverTime(float damage, float inBetweenTime, string damageType)
    {
        while (damageTickTimer.ContainsKey(damageType) && damageTickTimer[damageType] > 0)
        {
            damageTickTimer[damageType]--;
            health.TakeDamage(gameObject, damage);
            if (damageTickTimer[damageType] == 0)
            {
                damageTickTimer.Remove(damageType);
            }
            yield return new WaitForSeconds(inBetweenTime);
        }
    }
}
