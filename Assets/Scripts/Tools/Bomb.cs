using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : ToolBase
{
    [Header("Bomb Settings")]
    // public List<int> toolRarityRange = new List<int> { 1, 2, 3 };
    public int explosionRadius = 10; // Radius of the explosion effect
    void Start()
    {
        // toolRarityLevel = toolRarityRange[Random.Range(0, toolRarityRange.Count)];
        Debug.Log("Created " + toolName + " with damage: " + toolDamage + ", durability: " + toolDurability + ", action speed: " + toolActionSpeed);
    }

    public override void UseTool(Vector3Int targetCoordinate) // override to implement bomb-specific behavior, such as affecting a larger area and applying damage in an explosion radius
    {
        if (toolDurability > 0)
        {
            List<Vector3Int> affectedTiles = GetAffectedTiles(targetCoordinate);

            foreach (Vector3Int tileCoordinate in affectedTiles)
            {
                MineBlock(tileCoordinate, toolDamage);
            }

            Debug.Log("Using " + toolName + " for " + toolDamage + " damage in an explosion radius of " + explosionRadius);
            toolDurability--;
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
            Debug.Log(toolName + " is out of durability and cannot be used.");
        }
    }

}
