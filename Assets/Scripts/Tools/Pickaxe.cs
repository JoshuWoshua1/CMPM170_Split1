using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickaxe : ToolBase
{
    [Header("Pickaxe Settings")]
    // public List<int> toolRarityRange = new List<int> { 1, 2, 3 };
    public int miningAreaSize = 1; // Size of the area affected by the pickaxe (e.g., 1 for single block, 3 for 3x3 area)

    void Start()
    {
        // toolRarityLevel = toolRarityRange[Random.Range(0, toolRarityRange.Count)];
        Debug.Log("Created " + toolName + " with damage: " + toolDamage + ", durability: " + toolDurability + ", action speed: " + toolActionSpeed);
    }

    protected override List<Vector3Int> GetAffectedTiles(Vector3Int targetCoordinate)
    {
        List<Vector3Int> affectedTiles = new List<Vector3Int>();

        int halfSize = miningAreaSize / 2;
        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                for (int z = -halfSize; z <= halfSize; z++)
                {
                    affectedTiles.Add(new Vector3Int(targetCoordinate.x + x, targetCoordinate.y + y, targetCoordinate.z + z));
                }
            }
        }

        return affectedTiles;
    }
}
