using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Skill"))]
public class Skill : Item
{
    [SerializeField] bool consumable = false;
    [SerializeField] string projectile="";
    public virtual void Use(GameObject user)
    {
        Debug.Log("Using action: " + this);
    }

    public bool isConsumable()
    {
        return consumable;
    }
}
