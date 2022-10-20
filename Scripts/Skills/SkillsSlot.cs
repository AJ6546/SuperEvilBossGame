using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsSlot : MonoBehaviour, IDragContainer<Skill>
{
    [SerializeField] SkillItem icon = null;
    PlayerSkills skills;
    [SerializeField] int index;
    Skill item = null;

    public void Setup(PlayerSkills skills, int index)
    {
        this.skills = skills;
        this.index = index;
        item = GetItem();
        icon.SetItem(skills.GetItemFromSlot(index), skills.GetNumberInSlot(index));
    }

    public Skill GetItem()
    {
        return skills.GetItemFromSlot(index);
    }
    public int GetNumber()
    {
        return skills.GetNumberInSlot(index);
    }
    public void RemoveItem(int number)
    {
        skills.RemoveFromSlot(index, number);
    }
    public void AddItems(Skill item, int number)
    {
        skills.AddItemToSlot(index, item, number);
    }
    public int MaxAcceptable(Skill item)
    {
        if (skills.HasSpaceForItem(item))
        { return int.MaxValue; }
        return 0;
    }
    public void UseItem(int number)
    {
        if (item != null)
        {
            item.Use();
            RemoveItem(number);
        }
    }
}
