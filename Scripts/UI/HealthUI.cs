using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image foreground;
    [SerializeField] Text text;
    private void Awake()
    {
        health= GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        foreground.fillAmount = Mathf.Max(health.GetHealthFraction(), 0);
        text.text = (health.GetHealth()).ToString();
    }
}
