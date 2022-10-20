using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClickTargeting", menuName = "Abilities/Targeting/ClickTargeting", order = 0)]
public class ClickTargeting : Targeting
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float areaEffectRadius;
    [SerializeField] Transform targetingPrefab;
    Transform targetingPrefabInstance = null;
    public override void StartTargetting(GameObject user, Action<IEnumerable<GameObject>> finished, int index)
    {
        Skills skills = user.GetComponent<Skills>();
        skills.Targeting(index);
        PlayerController playerController = user.GetComponent<PlayerController>();
        playerController.StartCoroutine(Targeting(user, playerController,skills,finished));
    }
    public override void StopTargeting()
    {
        if (targetingPrefabInstance != null)
            targetingPrefabInstance.gameObject.SetActive(false);
    }
    IEnumerator Targeting(GameObject user, PlayerController playerController, Skills skills,Action<IEnumerable<GameObject>> finished)
    {
        if (targetingPrefabInstance == null)
            targetingPrefabInstance = Instantiate(targetingPrefab);
        else
            targetingPrefabInstance.gameObject.SetActive(true);
        targetingPrefabInstance.localScale = new Vector3(areaEffectRadius * 2, 1, areaEffectRadius * 2);
        while (true)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
            {
                targetingPrefabInstance.position = raycastHit.point;
                if (skills.IsTargeting() && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("yipikaye mudafukka");
                    yield return new WaitWhile(() => Input.GetMouseButtonDown(0));
                    finished(GetGameObjectsInRadius(raycastHit.point));
                    yield break;
                }
            }
            yield return null;
        }
        
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
    {
        RaycastHit[] hits=Physics.SphereCastAll(point, areaEffectRadius, Vector3.up, 0);
        foreach (var hit in hits)
        {
            yield return hit.collider.gameObject;
        }
    }

}
