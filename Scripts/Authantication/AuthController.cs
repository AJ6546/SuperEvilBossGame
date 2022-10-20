using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AuthController : MonoBehaviour
{
    [SerializeField] Text email, password;
    [SerializeField] TextMeshProUGUI message;
    FirebaseAuth auth;
    SaveLoadManager slManager;
    [SerializeField] Image screan;
    bool userAuthenticated;
    [SerializeField] GameObject introPlayer, background,controlsUi, creditsUi;
    [SerializeField] float introPlayTime = 111f;
    private void Awake()
    {
        userAuthenticated = FirebaseAuth.DefaultInstance.CurrentUser != null ? true : false;
    }


    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        slManager = GetComponent<SaveLoadManager>();
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        if (userAuthenticated)
        {
            screan.gameObject.SetActive(true);
            message.text = "Logging in as " + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            yield return new WaitForSeconds(1f);
            message.text = "Fetching data ";
        }
        else
        {
            screan.gameObject.SetActive(false);
            message.text = "Please Log in";
        }
    }

    public void Login()
    {
        if (email.text == "" || password.text == "")
        {
            message.text = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
           task =>
           {
               if (task.IsCanceled)
               {
                   message.text = "LogIn cancled";
                   return;
               }
               if (task.IsFaulted)
               {
                   message.text = task.Exception.Flatten().InnerExceptions[0].Message;
                   return;
               }
               if (task.IsCompleted)
               {
                   FirebaseUser user = task.Result;
                   message.text = "Logged in as " + user.Email;
               }
           });
    }


    public void Register()
    {
        if (email.text == "" || password.text == "")
        {
            message.text = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    message.text = "Registeration cancled";
                    return;
                }
                if (task.IsFaulted)
                {
                    print(task.Exception);
                    message.text = task.Exception.Flatten().InnerExceptions[0].Message;
                    return;
                }
                if (task.IsCompleted)
                {
                    FirebaseUser newUser = task.Result;
                    slManager.SaveData(newUser.UserId, newUser.Email);
                    message.text = "User Created Successfully\nEmail: " + newUser.Email + "\nUserId: " + newUser.UserId;
                    return;
                }
            }
            );
    }
    public void AnonymousLogin()
    {
        int randomNo = Random.Range(0, 99999);
        string guestId = "Guest" + randomNo.ToString() + "@EvilBossGame.com";
        auth.SignInAnonymouslyAsync().ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    message.text = "LogIn cancled";
                    return;
                }
                if (task.IsFaulted)
                {
                    message.text = task.Exception.Flatten().InnerExceptions[0].Message;
                    return;
                }
                if (task.IsCompleted)
                {
                    FirebaseUser anonymousUser = task.Result;
                    slManager.SaveData(anonymousUser.UserId, guestId);
                    message.text = "User Logged in anonymously\nGuestID: " + guestId + "\nUserId: " + anonymousUser.UserId;
                    return;
                }
            });
    }
    public void Logout()
    {
        if (auth.CurrentUser != null)
        {
            message.text = "User " + auth.CurrentUser.UserId + " Logged out";
            auth.SignOut();
        }
        StartCoroutine(Quit());
    }
    private void Update()
    {
        message.SetAllDirty();
    }
    IEnumerator Quit()
    {
        message.text = "Exiting App";
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
    public void PlayIntro()
    {
        background.SetActive(false);
        introPlayer.GetComponent<VideoPlayer>().Play();
        StartCoroutine(Play());
    }
    public void Controls()
    {
        controlsUi.SetActive(true);
    }
    public void Credits()
    {
        creditsUi.SetActive(true);
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(introPlayTime);
        background.SetActive(true);
    }
}
