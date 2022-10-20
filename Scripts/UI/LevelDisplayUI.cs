using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayUI : MonoBehaviour
{
    [SerializeField] CharacterStats characterStat;
    [SerializeField] Text levelText;
    void Awake()
    {
        characterStat = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }
    void Update()
    {
        levelText.text = characterStat.CurrentLevel().ToString();
    }
}
