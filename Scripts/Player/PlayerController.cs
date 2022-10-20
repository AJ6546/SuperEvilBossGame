using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoystick fixedJoystick;
    [SerializeField] FixedButton jumpButton, inventoryButton ,stabButton, logoutButton, muteButton, reloadButton;
    
    [SerializeField] FixedTouchField touchField;
    [SerializeField] ThirdPersonUserControl control;

    [SerializeField] Camera camera;
    [SerializeField] float cameraAngleX, cameraSpeed = 0.2f, rotOffset,touchRate;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] GameObject inventoryUI, skillsUI, messageBox;
    public bool isMoving;
    void Awake()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        control = GetComponent<ThirdPersonUserControl>();
        touchField = FindObjectOfType<FixedTouchField>();
        messageBox.gameObject.SetActive(false);
    }

    void Update()
    {
        cameraAngleX += touchField.TouchDist.x * cameraSpeed;
        camera.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * cameraOffset;
        camera.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * rotOffset - camera.transform.position, Vector3.up);
        if (touchField.TouchDist.x == 0)
        { cameraOffset.y -= touchField.TouchDist.y * touchRate; }
        if (GetComponent<Health>().IsDead())
        {
            reloadButton.gameObject.SetActive(true);
        }
        else
        {
            reloadButton.gameObject.SetActive(false);
        }
        if (GetComponent<Health>().IsDead() && reloadButton.Pressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (GetComponent<Health>().IsDead()) return;
        control.m_Jump = Input.GetKey("space") || jumpButton.Pressed;
        control.hInput = Input.GetAxis("Horizontal") + fixedJoystick.Horizontal;
        control.vInput = Input.GetAxis("Vertical") + fixedJoystick.Vertical;
        if (Input.GetKeyDown("-") || stabButton.Pressed)
        {
            GetComponent<Health>().TakeDamage(gameObject, 5);
        }
        if (Input.GetKeyDown("i") || inventoryButton.Pressed)
        {
            inventoryUI.SetActive(true);
        }
        if (fixedJoystick.Horizontal != 0f || fixedJoystick.Vertical != 0f ||
            Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        bool logout = Input.GetKey("escape") || logoutButton.Pressed;
        if (logout)
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }
        bool mute = Input.GetKey("m") || muteButton.Pressed;
        if(mute)
        {
            messageBox.SetActive(true);
            StartCoroutine(HideMessage());
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            FindObjectOfType<CoinSpawner>().RemoveFromList(other.gameObject);
            Destroy(other.gameObject, 0.2f);
        }
    }
    public static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2f);
        messageBox.SetActive(false);
    }
}
