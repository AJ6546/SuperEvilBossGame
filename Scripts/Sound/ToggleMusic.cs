using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMusic : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] AudioSource music;
   
    void Start()
    {
        health = GetComponent<Health>();
        music = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (health.IsDead())
        {
            music.enabled = false;
        }
        else
        {
            music.enabled = true;
        }
    }
}
