using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hands : ToolBase
{
    [Header("Hands Settings")]
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

        // Hands affect a 1x1 area (just the targeted block)
        affectedTiles.Add(targetCoordinate);

        return affectedTiles;
    }

    /*
    public override void ToolAnimate(Vector2 position)
    {
        // Placeholder for hands-specific animation logic
        Debug.Log("Animating " + toolName + " with a punching motion at position " + position);
    }
    */

}
