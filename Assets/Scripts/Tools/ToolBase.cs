using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tool
{
    public string Rarity;
    public string Name;
    public string Description;
    public int Durability;
    public int MiningSpeed;
    public int MiningDamage;
    public int ImageIndex;
    public string ToolType;

    public Tool(string rarity, string name, string description, int durability, int miningSpeed, int miningDamage, int imageIndex = 0, string toolType = "Pickaxe")
    {
        Rarity = rarity;
        Name = name;
        Description = description;
        Durability = durability;
        MiningSpeed = miningSpeed;
        MiningDamage = miningDamage;
        ImageIndex = imageIndex;
        ToolType = toolType;
    }
}

public class ToolBase : MonoBehaviour
{
    [Header("Tool Settings")]
    public string toolName = "No-Name";
    public string toolDescription = "No description";
    public int toolDamage = 1;
    public int toolDurability = 10;
    public int maxToolDurability = 10;
    public int toolActionSpeed = 1;
    public int toolRarityLevel = 1;
    public int fortuneLevel = 1;
    public SpriteRenderer toolSprite;
    public float nextUseTime = 0f;

    [Header("Default Equipped Transform")]
    public Vector3 equippedLocalPosition = new Vector3(0f, -4f, 0f);
    public Vector3 equippedLocalRotation = new Vector3(0f, 0f, 0f);
    public Vector3 equippedLocalScale = new Vector3(7f, 7f, 7f);

    protected virtual void Awake()
    {
        maxToolDurability = Mathf.Max(1, toolDurability);
    }

    private void EnsureToolSpriteReference()
    {
        if (toolSprite != null)
        {
            return;
        }

        toolSprite = GetComponent<SpriteRenderer>();
    }

    /* ========================= TEMP LOOTBOX ADAPTER START =========================
        * this is a temporary method to initialize tools from the lootbox system, it will be replaced with a more robust system later on.
        * this was made using github copilot 
        */
    public virtual void InitializeFromData(Tool data, Sprite icon = null)
    {
        if (data == null)
        {
            Debug.LogWarning("InitializeFromData called with null Tool data.");
            return;
        }

        EnsureToolSpriteReference();

        toolName = data.Name;
        toolDescription = data.Description;
        toolDurability = Mathf.Max(0, data.Durability);
        maxToolDurability = Mathf.Max(1, data.Durability);
        toolActionSpeed = Mathf.Max(1, data.MiningSpeed);
        toolDamage = Mathf.Max(0, data.MiningDamage);
        toolRarityLevel = ParseRarityLevel(data.Rarity);
        nextUseTime = 0f;

        if (toolSprite != null && icon != null)
        {
            toolSprite.sprite = icon;
        }
    }

    protected virtual int ParseRarityLevel(string rarity)
    {
        if (string.IsNullOrWhiteSpace(rarity))
        {
            return 1;
        }

        if (int.TryParse(rarity, out int rarityNumber))
        {
            return Mathf.Max(1, rarityNumber);
        }

        switch (rarity.Trim().ToLower())
        {
            case "common":
                return 1;
            case "uncommon":
                return 2;
            case "rare":
                return 3;
            case "epic":
                return 4;
            case "legendary":
                return 5;
            default:
                return 1;
        }
    }
    /* end of copilot code
     *========================== TEMP LOOTBOX ADAPTER END ========================== */

    public void OnEquip()
    {
        transform.localPosition = equippedLocalPosition;
        transform.localRotation = Quaternion.Euler(equippedLocalRotation);
        transform.localScale = equippedLocalScale;
        Debug.Log("Equipped " + toolName);
        // Add tool to player here
    }

    public void OnUnequip()
    {
        Debug.Log("Unequipped " + toolName);
        Destroy(gameObject);
        // Remove tool from player here
    }

    public void tryUseTool(Vector3Int targetCoordinate) // checks if tool can be used based on its cooldown
    {
        if (Time.time >= nextUseTime)
        {
            UseTool(targetCoordinate);
            StartCoroutine(ToolAnimate());
        }
        else
        {
            Debug.Log(toolName + " is on cooldown. Please wait.");
        }
    }
    public virtual void UseTool(Vector3Int targetCoordinate) // main logic for using the tool, checks durability and applies damage to blocks
    {
        if (toolDurability > 0)
        {
            List<Vector3Int> affectedTiles = GetAffectedTiles(targetCoordinate);

            foreach (Vector3Int tileCoordinate in affectedTiles)
            {
                MineBlock(tileCoordinate, toolDamage);
            }

            Debug.Log("Using " + toolName + " for " + toolDamage + " damage.");
            toolDurability--;
            toolDurability = Mathf.Max(0, toolDurability);
            if (toolActionSpeed > 0)
            {
                nextUseTime = Time.time + 1f / toolActionSpeed;
            }
            else
            {
                Debug.LogWarning(toolName + " has an invalid action speed. Please check the tool settings.");
            }
        }
        else
        {
            Debug.Log(toolName + " is broken and cannot be used.");
            ToolBroken();
        }
    }

    protected virtual List<Vector3Int> GetAffectedTiles(Vector3Int targetCoordinate)
    {
        return new List<Vector3Int> { targetCoordinate };
    }

    private void ToolBroken()
    {
        Debug.Log(toolName + " has broken!");
        // placeholder for vfx or other things attatched to tools breaking
        // maybe make particles that use the tools colors like in minecraft?
        OnUnequip();
    }

    public void MineBlock(Vector3Int tileCoordinate, int damage)
    {
        Vector3 worldPosition = new Vector3(tileCoordinate.x, tileCoordinate.y, tileCoordinate.z);

        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPosition, 0.5f);

        foreach (Collider2D hit in hits)
        {
            Destroyable destroyable = hit.GetComponent<Destroyable>();

            if (destroyable != null)
            {
                destroyable.TakeDamage(damage);
                Debug.Log(toolName + " damaged " + hit.gameObject.name + " for " + damage + " damage.");
                return;
            }
        }

        Debug.Log(toolName + " tried to mine at " + tileCoordinate + " but found no destroyable object.");
    }

    public virtual IEnumerator ToolAnimate()
    {
        // Placeholder for tool animation logic
        Debug.Log(toolName + "has no animation.");

        EnsureToolSpriteReference();
        if (toolSprite == null)
        {
            yield break;
        }

        toolSprite.transform.localScale = new Vector2(8f, 8f); // example animation effect, scales the sprite up slightly when used
        
        yield return new WaitForSeconds(0.5f); // wait for a short duration to simulate animation timing

        transform.localPosition = equippedLocalPosition;
        transform.localRotation = Quaternion.Euler(equippedLocalRotation);
        transform.localScale = equippedLocalScale;
    }

}