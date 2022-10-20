
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    float healthPoints, startHealth = 500, maxHealth,
            healTime = 10, healthRefillRate = 0.1f, healAfter = 0, regeneratePercent=70;
    [SerializeField] float armor = 0;
    [SerializeField] bool isPercent, isDead = false;
    [SerializeField] string healthUpEffect="", deathEffect="";
    Animator animator;
    [SerializeField] UnityEvent<float> takeDamage;
    [SerializeField] PoolManager poolManager;
    [SerializeField] GameObject door;
    [SerializeField] string voice, musicbit;
    [SerializeField] SoundManager soundManager;
    [SerializeField] SaveLoadManager slManager;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] string damageSound="", deathSound="";
    void Start()
    {
        
        GetComponent<CharacterStats>().onLevelUp += RegenerateHealth;
        maxHealth = startHealth = healthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health);
        animator = GetComponent<Animator>();
        poolManager = PoolManager.instance;
        soundManager = SoundManager.instance;
        if (door!=null)
        {
            door.SetActive(false);
        }
        if (CompareTag("Player"))
        {
            slManager = FindObjectOfType<SaveLoadManager>();
            playerStats = new PlayerStats();
            maxHealth = startHealth = healthPoints = playerStats.Health;
        }
    }

    private void RegenerateHealth()
    {
        maxHealth = healthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health);
        float regenHealthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health) * regeneratePercent / 100;
        healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        maxHealth = Mathf.Max(maxHealth, regenHealthPoints);
        if (CompareTag("Player"))
            slManager.UpdateHealth(PlayerStats.USERID, System.Convert.ToInt32(healthPoints));
    }

    void Update()
    {
        if (healthPoints < maxHealth && Time.time > healAfter)
        {
            healthPoints += healthRefillRate;
            if (isDead && Mathf.RoundToInt(healthPoints) >= Mathf.RoundToInt(maxHealth * 0.8f))
            {
                isDead = !isDead;
                if (GetComponent<Animator>())
                    animator.SetTrigger("Resurrect");
                if (GetComponent<NavMeshAgent>())
                    GetComponent<NavMeshAgent>().isStopped = false;
            }
        }
    }

    IEnumerator Relocate()
    {
        yield return new WaitForSeconds(1f);
        if (deathEffect != "")
            poolManager.Spawn(deathEffect, transform.position, transform);
        yield return new WaitForSeconds(0.1f);
        if (CompareTag("Enemy"))
        {
            Vector3 newPos = new Vector3(Random.Range(0, 100), transform.position.y, Random.Range(0, 100));
            transform.position = newPos;
        }
    }
    public float GetStartHealth()
    {
        return startHealth;
    }
    public void TakeDamage(GameObject instigator,float damage)
    {
        if (isDead)
            return;
        damage = GetDamage(damage);
        healthPoints = Mathf.Lerp(healthPoints, Mathf.Max(healthPoints - damage,0), Time.deltaTime * 1000);
        healAfter = Time.time + healTime;
        if (CompareTag("Player"))
            soundManager.Play(damageSound, transform.position);
        if (healthPoints <= 0)
        {
            if (instigator.CompareTag("Player"))
            {
                if (CompareTag("NPC")) GetComponent<TargetFinder>().UpdateState(4);
                if (CompareTag("Enemy")) GetComponent<TargetFinder>().UpdateState(2);
            }
            isDead = true;
            if (CompareTag("Player"))
                soundManager.Play(deathSound, transform.position);
            if (GetComponent<EnemyController>() && GetComponent<EnemyController>().IsBoss())
            {
                instigator.GetComponent<CharacterStats>().SetDoorsUnloacked(); door.gameObject.SetActive(true);
                soundManager.Play(voice, transform.position);
                soundManager.Play(musicbit, transform.position);
            }
            if(!GetComponent<AnimalMover>())
                animator.SetTrigger("Death");
            else
                StartCoroutine(Relocate());
            if(instigator!=gameObject)
                AwardExperience(instigator);
        }
        else
        {
            takeDamage.Invoke(damage);
        }
    }

    private void AwardExperience(GameObject instigator)
    {
       Experience experience = instigator.GetComponent<Experience>();
       if (experience!=null)
       {
            experience.GainExperience(GetComponent<CharacterStats>().GetStat(Stats.Experience));
       }
    }

    public float GetHealthFraction()
    {
        return healthPoints / maxHealth;
    }

    float GetDamage(float damage)
    {
        if (isPercent)
        {
            damage -= armor * damage / 100;
        }
        else
        {
            damage -= armor;
        }
        return damage;
    }
    public float ArmorModifier(float mod, bool isPercent, float lastingTime)
    {
        if (lastingTime > 0)
        {
            armor += mod;
            StartCoroutine(Protection(lastingTime, mod));
        }
        else { armor = mod; }
        this.isPercent = isPercent;
        return armor;
    }

    IEnumerator Protection(float lastingTime, float mod)
    {
        yield return new WaitForSeconds(lastingTime);
        armor -= mod;
        GetComponent<StatsModifier>().UpdateArmor(armor);
    }

    public float HealthModifier(float oldHealthMod, float newHealthMod, bool isPercent, bool hasCap)
    {
        if (this.isPercent)
        { healthPoints -= startHealth * oldHealthMod / 100; maxHealth -= startHealth * oldHealthMod / 100; }
        else { healthPoints -= oldHealthMod; maxHealth -= oldHealthMod; }
        this.isPercent = isPercent;
        if (isPercent)
        {
            if (hasCap)
            {
                healthPoints = Mathf.Min(maxHealth, healthPoints += startHealth * newHealthMod / 100);
            }
            else
            {
                healthPoints += startHealth * newHealthMod / 100; maxHealth += startHealth * newHealthMod / 100;
            }
        }
        else
        {
            if (hasCap)
            {
                healthPoints = Mathf.Min(maxHealth, healthPoints += newHealthMod);
            }
            else
            { healthPoints += newHealthMod; maxHealth += newHealthMod; }
        }
        if(newHealthMod>oldHealthMod)
        {
            if (healthUpEffect != "")
                HealthUpEffect();
        }
        return healthPoints;
    }

    private void HealthUpEffect()
    {
        poolManager.Spawn(healthUpEffect, transform.position, transform);
    }
    public float GetBaseArmor()
    {
        return armor;
    }

    public int GetHealth()
    {
        return Mathf.RoundToInt(healthPoints);
    }

    public bool IsDead()
    {
        return isDead;
    }
    public void Live()
    {
        GetComponent<StatsModifier>().UpdateHealth(0, 100f, true, true);
        isDead = false;
    }
}
