using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeting : ScriptableObject
{
    public abstract void StartTargetting(GameObject user,Action<IEnumerable<GameObject>> finished, int index);
    public abstract void StopTargeting();
}
