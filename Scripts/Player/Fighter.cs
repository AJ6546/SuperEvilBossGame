using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField]
    float dmgFactor01 = 50, dmgFactor02 = 25, dmgFactor03 = 20, dmgFactor04=10, dmgFactor05=5,
        strength = 100, attackRange = 3f,evilDamage=20f;
    public Animator playerAnimator;
    [SerializeField] FixedButton attack01Button, attack02Button, attack03Button, attack04Button, attack05Button,
        utilSkill01Button, utilSkill02Button, utilSkill03Button, utilSkill04Button, utilSkill05Button,evilButton;
    [SerializeField] CoolDownTimer cd;
    [SerializeField] float strengthModifier = 0, fleetingStrengthModifier = 0;
    [SerializeField] GameObject attack01Fill, attack02Fill, attack03Fill, attack04Fill, attack05Fill, 
        utilSkill01Fill, utilSkill02Fill, utilSkill03Fill, utilSkill04Fill, utilSkill05Fill, evilFill;
    [SerializeField] string projectile,trap,stream,summon, summonEffect;
    [SerializeField] Skills skill;
    [SerializeField] ProjectileInstantiator projectileInstantiator;
    [SerializeField] bool isFirstPress=true;
    [SerializeField] float tempSpeed,moveSpeed, speedMultiplier = 5f, streamTime=5f;
    ThirdPersonCharacter tpc;
    [SerializeField] bool canAttack=true;
    [SerializeField] string[] sfx = new string[10];
    public SoundManager soundManager;
    void Start()
    {
        tpc = GetComponent<ThirdPersonCharacter>();
        tempSpeed = tpc.m_MoveSpeedMultiplier;
        playerAnimator = GetComponent<Animator>();
        cd = GetComponent<CoolDownTimer>();
        GetComponent<CharacterStats>().onLevelUp += ModifyStrength;
        strength = GetComponent<CharacterStats>().GetStat(Stats.Strength);
        skill = GetComponent<Skills>();
        projectileInstantiator = GetComponent<ProjectileInstantiator>();
        soundManager = SoundManager.instance;
    }

    private void ModifyStrength()
    {
        strength = GetComponent<CharacterStats>().GetStat(Stats.Strength);
    }

    void Update()
    {
        Refill();
        if (GetComponent<Health>().IsDead()) return;

    
        target = GetComponent<TargetFinder>().GetTarget().gameObject;
        if(target.GetComponent<AnimalMover>())
        {
            evilButton.gameObject.SetActive(true);
        }
        else
        {
            evilButton.gameObject.SetActive(false);
        }
        Attack();
        Skills();
        Evil();
    }
    #region EvilAttack
    private void Evil()
    {
        if (cd.nextAttackTime["Attack06"]
             < Time.time && (Input.GetKeyDown("x") || evilButton.Pressed) && canAttack)
        {
            soundManager.Play(sfx[10], transform.position);
            playerAnimator.SetTrigger("EvilAttack");
            canAttack = false;
            cd.nextAttackTime["Attack06"] = cd.coolDownTime["Attack06"] + (int)Time.time;
            evilFill.GetComponent<Image>().fillAmount = 1;
        }
    }
    void Evil_Hit()
    {
        
        if (InAttackRange())
        {
            if (target.GetComponent<AnimalMover>())
            { target.GetComponent<AnimalMover>().Kick(); }
            target.GetComponent<Health>().TakeDamage(gameObject, evilDamage);
        }
    }
    #endregion

    #region Attack
    private void Attack()
    {
        if (cd.nextAttackTime["Attack01"]
            < Time.time && (Input.GetKeyDown("1") || attack01Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[0], transform.position);
            playerAnimator.SetTrigger("Attack01");
            canAttack = false;
            cd.nextAttackTime["Attack01"] = cd.coolDownTime["Attack01"] + (int)Time.time;
            attack01Fill.GetComponent<Image>().fillAmount = 1;
        }
        if (cd.nextAttackTime["Attack02"]
            < Time.time && (Input.GetKeyDown("2") || attack02Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[2], transform.position);
            playerAnimator.SetTrigger("Attack02");
            canAttack = false;
            cd.nextAttackTime["Attack02"] = cd.coolDownTime["Attack02"] + (int)Time.time;
            attack02Fill.GetComponent<Image>().fillAmount = 1;
        }
        if (cd.nextAttackTime["Attack03"]
            < Time.time && (Input.GetKeyDown("3") || attack03Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[4], transform.position);
            playerAnimator.SetTrigger("Attack03");
            canAttack = false;
            cd.nextAttackTime["Attack03"] = cd.coolDownTime["Attack03"] + (int)Time.time;
            attack03Fill.GetComponent<Image>().fillAmount = 1;
        }
        if (cd.nextAttackTime["Attack04"]
           < Time.time && (Input.GetKeyDown("4") || attack04Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[0], transform.position);
            playerAnimator.SetTrigger("Attack04");
            canAttack = false;
            cd.nextAttackTime["Attack04"] = cd.coolDownTime["Attack04"] + (int)Time.time;
            attack04Fill.GetComponent<Image>().fillAmount = 1;
        }

        if (cd.nextAttackTime["Attack05"]
           < Time.time && (Input.GetKeyDown("5") || attack05Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[4], transform.position);
            playerAnimator.SetTrigger("Attack05");
            canAttack = false;
            cd.nextAttackTime["Attack05"] = cd.coolDownTime["Attack05"] + (int)Time.time;
            attack05Fill.GetComponent<Image>().fillAmount = 1;
        }
    }
    #endregion
    #region Skills
    private void Skills()
    {
        if (cd.nextAttackTime["utilSkill01"]
            < Time.time && (Input.GetKeyDown("6") || utilSkill01Button.Pressed) && canAttack)
        {
            if (Mathf.Approximately(GetComponent<Health>().GetHealthFraction(), 1))
                return;
            soundManager.Play(sfx[5], transform.position);
            playerAnimator.SetTrigger("UtilSkill01");
            canAttack = false;
            cd.nextAttackTime["utilSkill01"] = cd.coolDownTime["utilSkill01"] + (int)Time.time;
            utilSkill01Fill.GetComponent<Image>().fillAmount = 1;
        }
        if (cd.nextAttackTime["utilSkill02"]
            < Time.time && (Input.GetKeyDown("7") || utilSkill02Button.Pressed) && canAttack)
        {
            playerAnimator.SetTrigger("UtilSkill02");
            canAttack = false;
            cd.nextAttackTime["utilSkill02"] = cd.coolDownTime["utilSkill02"] + (int)Time.time;
            utilSkill02Fill.GetComponent<Image>().fillAmount = 1;
        }

        if (cd.nextAttackTime["utilSkill03"]
           < Time.time && (Input.GetKeyDown("8") || utilSkill03Button.Pressed) && canAttack)
        {
            soundManager.Play(sfx[7], transform.position);
            playerAnimator.SetTrigger("UtilSkill03");
            canAttack = false;
            cd.nextAttackTime["utilSkill03"] = cd.coolDownTime["utilSkill03"] + (int)Time.time;
            utilSkill03Fill.GetComponent<Image>().fillAmount = 1;
        }

        if (cd.nextAttackTime["utilSkill04"]
           < Time.time && (Input.GetKeyDown("9") || utilSkill04Button.Pressed))
        {
            moveSpeed = tpc.m_MoveSpeedMultiplier;
            soundManager.Play(sfx[8], transform.position);
            if (GetComponent<PlayerController>().isMoving)
            {
                Hit09();
            }
            if (isFirstPress)
            { StartCoroutine(StopHit09(streamTime)); }
        }

        if (cd.nextAttackTime["utilSkill05"]
           < Time.time && (Input.GetKeyDown("0") || utilSkill05Button.Pressed) && canAttack)
        {
            playerAnimator.SetTrigger("UtilSkill05");
            canAttack = false;
            cd.nextAttackTime["utilSkill05"] = cd.coolDownTime["utilSkill05"] + (int)Time.time;
            utilSkill05Fill.GetComponent<Image>().fillAmount = 1;
        }
    }
    #endregion
    public float GetBaseStrength()
    {
        return strength;
    }

    public void SetAttackButtonImages(Sprite[] img)
    {
        if(img[0]!=null)
            attack01Button.GetComponent<Image>().sprite = img[0];
        if (img[1] != null)
            attack02Button.GetComponent<Image>().sprite = img[1];
        if (img[2] != null)
            attack03Button.GetComponent<Image>().sprite = img[2];
        if (img[3] != null)
            attack04Button.GetComponent<Image>().sprite = img[3];
        if (img[4] != null)
            attack05Button.GetComponent<Image>().sprite = img[4];
    }
    #region Damage
    bool InAttackRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    #region Attacks
    void Hit01()
    {
        if (InAttackRange())
        {
            target.GetComponent<Health>().TakeDamage(gameObject,(strength + strengthModifier + fleetingStrengthModifier)
              / dmgFactor01);
        }
    }
    void Hit02()
    {
        if (InAttackRange())
        {
            target.GetComponent<Health>().TakeDamage(gameObject,(strength + strengthModifier + fleetingStrengthModifier)
              / dmgFactor02);
        }
    }
    void Hit03()
    {
        if (InAttackRange())
        {
            target.GetComponent<Health>().TakeDamage(gameObject,(strength + strengthModifier + fleetingStrengthModifier)
              / dmgFactor03);
        }
    }
    void Hit04()
    {
        if (InAttackRange())
        {
            target.GetComponent<Health>().TakeDamage(gameObject,(strength + strengthModifier + fleetingStrengthModifier)
              / dmgFactor04);
        }
    }
    void Hit05()
    {
        
        if (InAttackRange())
        {
            target.GetComponent<Health>().TakeDamage(gameObject,(strength + strengthModifier + fleetingStrengthModifier)
              / dmgFactor05);
        }
    }
    #endregion
    #region UtilitySkillS
    void Hit06()
    {
        GetComponent<StatsModifier>().UpdateHealth(0, 50f, true, true);
    }
    void Hit07()
    {
        soundManager.Play(sfx[6], transform.position);
        soundManager.Play(sfx[6], transform.position);
        projectileInstantiator.SpawnProjectile(gameObject,projectile);
    }
    void Hit08()
    {
        projectileInstantiator.SpawnProjectile(gameObject, trap);
    }
    void Hit09()
    {
        tpc.m_MoveSpeedMultiplier = tempSpeed * speedMultiplier;
        projectileInstantiator.SpawnProjectile(gameObject,stream, transform.position, true);
    }
    private IEnumerator StopHit09(float time)
    {
        isFirstPress = false;
        yield return new WaitForSeconds(time);
        tpc.m_MoveSpeedMultiplier = tempSpeed;
        cd.nextAttackTime["utilSkill04"] = cd.coolDownTime["utilSkill04"] + (int)Time.time;
        utilSkill04Fill.GetComponent<Image>().fillAmount = 1;
        attack02Fill.GetComponent<Image>().fillAmount = 1;
        isFirstPress = true;
    }
    void Hit10()
    {
        projectileInstantiator.SpawnProjectile(gameObject, summonEffect, transform.position + transform.right * 2f, false);
        StartCoroutine(Summon());
    }
    IEnumerator Summon()
    {
        yield return new WaitForSeconds(.5f);
        soundManager.Play(sfx[9], transform.position);
        projectileInstantiator.SpawnProjectile(gameObject, summon, transform.position + transform.right * 2f, true);
    }
    #endregion
    public float DamageModifier(float damageModifier, bool isPercent, float lastingTime)
    {
        if (isPercent)
        {
            float modifier = strength * damageModifier / 100;

            if (lastingTime > 0)
            {
                fleetingStrengthModifier += modifier;
                StartCoroutine(PowerDown(lastingTime, modifier));
            }
            else
            {
                strengthModifier = modifier;
            }
        }
        else
        {
            if (lastingTime > 0)
            {
                fleetingStrengthModifier += damageModifier;
                StartCoroutine(PowerDown(lastingTime, damageModifier));
            }
            else
            { strengthModifier = damageModifier; }
        }
        return (strength + strengthModifier + fleetingStrengthModifier);
    }
    IEnumerator PowerDown(float lastingTime, float damageModifier)
    {
        yield return new WaitForSeconds(lastingTime);
        fleetingStrengthModifier -= damageModifier;
        GetComponent<StatsModifier>().UpdateStrength(strength + strengthModifier + fleetingStrengthModifier);
    }
    #endregion
    #region Refill
    void Refill()
    {
        #region Attacks
        if (attack01Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack01Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["Attack01"]) * Time.deltaTime;
            attack01Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack01"] - (int)Time.time,0)).ToString();
        }
        else { attack01Fill.GetComponentInChildren<Text>().text = ""; }
        if (attack02Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack02Fill.GetComponent<Image>().fillAmount -=
            (1.0f / cd.coolDownTime["Attack02"]) * Time.deltaTime;
            attack02Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack02"] - (int)Time.time,0)).ToString();
        }
        else { attack02Fill.GetComponentInChildren<Text>().text = ""; }
        if (attack03Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack03Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["Attack03"]) * Time.deltaTime;
            attack03Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack03"] - (int)Time.time,0)).ToString();
        }
        else { attack03Fill.GetComponentInChildren<Text>().text = ""; }

        if (attack04Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack04Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["Attack04"]) * Time.deltaTime;
            attack04Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack04"] - (int)Time.time,0)).ToString();
        }
        else { attack04Fill.GetComponentInChildren<Text>().text = ""; }

        if (attack05Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack05Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["Attack05"]) * Time.deltaTime;
            attack05Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack05"] - (int)Time.time,0)).ToString();
        }
        else { attack05Fill.GetComponentInChildren<Text>().text = ""; }
        #endregion
        #region UtilitySkill
        if (utilSkill01Fill.GetComponent<Image>().fillAmount > 0)
        {
            utilSkill01Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["utilSkill01"]) * Time.deltaTime;
            utilSkill01Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["utilSkill01"] - (int)Time.time,0)).ToString();
        }
        else { utilSkill01Fill.GetComponentInChildren<Text>().text = ""; }
        if (utilSkill02Fill.GetComponent<Image>().fillAmount > 0)
        {
            utilSkill02Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["utilSkill02"]) * Time.deltaTime;
            utilSkill02Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["utilSkill02"] - (int)Time.time,0)).ToString();
        }
        else { utilSkill02Fill.GetComponentInChildren<Text>().text = ""; }

        if (utilSkill03Fill.GetComponent<Image>().fillAmount > 0)
        {
            utilSkill03Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["utilSkill03"]) * Time.deltaTime;
            utilSkill03Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["utilSkill03"] - (int)Time.time,0)).ToString();
        }
        else { utilSkill03Fill.GetComponentInChildren<Text>().text = ""; }


        if (utilSkill04Fill.GetComponent<Image>().fillAmount > 0)
        {
            utilSkill04Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["utilSkill04"]) * Time.deltaTime;
            utilSkill04Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["utilSkill04"] - (int)Time.time,0)).ToString();
        }
        else { utilSkill04Fill.GetComponentInChildren<Text>().text = ""; }


        if (utilSkill05Fill.GetComponent<Image>().fillAmount > 0)
        {
            utilSkill05Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["utilSkill05"]) * Time.deltaTime;
            utilSkill05Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["utilSkill05"] - (int)Time.time,0)).ToString();
        }
        else { utilSkill05Fill.GetComponentInChildren<Text>().text = ""; }
        #endregion
        #region Evil
        if (evilFill.GetComponent<Image>().fillAmount > 0)
        {
            evilFill.GetComponent<Image>().fillAmount -=
                (1.0f / cd.coolDownTime["Attack06"]) * Time.deltaTime;
            evilFill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cd.nextAttackTime["Attack06"] - (int)Time.time, 0)).ToString();
        }
        else { evilFill.GetComponentInChildren<Text>().text = ""; }
        #endregion
    }
    public void ResetFill()
    {
        if (attack01Fill != null && attack02Fill != null && attack03Fill != null && 
            attack04Fill != null && attack05Fill != null && utilSkill01Fill != null && utilSkill02Fill!=null
            && utilSkill03Fill != null && utilSkill04Fill != null && utilSkill05Fill != null)
        {
            attack01Fill.GetComponent<Image>().fillAmount = 0;
            attack02Fill.GetComponent<Image>().fillAmount = 0;
            attack03Fill.GetComponent<Image>().fillAmount = 0;
            attack04Fill.GetComponent<Image>().fillAmount = 0;
            attack05Fill.GetComponent<Image>().fillAmount = 0;
            utilSkill01Fill.GetComponent<Image>().fillAmount = 0;
            utilSkill02Fill.GetComponent<Image>().fillAmount = 0;
            utilSkill03Fill.GetComponent<Image>().fillAmount = 0;
            utilSkill04Fill.GetComponent<Image>().fillAmount = 0;
            utilSkill05Fill.GetComponent<Image>().fillAmount = 0;

        }
    }
    #endregion
    void CanAttack()
    {
        canAttack = true;
    }
}
