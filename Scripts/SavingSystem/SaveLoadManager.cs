using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    PlayerData data;
    DatabaseReference reference;
    PlayerStats playerStats;
    int activeScene;
    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().buildIndex + 1;
    }
    public void SaveData(string userid, string usermail)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        data = new PlayerData(userid, usermail, activeScene, 500, 1, 0, 10);
        string jsonData = JsonUtility.ToJson(data);
        reference.Child("Player_" + userid).SetRawJsonValueAsync(jsonData);
        playerStats = new PlayerStats(userid, usermail,
            activeScene, 500,1,0,1);
    }
    public void LoadData(string userid)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.GetValueAsync().ContinueWith(
            task =>
            {
                if (task.IsCanceled) { Debug.Log("Loading data canceled"); return; }
                if (task.IsFaulted) { Debug.Log(task.Exception.Flatten().InnerExceptions[0].Message); }
                if (task.IsCompleted)
                {
                    DataSnapshot data = task.Result;
                    string playerData = data.Child("Player_" + userid).GetRawJsonValue();
                    PlayerData pd = JsonUtility.FromJson<PlayerData>(playerData);
                    playerStats = new PlayerStats(userid, pd._email,
                        pd._sceneToLoad, pd._health,pd._level,pd._experience,pd._doorsUnlocked);
                }
            }
            );
    }
    public void UpdateSceneToLoad(string userId, int sceneToLoad)
    {
        playerStats = new PlayerStats();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IDictionary<string, object> update = new Dictionary<string, object>();
        update["_sceneToLoad"] = sceneToLoad;
        reference.Child("Player_" + userId).UpdateChildrenAsync(update);
        playerStats.SceneToLoad = sceneToLoad;
    }
    public void UpdateHealth(string userId, int health)
    {
        playerStats = new PlayerStats();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IDictionary<string, object> update = new Dictionary<string, object>();
        update["_health"] = health;
        reference.Child("Player_" + userId).UpdateChildrenAsync(update);
        playerStats.SceneToLoad = health;
    }
    public void UpdateLevel(string userId, int level)
    {
        playerStats = new PlayerStats();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IDictionary<string, object> update = new Dictionary<string, object>();
        update["_level"] = level;
        reference.Child("Player_" + userId).UpdateChildrenAsync(update);
        playerStats.SceneToLoad = level;
    }
    public void UpdateExperience(string userId, int experience)
    {
        playerStats = new PlayerStats();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IDictionary<string, object> update = new Dictionary<string, object>();
        update["_experience"] = experience;
        reference.Child("Player_" + userId).UpdateChildrenAsync(update);
        playerStats.SceneToLoad = experience;
    }
    public void UpdateDoorsUnlocked(string userId, int doorsUnlocked)
    {
        playerStats = new PlayerStats();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        IDictionary<string, object> update = new Dictionary<string, object>();
        update["_doorsUnlocked"] = doorsUnlocked;
        reference.Child("Player_" + userId).UpdateChildrenAsync(update);
        playerStats.SceneToLoad = doorsUnlocked;
    }
}
