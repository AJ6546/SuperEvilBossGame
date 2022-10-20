using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] int buildIndex;
    [SerializeField] bool nextScene;
    [SerializeField] SaveLoadManager slManager;
    string userId = "";
    PlayerStats playerStats;
    private void Start()
    {
        slManager = FindObjectOfType<SaveLoadManager>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        playerStats = new PlayerStats();
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
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (nextScene)
            {
                slManager.LoadData(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
                slManager.UpdateSceneToLoad(PlayerStats.USERID,buildIndex + 1);
                StartCoroutine(LoadScene(buildIndex + 1));
            }
            else
            {
                slManager.LoadData(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
                slManager.UpdateSceneToLoad(PlayerStats.USERID, buildIndex - 1);
                StartCoroutine(LoadScene(buildIndex-1)); }
        }
    }

    IEnumerator LoadScene(int scenetToLoad)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scenetToLoad);
    }
}
