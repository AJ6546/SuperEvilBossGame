using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Progression", menuName ="Stats/Progression",order =0)]
public class Progression: ScriptableObject
{

    [SerializeField] ProgressionCharacterClass[] characterClasses;

    Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;
    public float GetStat(Stats stat, CharacterClass characterClass, int level)
    {
        BuildLookup();
        float[] levels = lookupTable[characterClass][stat];
        if (levels.Length < level) return 0;
        else return levels[level-1];
    }
    public int GetLevels(Stats stat, CharacterClass characterClass)
    {
        BuildLookup();
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }
    private void BuildLookup()
    {
        if (lookupTable != null) return;
        lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();
        foreach(ProgressionCharacterClass progressionClass in characterClasses)
        {
            var statLookupTable = new Dictionary<Stats, float[]>();
            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levels;
            }
            lookupTable[progressionClass.characterClass] = statLookupTable;
        }
    }

    public Sprite GetIcon(CharacterClass characterClass)
    {
        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            if (progressionClass.characterClass == characterClass)
            {
                return progressionClass.characterIcon;
            }
        }
        return null;
    }

    
    public float GetUIHealthSize(CharacterClass characterClass)
    {
        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            if (progressionClass.characterClass == characterClass)
            {
                return progressionClass.UIHealthSize;
            }
        }
        return 0;
    }

    public float GetExperience()
    {
        return 10;
    }
    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        
        public ProgressionStat[] stats;
        public Sprite characterIcon;
        public float UIHealthSize;
    }
    [System.Serializable]
    class ProgressionStat
    {
        public Stats stat; 
        public float[] levels;
    }
}
