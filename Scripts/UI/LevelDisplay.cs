using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Canvas rootCanvas=null;
    [SerializeField] Health health;
    [SerializeField] PlayerController player;
    void Start()
    {
        rootCanvas.enabled = false;
        health = GetComponentInParent<Health>();
        player = FindObjectOfType<PlayerController>();
        levelText.text = "Level: 1";
    }
    void Update()
    {
        if (player.GetComponent<TargetFinder>().GetTarget() == health)
        {
            rootCanvas.enabled = true;
        }
        else
        {
            rootCanvas.enabled = false;
        }
    }
    public void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level.ToString();
    }
}
