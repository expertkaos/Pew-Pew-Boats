using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{

    [Header("Base Stats (Unchanging)")]
    public float baseAcceleration = 8f;
    public float baseMaxSpeed = 4f;
    public float baseRotationSpeed = 180f;
    public float baseFriction = 2f;
    public float baseFireRate = 2f;
    public float baseProjectileForce = 5f;
    public int baseMaxHealth = 30;
    public int baseDamage = 10;

    [Header("Runtime Stats (Modifiable)")]
    public float acceleration;
    public float maxSpeed;
    public float rotationSpeed;
    public float friction;
    public float fireRate;
    public float projectileForce;
    public int maxHealth;
    public int currentHealth;
    public int damage;

    public void ResetStats()
    {
        acceleration = baseAcceleration;
        maxSpeed = baseMaxSpeed;
        rotationSpeed = baseRotationSpeed;
        friction = baseFriction;
        maxHealth = baseMaxHealth;
        currentHealth = maxHealth;
        damage = baseDamage;
        fireRate = baseFireRate;
        projectileForce = baseProjectileForce;
    }

    // Upgrade methods
    public void IncreaseProjectileForce(float amount)
    {
        projectileForce += amount;
    }
    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;
        fireRate = Mathf.Clamp(fireRate, 0.1f, 10f);
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

    public void IncreaseSpeed(float amount)
    {
        maxSpeed += amount;
    }

    public void IncreaseAcceleration(float amount)
    {
        acceleration += amount;
    }

    public void ReduceFriction(float amount)
    {
        friction = Mathf.Max(0, friction - amount);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
