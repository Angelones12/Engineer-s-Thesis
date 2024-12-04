using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HealthSystem
{
    public event Action OnHealthChanged;

    public event Action OnDeath;

    public float health;
    private float maxhealth;

    public float Health
    {
        get => health;
        set
        {
            health = value;
            if(health > maxhealth)
            {
                health = maxhealth;
            }
            if(health <= 0)
            {
                OnDeath?.Invoke();
            }

            OnHealthChanged?.Invoke();
        }
    }

    public float MaxHealth
    {
        get => maxhealth;
        set
        {
            maxhealth = value;
        }
    }

    public HealthSystem(float defaultHealth)
    {
        health = defaultHealth;
        maxhealth = defaultHealth;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void PrintHealth()
    {
        UnityEngine.Debug.Log($"Health:{health}/{maxhealth}");
    }

}
