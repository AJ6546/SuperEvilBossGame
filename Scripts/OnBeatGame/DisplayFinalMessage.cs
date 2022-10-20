using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFinalMessage : MonoBehaviour
{
    [SerializeField]
    FixedButton finalMessage;
    [SerializeField] GameObject finalMessageUI;
    void Start()
    {
        finalMessageUI.SetActive(true);
    }
    void Update()
    {
        if(Input.GetKey("f")|| finalMessage.Pressed)
        {
            finalMessageUI.SetActive(true);
        }
    }
}
