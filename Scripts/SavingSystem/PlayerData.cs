using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string _userid;
    public string _email;
    public int _level;
    public int _doorsUnlocked;
    public int _experience;
    public int _health;
    public int _sceneToLoad;

    public PlayerData() { }

    public PlayerData(string userid, string usermail, int sceneToLoad,  int health, 
        int level,int experience, int doorsUnlocked)
    {
        _userid = userid;
        _email = usermail;
        _sceneToLoad = sceneToLoad;
        _health = health;
        _level = level;
        _experience = experience;
        _doorsUnlocked = doorsUnlocked;
    }
}