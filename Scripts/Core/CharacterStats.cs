using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Range(1, 60)]
    [SerializeField] int startingLevel=1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression=null;
    [SerializeField] int currentLevel = 1;
    [SerializeField] string levelUpParticleEffect=null;
    [SerializeField] int doorsUnlocked = 1;
    public event Action onLevelUp;
    PoolManager poolManager;
    PlayerStats playerStats;
    [SerializeField] SaveLoadManager slManager;
    private void Start()
    {
        playerStats = new PlayerStats();
        slManager = FindObjectOfType<SaveLoadManager>();
        poolManager =PoolManager.instance;
        if (CompareTag("Player"))
            currentLevel = Convert.ToInt32(playerStats.Level);
        else
            currentLevel = CalculateLevel();
        Experience experience = GetComponent<Experience>();
        if(experience!=null)
        {
            experience.onExperienceGained += UpdateLevel;
        }
    }
    private void UpdateLevel()
    {
        int newLevel= CalculateLevel();
        if(newLevel>currentLevel)
        {
            currentLevel = newLevel;
            if(CompareTag("Player"))
                LevelUpEffect();
            onLevelUp();
        }
    }

    private void LevelUpEffect()
    {
        poolManager.Spawn(levelUpParticleEffect, transform.position, transform);
        slManager.UpdateLevel(PlayerStats.USERID,
                   Convert.ToInt32(currentLevel));
    }

    public float GetStat(Stats stat)
    {
        return progression.GetStat(stat, characterClass, CalculateLevel());
    }
    public int GetLevel()
    {
        if(currentLevel<1)
        {
            currentLevel = CalculateLevel();
        }
        return currentLevel;
    }

    public Sprite GetIcon()
    {
        return progression.GetIcon(characterClass);
    }
    public float GetHealthUISize()
    {
        return progression.GetUIHealthSize(characterClass);
    }
    public int CalculateLevel()
    {
        Experience exp=GetComponent<Experience>();
        if (exp == null)
            return startingLevel;
       float currentXP = exp.GetExperience();
       int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
        for (int level=currentLevel; level< penultimateLevel; level++)
        {
            float XPToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
            if(XPToLevelUp>currentXP)
            {
                return level;
            }
        }
        GetComponentInChildren<LevelDisplay>().UpdateLevel(currentLevel);
        return penultimateLevel + 1;
    }
    public int GetDoorsUnloacked()
    {
        return doorsUnlocked;
    }
    public void SetDoorsUnloacked()
    {
        doorsUnlocked+=2;
        slManager.UpdateDoorsUnlocked(PlayerStats.USERID,doorsUnlocked);
    }
    public float CurrentLevel()
    {
        return currentLevel;
    }
}
