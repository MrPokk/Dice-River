using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct HealthComponent
{
    [Tooltip("Maximum number of dash charges the entity can have."), Range(1, MaxHealthLimit)]
    public int maxHealth;

    [Tooltip("Maximum number of dash charges the entity can have.")]
    public int currentHealth;

    private const int MaxHealthLimit = 100;

    public readonly int GetCurrentHealth() => currentHealth > 0 ? currentHealth : maxHealth;

    public readonly int GetMaxHealth() => maxHealth;

    public void SetHealth(int health) => currentHealth = Mathf.Clamp(health, 0, maxHealth);

    public void SetMaxHealth(int health) => maxHealth = Mathf.Clamp(health, 1, MaxHealthLimit);

    public void ResetHealth() => currentHealth = maxHealth;

    [ReadOnly] public float timeImmunity;
    [ReadOnly] public int lastDamage;
}

public class HealthComponentProvider : ProviderEcs<HealthComponent>
{ }
