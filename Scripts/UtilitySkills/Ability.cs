
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 0)]
public class Ability : ScriptableObject
{
    public int id;
    public Targeting targeting;
    public virtual void Use(GameObject user, int index)
    {
        Debug.Log("Use Ability");
    }
    public void TargetAcquired(IEnumerable<GameObject> targets)
    {
        Debug.Log("Targets Acquired");
        targeting.StopTargeting();
        foreach (var target in targets)
        {
            Debug.Log(target);
        }
    }
}
