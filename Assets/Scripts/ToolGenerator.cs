
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ToolGenerator : MonoBehaviour
{
    
    [SerializeField] private string LootTableFilePath;
    [SerializeField] private TextAsset lootTableTextAsset;
    [SerializeField] private string lootTableResourcePath;
    [SerializeField] private string[] Rarities = new string[] { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
    [SerializeField] private int[] Rarity_Weights = new int[] { 55, 30, 15, 6, 1 };

    private void Awake()
    {
        LoadLoot();
    }

    // Loot table: rarity -> list of consumables. Rarities and weights for roll.
    
    private Dictionary<string, int> lootRarities = new Dictionary<string, int>();
    private Dictionary<string, List<Tool>> lootTable = new Dictionary<string, List<Tool>>();
    
   

    // at start of game read from Tool csv file and creates a list of Tool Objects to use later for loot generation.   
    private void LoadLoot()
    {
        Debug.Log("File Path: " + LootTableFilePath + " loaded with " + lootTable.Count + " rarities.");
        Debug.Log("Rarities Loaded:" + Rarities.Length + " with corresponding weights: " + Rarity_Weights.Length);
        lootTable = LoadLootTable(LootTableFilePath);
        

        lootRarities = LoadRarities(Rarities, Rarity_Weights);
        
    }
    
    //Reads the Loot Rarities and their corresponding weights from unity inspector and creates a dictionary for use in loot generation.
    private Dictionary<string, int> LoadRarities(string[] rarities, int[] weights)
    {
        Dictionary<string, int> customRarities = new Dictionary<string, int>();
        if(rarities.Length != weights.Length)
        {
            Debug.LogError("ERROR Rarity Categories and Rarity Weights of unequal size"); 
            return customRarities;
        }
        for(int i = 0; i < rarities.Length; i++)
        {
            customRarities.Add(rarities[i],weights[i]);
        }
        return customRarities;
    }

    // Reads the Tool Loot Table from a CSV file and creates a dictionary of rarity categories and their corresponding list of Tool objects for use in loot generation. 
    private Dictionary<string, List<Tool>> LoadLootTable(string filePath)
    {
        Dictionary<string, List<Tool>> customLootTable = new Dictionary<string, List<Tool>>();

        if (!TryReadLootLines(filePath, out List<string> lines))
        {
            Debug.LogError("Failed to load loot table. No valid lines found.", this);
            return customLootTable;
        }

        int imageIndex = 0;
        for (int lineIndex = 1; lineIndex < lines.Count; lineIndex++)
        {
            string line = lines[lineIndex];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] values = line.Split(',');
            if (values.Length < 6)
            {
                Debug.LogWarning($"ToolGenerator: Skipping malformed loot line {lineIndex + 1}.", this);
                continue;
            }

            string ToolRarity = values[0].Trim();
            string ToolName = values[1].Trim();
            string ToolDescription = values[2].Trim();
            int ToolDurability = Int32.Parse(values[3].Trim());
            int ToolMiningSpeed = Int32.Parse(values[4].Trim());
            int ToolMiningDamage = Int32.Parse(values[5].Trim());
            Debug.Log($"Creating Tool of:  - Rarity: {ToolRarity}, Name: {ToolName}, Durability: {ToolDurability}, Mining Speed: {ToolMiningSpeed}, Mining Damage: {ToolMiningDamage}");
            
            Tool newTool = new Tool(ToolRarity, ToolName, ToolDescription, ToolDurability, ToolMiningSpeed, ToolMiningDamage, imageIndex);

            // if item rarity key already exists, add newItem to the list, else create new rarity key with a new list of items.
            if (customLootTable.ContainsKey(ToolRarity))
            {
                customLootTable[ToolRarity].Add(newTool);
            }
            else
            {
                customLootTable.Add(ToolRarity, new List<Tool> {newTool});
            }
            imageIndex++;
        }

        return customLootTable;
    }
  public Dictionary<string, List<Tool>> GetLootTable()
    {
        return lootTable;
    }
    public Dictionary<string,int> GetLootRarities()
    {
        
        return lootRarities;
    }

 // Not Sure what these are used for
    private bool TryReadLootLines(string configuredPath, out List<string> lines)
    {
        if (lootTableTextAsset != null)
        {
            lines = SplitLines(lootTableTextAsset.text);
            return lines.Count > 0;
        }

        List<string> attemptedPaths = BuildCandidatePaths(configuredPath);
        for (int i = 0; i < attemptedPaths.Count; i++)
        {
            string candidatePath = attemptedPaths[i];
            if (File.Exists(candidatePath))
            {
                lines = new List<string>(File.ReadAllLines(candidatePath));
                return lines.Count > 0;
            }
        }

        string resourcePath = string.IsNullOrWhiteSpace(lootTableResourcePath)
            ? Path.GetFileNameWithoutExtension(configuredPath)
            : lootTableResourcePath;
        TextAsset resourceCsv = Resources.Load<TextAsset>(resourcePath);
        if (resourceCsv != null)
        {
            lines = SplitLines(resourceCsv.text);
            return lines.Count > 0;
        }

        Debug.LogError($"ToolGenerator: Could not load loot table CSV. Checked file paths: {string.Join(" | ", attemptedPaths)} and Resources path '{resourcePath}'.", this);
        lines = new List<string>();
        return false;
    }

    private static List<string> SplitLines(string text)
    {
        List<string> lines = new List<string>();
        using (StringReader reader = new StringReader(text ?? string.Empty))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    private static List<string> BuildCandidatePaths(string configuredPath)
    {
        List<string> paths = new List<string>();
        AddPath(paths, configuredPath);

        string normalized = string.IsNullOrWhiteSpace(configuredPath)
            ? string.Empty
            : configuredPath.Replace('\\', '/');

        if (normalized.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
        {
            string relativeToAssets = normalized.Substring("Assets/".Length);
            AddPath(paths, Path.Combine(Application.dataPath, relativeToAssets));
            AddPath(paths, Path.Combine(Application.streamingAssetsPath, relativeToAssets));
        }

        string fileName = Path.GetFileName(normalized);
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            AddPath(paths, Path.Combine(Application.streamingAssetsPath, fileName));
            AddPath(paths, Path.Combine(Application.streamingAssetsPath, "CSV Files", fileName));
        }

        return paths;
    }

    private static void AddPath(List<string> paths, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        if (!paths.Contains(path))
        {
            paths.Add(path);
        }
    }

  
}