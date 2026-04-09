using UnityEngine;
using System.Collections;

public class Drill : ToolBase
{
    [Header("Tool Settings")]
    public string toolName = "Drill";
    public int toolBaseDamage = 1;
    public int toolBaseDurability = 150;
    public int toolBaseActionSpeed = 10;
    public List<int> toolRarityRange = new List<int> { 1, 2, 3 };
    void Start()
    {
        toolDamage = toolBaseDamage;
        toolDurability = toolBaseDurability;
        toolActionSpeed = toolBaseActionSpeed;
        toolRarityLevel = toolRarityRange[Random.Range(0, toolRarityRange.Count)];
        Debug.Log("Created " + toolName + " with damage: " + toolDamage + ", durability: " + toolDurability + ", action speed: " + toolActionSpeed + ", rarity level: " + toolRarityLevel);
    }

    protected override List<Vector3Int> GetAffectedTiles(Vector3Int targetCoordinate)
    {
        List<Vector3Int> affectedTiles = new List<Vector3Int>();

        // Drill affects a 1x1 area (just the targeted block)
        affectedTiles.Add(targetCoordinate);

        return affectedTiles;
    }

}
