using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drill : ToolBase
{
    [Header("Drill Settings")]
    public string temp = "na";
    // public List<int> toolRarityRange = new List<int> { 1, 2, 3 };
    void Start()
    {
        // toolRarityLevel = toolRarityRange[Random.Range(0, toolRarityRange.Count)];
        Debug.Log("Created " + toolName + " with damage: " + toolDamage + ", durability: " + toolDurability + ", action speed: " + toolActionSpeed);
    }

    protected override List<Vector3Int> GetAffectedTiles(Vector3Int targetCoordinate)
    {
        List<Vector3Int> affectedTiles = new List<Vector3Int>();

        // Drill affects a 1x1 area (just the targeted block)
        affectedTiles.Add(targetCoordinate);

        return affectedTiles;
    }

}
