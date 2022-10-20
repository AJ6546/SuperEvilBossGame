using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsModifier : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Fighter fighter;
    [SerializeField] ThirdPersonCharacter tpc;

    [SerializeField] float baseHealth, modifiedHealth;
    [SerializeField] float baseArmor, modifiedArmor;
    [SerializeField] float baseStrength, modifiedStrength;
    [SerializeField] float baseSpeed, modifiedSpeed;

    void Awake()
    {
        if (GetComponent<Health>())
        {
            health = GetComponent<Health>();
            baseHealth = health.GetStartHealth();
            baseArmor = health.GetBaseArmor();
        }
        if (GetComponent<Fighter>())
        {
            fighter = GetComponent<Fighter>();
            baseStrength = fighter.GetBaseStrength();
        }
        if (GetComponent<ThirdPersonCharacter>())
        {
            tpc = GetComponent<ThirdPersonCharacter>();
            baseSpeed = tpc.GetBaseSpeed();

        }
    }

    public void UpdateHealth(float oldHealthMod, float newHealthMod, bool isPercent, bool hasCap)
    { 
        modifiedHealth = health.HealthModifier(oldHealthMod, newHealthMod, isPercent, hasCap);
    }

    public void UpdateStrength(float mod, bool isPercent, float lastingTime)
    {
        modifiedStrength = fighter.DamageModifier(mod, isPercent, lastingTime);
    }

    public void UpdateArmor(float mod, bool isPercent, float lastingTime)
    {
        modifiedArmor = health.ArmorModifier(mod, isPercent, lastingTime);
    }
    public void UpdateSpeed(float oldSpeedMod, float newSpeedMod, float lastingTime)
    {
        modifiedSpeed = tpc.SpeedModifier(oldSpeedMod, newSpeedMod, lastingTime);
    }

    public void UpdateArmor(float modifiedArmor)
    {
        this.modifiedArmor = modifiedArmor;
    }
    public void UpdateStrength(float modifiedStrength)
    {
        this.modifiedStrength = modifiedStrength;
    }
    public void UpdateSpeed(float modifiedSpeed)
    {
        this.modifiedSpeed = modifiedSpeed;
    }
}
