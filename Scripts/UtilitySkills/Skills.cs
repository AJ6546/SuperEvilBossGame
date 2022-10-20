using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public Ability[] currentAbilities = new Ability[3];
    public bool targeting=false;
    [SerializeField] bool hasHit = false;
    [SerializeField] Ray ray;
    [SerializeField] RaycastHit hit;
    [SerializeField] float forgetAfter;
    private void Awake()
    {
        for(int i=0;i<currentAbilities.Length;i++)
        {

        }
    }
    public void Use(GameObject user, int index)
    {   if (currentAbilities[index]!=null)
            currentAbilities[index].Use(user, index);
        else { Debug.Log("Skills is not good"); } 
    }
    void Update()
    {
        if (targeting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ResetTargeting(0.2f));
                }
            }
        }
        
    }
    public void Targeting(int index)
    {
        StartCoroutine(ResetTargeting(forgetAfter, index));
    }
    public bool IsTargeting()
    {
        return targeting;
    }
    public IEnumerator ResetTargeting(float time, int index=10)
    {
        yield return new WaitForSeconds(0.2f);
        targeting = true;
        yield return new WaitForSeconds(time);
        targeting = false;
        if(index<3)
            currentAbilities[index].targeting.StopTargeting();
    }
}
