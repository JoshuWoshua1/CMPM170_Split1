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

    [Header("Item Drop Settings")]
    public int droppedItemID = 0; // ID of the item to drop when destroyed (0 = no drop)
    public int droppedItemQuantity = 1; // How many items to drop

    void Start()
    {
        ConfigureFromBlockName();
        currentHealth = maxHealth;

        // If there's a healthbar attached, initialize it
        if (healthBar != null)
        {
            healthBar.maxHealth = maxHealth;
            healthBar.health = currentHealth;
        }
    }

    void ConfigureFromBlockName()
    {
        if(name.Contains("Dirt"))
        {
            maxHealth = 100f;
        }
        else if(name.Contains("Stone"))
        {
            maxHealth = 150f;
        }
        else if(name.Contains("Gold"))
        {
            maxHealth = 300f;
        }
        else if(name.Contains("Diamond"))
        {
            maxHealth = 500f;
        }
        else if(name.Contains("Ruby"))
        {
            maxHealth = 700f;
        }
        else if(name.Contains("Emerald"))
        {
            maxHealth = 1000f;
        }
        else if(name.Contains("Lava"))
        {
            maxHealth = 1500f;
        }
        else if(name.Contains("Greystone"))
        {
            maxHealth = 350f;
        }
        else
        {
            maxHealth = 100f; // Default health for unknown blocks
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

        // Drop item into player inventory
        if (droppedItemID > 0)
        {
            DropItemToInventory();
        }

        // Spawn destruction effect
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Destroy the game object
        Destroy(gameObject);
    }

    void DropItemToInventory()
    {
        // Find the ItemDictionary in the scene
        ItemDictionary itemDict = FindFirstObjectByType<ItemDictionary>();
        if (itemDict == null)
        {
            Debug.LogWarning("ItemDictionary not found in scene! Cannot drop item.");
            return;
        }

        // Get the item prefab from the dictionary
        GameObject itemPrefab = itemDict.GetItemPrefab(droppedItemID);
        if (itemPrefab == null)
        {
            Debug.LogWarning("Item with ID " + droppedItemID + " not found in ItemDictionary!");
            return;
        }

        // Find the ShopManager (which manages player inventory)
        ShopManager shopManager = ShopManager.Instance;
        if (shopManager == null)
        {
            Debug.LogWarning("ShopManager not found! Cannot add item to inventory.");
            return;
        }

        // Find an empty inventory slot
        Transform emptySlot = FindEmptyInventorySlot(shopManager.playerInventoryPanel);
        if (emptySlot == null)
        {
            Debug.Log("No empty inventory slots! Item dropped on ground.");
            // TODO: Could spawn item in world here as a pickup
            return;
        }

        // Instantiate the item in the inventory slot
        GameObject droppedItem = Instantiate(itemPrefab, emptySlot);
        Item itemComponent = droppedItem.GetComponent<Item>();

        if (itemComponent != null)
        {
            // Set the quantity
            itemComponent.quantity = droppedItemQuantity;
        }

        // Position the item in the slot
        RectTransform rectTransform = droppedItem.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }

        // Update the slot's current item reference
        Slot slotComponent = emptySlot.GetComponent<Slot>();
        if (slotComponent != null)
        {
            slotComponent.currentItem = droppedItem;
        }
        else
        {
            // Try ShopSlot if regular Slot not found
            ShopSlot shopSlotComponent = emptySlot.GetComponent<ShopSlot>();
            if (shopSlotComponent != null)
            {
                shopSlotComponent.currentItem = droppedItem;
            }
        }

        Debug.Log("Added " + droppedItemQuantity + "x " + itemComponent.Name + " to player inventory!");
    }

    Transform FindEmptyInventorySlot(Transform inventoryPanel)
    {
        if (inventoryPanel == null) return null;

        foreach (Transform child in inventoryPanel)
        {
            // Check for regular Slot component
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                return child;
            }

            // Check for ShopSlot component (used in shop inventory UI)
            ShopSlot shopSlot = child.GetComponent<ShopSlot>();
            if (shopSlot != null && shopSlot.currentItem == null)
            {
                return child;
            }
        }
        return null;
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
