using UnityEngine;
using UnityEngine.Events;

public class Destroyable : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar Reference (Optional)")]
    public healthbar healthBar; // Reference to the UI healthbar - optional, only needed if you want to show health on screen

    [Header("Events")]
    public UnityEvent OnDestroyed; // Called when object is destroyed - use this to trigger effects, sounds, etc.

    [Header("Destruction Settings")]
    public GameObject destroyEffect; // Optional particle effect when destroyed

    void Start()
    {
        currentHealth = maxHealth;

        // If there's a healthbar attached, initialize it
        if (healthBar != null)
        {
            healthBar.maxHealth = maxHealth;
            healthBar.health = currentHealth;
        }
    }

    // Main method that tools and other classes will call to damage this object
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log(gameObject.name + " took " + damage + " damage. Health: " + currentHealth + "/" + maxHealth);

        // Update healthbar if it exists
        if (healthBar != null)
        {
            healthBar.health = currentHealth;
        }

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    // Method to heal/repair the object
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log(gameObject.name + " healed " + amount + ". Health: " + currentHealth + "/" + maxHealth);

        // Update healthbar if it exists
        if (healthBar != null)
        {
            healthBar.health = currentHealth;
        }
    }

    // Set health directly to a specific value
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);

        // Update healthbar if it exists
        if (healthBar != null)
        {
            healthBar.health = currentHealth;
        }

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Debug.Log(gameObject.name + " destroyed!");

        OnDestroyed?.Invoke();

        // Spawn destruction effect
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Destroy the game object
        Destroy(gameObject);
    }

    // Public getter for current health (so other classes can check it)
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Public getter for health percentage (0 to 1)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    // Public getter for max health
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
