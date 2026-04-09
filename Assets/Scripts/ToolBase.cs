using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolBase : MonoBehaviour
{
    [Header("Tool Settings")]
    public string toolName = "No-Name";
    public int toolDamage = 1;
    public int toolDurability = 10;
    public int toolActionSpeed = 1;
    public int toolRarityLevel = 1;
    public int fortuneLevel = 1;
    private float nextUseTime = 0f;

    public void OnEquip()
    {
        Debug.Log("Equipped " + toolName);
        // Add tool to player here
    }

    public void OnUnequip()
    {
        Debug.Log("Unequipped " + toolName);
        // Remove tool from player here
    }

    public void tryUseTool(Vector3Int targetCoordinate) // checks if tool can be used based on its cooldown
    {
        if (Time.time >= nextUseTime)
        {
            UseTool(targetCoordinate);
        }
        else
        {
            Debug.Log(toolName + " is on cooldown. Please wait.");
        }
    }
    public void UseTool(Vector3Int targetCoordinate) // main logic for using the tool, checks durability and applies damage to blocks
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
        OnUnequip();
    }

    private void MineBlock(Vector3Int tileCoordinate, int damage)
    {
        // Placeholder for block mining logic
        Debug.Log(toolName + " is mining block at " + tileCoordinate + " for " + damage + " damage.");
        // if block.hp > damagem, block.hp -= damage; else block is destroyed
    }

}