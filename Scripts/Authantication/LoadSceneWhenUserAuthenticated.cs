using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWhenUserAuthenticated : MonoBehaviour
{
    SaveLoadManager slManager;
    string userId = "";
    PlayerStats playerStats;
    private void Start()
    {
        playerStats = new PlayerStats();
        slManager = GetComponent<SaveLoadManager>();
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckUser();
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        CheckUser();
    }

    private void CheckUser()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        slManager.LoadData(userId);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(Convert.ToInt32(playerStats.SceneToLoad));
    }
}
