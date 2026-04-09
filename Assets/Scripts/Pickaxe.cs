using UnityEngine;
using System.Collections;

public class Pickaxe : ToolBase
{
    [Header("Tool Settings")]
    public string toolName = "Pickaxe";
    public int toolBaseDamage = 2;
    public int toolBaseDurability = 15;
    public int toolBaseActionSpeed = 1;
    public List<int> toolRarityRange = new List<int> { 1, 2, 3 };
    public int miningAreaSize = 1; // Size of the area affected by the pickaxe (e.g., 1 for single block, 3 for 3x3 area)

    void Start()
    {
        toolDamage = toolBaseDamage;
        toolDurability = toolBaseDurability;
        toolActionSpeed = toolBaseActionSpeed;
        toolRarityLevel = toolRarityRange[Random.Range(0, toolRarityRange.Count)];
        Debug.Log("Created " + toolName + " with damage: " + toolDamage + ", durability: " + toolDurability + ", action speed: " + toolActionSpeed + ", rarity level: " + toolRarityLevel);
    }

}
