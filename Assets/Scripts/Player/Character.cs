using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Scripts.Data;
using System;

public class Character : MonoBehaviour, IDataPersistence
{
    private HealthSystem healthSystem;
    public float maxHealth = 6;
    private Animator animator;
    private bool isImmune = false;
    private float immuneDuration = 0.8f;
    private float immuneTimer = 0f;
    private DataPersistenceManager dataPersistenceManager;
    private bool isDead = false;
    public bool canMove = true;
    private static readonly int DeathTrigger = Animator.StringToHash("DeathTrigger");

    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerRespawned;

    public HealthSystem HealthSystem
    {
        get { return healthSystem; }
    }

    private void Awake()
    {
        StartCoroutine(FindDataPersistenceManager());

        healthSystem = new HealthSystem(maxHealth);
        healthSystem.PrintHealth();

        healthSystem.OnHealthChanged += OnHealthChanged;
        healthSystem.OnDeath += HandleDeath;

        animator = GetComponent<Animator>();
    }

    private IEnumerator FindDataPersistenceManager()
    {
        yield return new WaitForSeconds(0.1f);  
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
    }

    private void HandleDeath()
    {
        if (!isDead)
        {
            canMove = false;
            animator.SetTrigger(DeathTrigger);
            isDead = true;
            
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(4);

        canMove = true;

        if (dataPersistenceManager != null)
        {
            dataPersistenceManager.LoadGame();

            dataPersistenceManager.GameData.deathCounter++;

            dataPersistenceManager.SaveGame();

            OnPlayerRespawned?.Invoke();
        }
        else
        {
            UnityEngine.Debug.LogError("DataPersistenceManager is null");
        }
    }

    private void OnHealthChanged()
    {
        healthSystem.PrintHealth();
        OnPlayerDamaged?.Invoke();
        if (!isImmune)
        {
            isImmune = true;
            immuneTimer = immuneDuration;
            StartCoroutine(ImmuneCooldown());
        }
    }

    public void LoadData(GameData data)
    {
        isDead = false;
        healthSystem.Health = data.health;
        healthSystem.MaxHealth = maxHealth;
        healthSystem.PrintHealth(); 
    }

    public void SaveData(ref GameData data)
    {
        data.health = healthSystem.Health;
    }

    private IEnumerator ImmuneCooldown()
    {
        while (immuneTimer > 0f)
        {
            immuneTimer -= Time.deltaTime;
            yield return null;
        }

        isImmune = false;
    }

    public void HealToMaxHealth()
    {
        healthSystem.Health = healthSystem.MaxHealth;
        healthSystem.PrintHealth();
    }

    public void TakeDamage()
    {
        if (!isImmune)
        {
            healthSystem.Health -= 1;
            OnPlayerDamaged?.Invoke();
            animator.SetTrigger("DamageTrigger");
        }
    }

    public float GetHealth()
    {
        return healthSystem.Health;
    }

}
