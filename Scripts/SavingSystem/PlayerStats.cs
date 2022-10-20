using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static string USERID = "USERID", USERMAIL = "USERMAIL";
    static int SCENETOLOAD = 0, HEALTH=500, LEVEL=0,EXPERIENCE=0,DOORSUNLOCKED=1;
    public PlayerStats(string useid, string usermail, int sceneToLoad, int health,
        int level,int experience,int doorsUnlocked)
    {
        USERID = useid;
        USERMAIL = usermail;
        SCENETOLOAD = sceneToLoad;
        HEALTH = health;
        LEVEL = level;
        EXPERIENCE = experience;
        DOORSUNLOCKED = doorsUnlocked;
    }
    public PlayerStats() { }


    public int SceneToLoad
    {
        get { return SCENETOLOAD; }
        set { SCENETOLOAD = value; }
    }
    public int Health
    {
        get { return HEALTH; }
        set { HEALTH = value; }
    }
    public int Level
    {
        get { return LEVEL; }
        set { LEVEL = value; }
    }
    public int Experience
    {
        get { return EXPERIENCE; }
        set { EXPERIENCE = value; }
    }
    public int DoorsUnlocked
    {
        get { return DOORSUNLOCKED; }
        set { DOORSUNLOCKED = value; }
    }
}
