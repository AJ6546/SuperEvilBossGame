using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    [SerializeField] SkillsSlot skillsSlotPrefab = null;
    PlayerSkills playerSkills;
    void Awake()
    {
        playerSkills = PlayerSkills.GetPlayerSkills();
        playerSkills.inventoryUpdated += Redraw;
    }

    void Start()
    {
        Redraw();
    }

    void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < playerSkills.GetSize(); i++)
        {
            var slotUi = Instantiate(skillsSlotPrefab, transform);
            slotUi.Setup(playerSkills, i);
        }
    }
}
