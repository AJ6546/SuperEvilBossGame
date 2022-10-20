using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDamageOverTime : MonoBehaviour
{
    [SerializeField] int ticks;
    [SerializeField] string type;
    [SerializeField] float damage, inBetweenTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DamageOverTime>() && !other.CompareTag(tag))
        { other.GetComponent<DamageOverTime>().ApplyDamageOverTime(ticks, damage, inBetweenTime, type); }
    }
}
