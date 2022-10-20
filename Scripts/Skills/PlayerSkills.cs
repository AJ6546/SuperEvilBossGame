using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] Skill item;
    private void Awake()
    {
        slots = new SkillsSlot[skillsSize];
        AddToFirstEmptySlot(item, 1);
    }
    [SerializeField] int skillsSize = 10;

    SkillsSlot[] slots;


    public struct SkillsSlot
    {
        public Skill item;
        public int number;
    }


    public event Action inventoryUpdated;

    internal int GetNumberInSlot(int slot)
    {
        return slots[slot].number;
    }

    public int GetSize()
    {
        return slots.Length;
    }

    public static PlayerSkills GetPlayerSkills()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<PlayerSkills>();
    }
    public Skill GetItemFromSlot(int slot)
    {
        return slots[slot].item;
    }
    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].number -= number;
        if (slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }
    public bool AddItemToSlot(int slot, Skill item, int number)
    {
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number);
        }
        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }
        slots[slot].item = item;
        slots[slot].number += number;

        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }
    public bool AddToFirstEmptySlot(Skill item, int number)
    {
        int i = FindSlot(item);
        if (i < 0)
        {
            return false;
        }
        slots[i].item = item;
        if (item.IsStackable())
        {
            slots[i].number += number;
        }
        else { slots[i].number += 1; }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        if (number > 1 && !item.IsStackable())
        {
            number--;
            AddToFirstEmptySlot(item, number);
        }
        return true;
    }

    int FindSlot(Skill item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            return FindEmptySlot();
        }
        return i;
    }

    int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }
    public bool HasSpaceForItem(Skill item)
    {
        return FindSlot(item) >= 0;
    }
    public int FindStack(Skill item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }
}
