using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] Experience experience;
    [SerializeField] Text experiecneText;
    [SerializeField] Image foreground;
    void Awake()
    {
        experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
    }

    void Update()
    {
        foreground.fillAmount = Mathf.Min(experience.GetExperienceFraction(), 1);
        experiecneText.text = experience.GetExperience().ToString()+"/"+ experience.GetMaxExperience();
    }
}
