using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayMessage : MonoBehaviour
{
    [SerializeField]
    FixedButton instructionButton;
    [SerializeField] GameObject instructionsUI;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex==7)
            instructionsUI.SetActive(true);
        else
            instructionsUI.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKey("f")|| instructionButton.Pressed)
        {
            instructionsUI.SetActive(true);
        }
    }
}
