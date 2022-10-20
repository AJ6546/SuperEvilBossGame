using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStore : MonoBehaviour
{
    Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
    private class DockedItemSlot
    {
        public Skill item;
        public int number;
        public SlotType skillType;
    }
    public event Action storeUpdated;

    public Skill GetAction(int index)
    {
        if (dockedItems.ContainsKey(index))
        {
            return dockedItems[index].item;
        }
        return null;
    }

    public int GetNumber(int index)
    {
        if (dockedItems.ContainsKey(index))
        {
            return dockedItems[index].number;
        }
        return 0;
    }
    public void AddAction(Skill item, int index, int number)
    {
        if (dockedItems.ContainsKey(index))
        {
            if (object.ReferenceEquals(item, dockedItems[index].item))
            {
                dockedItems[index].number += number;
            }
        }
        else
        {
            var slot = new DockedItemSlot();
            slot.item = item as Skill;
            slot.number = number;
            dockedItems[index] = slot;
        }
        if (storeUpdated != null)
        {
            storeUpdated();
        }
    }
    public bool Use(int index, GameObject user)
    {
        if (dockedItems.ContainsKey(index))
        {
            dockedItems[index].item.Use(user);
            if (dockedItems[index].item.isConsumable())
            {
                RemoveItem(index, 1);
            }
            return true;
        }
        return false;
    }
    public void RemoveItem(int index, int number)
    {
        if (dockedItems.ContainsKey(index))
        {
            dockedItems[index].number -= number;
            if (dockedItems[index].number <= 0)
            {
                dockedItems.Remove(index);
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

    }
    public int MaxAcceptable(Skill item, int index)
    {
        var actionItem = item as Skill;
        if (!actionItem) return 0;

        if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item))
        {
            return 0;
        }
        if (actionItem.isConsumable())
        {
            return int.MaxValue;
        }
        if (dockedItems.ContainsKey(index))
        {
            return 0;
        }

        return 1;
    }
    [System.Serializable]
    private struct DockedItemRecord
    {
        public string itemID;
        public int number;
    }
}
public enum SlotType
{
    Healing, Projectile, Trap, Boon, Create
}
