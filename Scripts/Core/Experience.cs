using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints=0;
    [SerializeField] float maxExperience = 100;
    [SerializeField] SaveLoadManager slManager;
    public event Action onExperienceGained;
    [SerializeField] PlayerStats playerStats;
    private void Start()
    {
        if (CompareTag("Player"))
        {
            playerStats = new PlayerStats();
            experiencePoints = playerStats.Experience;
            slManager = FindObjectOfType<SaveLoadManager>();
        }
        maxExperience = GetComponent<CharacterStats>().GetStat(Stats.ExperienceToLevelUp);
    }
    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        if(CompareTag("Player"))
            slManager.UpdateExperience(PlayerStats.USERID, Convert.ToInt32(experiencePoints));
        onExperienceGained();
        maxExperience = GetComponent<CharacterStats>().GetStat(Stats.ExperienceToLevelUp);
    }
    public float GetExperienceFraction()
    {
        return experiencePoints / maxExperience;
    }
    public float GetExperience()
    {
        return experiencePoints;
    }
    public float GetMaxExperience()
    {
        return maxExperience;
    }
}
