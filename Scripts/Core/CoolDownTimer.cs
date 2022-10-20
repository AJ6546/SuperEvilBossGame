using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownTimer : MonoBehaviour
{
    [SerializeField] int[] attackTimers = new int[6];
    [SerializeField] int[] utilSkills = new int[5];
    public Dictionary<string, int> coolDownTime = new Dictionary<string, int>();
    public Dictionary<string, int> nextAttackTime = new Dictionary<string, int>();

    private void Start()
    {
        for (int i = 0; i < attackTimers.Length; i++)
        {
            coolDownTime["Attack0" + (i + 1).ToString()] = attackTimers[i];
            nextAttackTime["Attack0" + (i + 1).ToString()] = 0;
        }
        for (int i = 0; i < utilSkills.Length; i++)
        {
            coolDownTime["utilSkill0" + (i + 1).ToString()] = utilSkills[i];
            nextAttackTime["utilSkill0" + (i + 1).ToString()] = 0;
        }
    }
}
