using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedSkillSlot : MonoBehaviour, IDragContainer<Skill>
{
    [SerializeField] SkillItem icon = null;
    [SerializeField] int index = 0;

    // CACHE
    SkillStore store;

    // LIFECYCLE METHODS
    private void Awake()
    {
        store = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillStore>();
        store.storeUpdated += UpdateIcon;
    }

    // PUBLIC

    public void AddItems(Skill item, int number)
    {
        store.AddAction(item, index, number);
    }

    public Skill GetItem()
    {
        return store.GetAction(index);
    }

    public int GetNumber()
    {
        return store.GetNumber(index);
    }

    public int MaxAcceptable(Skill item)
    {
        return store.MaxAcceptable(item, index);
    }

    public void RemoveItem(int number)
    {
        store.RemoveItem(index, number);
    }

    void UpdateIcon()
    {
        icon.SetItem(GetItem(), GetNumber());
    }
}
