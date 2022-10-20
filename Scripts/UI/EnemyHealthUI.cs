using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] TargetFinder player;
    [SerializeField] Health health;
    [SerializeField] Image foreground;
    [SerializeField] Text text;
    [SerializeField] Image targetIcon;
    [SerializeField] RectTransform targetHealthUI;
    void Update()
    {
        if (player.GetTarget().GetComponent<Health>())
        {
            health = player.GetTarget();
            targetIcon.sprite = health.GetComponent<CharacterStats>().GetIcon();
            if (!health.IsDead() && health.GetComponent<EnemyController>() && health.GetComponent<EnemyController>().IsInRange())
            {
                targetHealthUI.gameObject.SetActive(true);
                targetIcon.gameObject.SetActive(true);
            }
            else
            {
                targetHealthUI.gameObject.SetActive(false);
                targetIcon.gameObject.SetActive(false);
                return;

            }

            foreground.fillAmount = Mathf.Max(health.GetHealthFraction(), 0);
            text.text = (health.GetHealth()).ToString();
            
            targetHealthUI.sizeDelta = new Vector2(health.GetComponent<CharacterStats>().GetHealthUISize(),
                targetHealthUI.sizeDelta.y);
        }
    }
    void FindDist()
    {

    }
}
