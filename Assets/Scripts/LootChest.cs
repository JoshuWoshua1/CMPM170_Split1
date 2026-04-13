// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;

using System;
public class LootChest : MonoBehaviour
{
   
    public Button GenerateBTN;
    public Button EquipBTN;
    [SerializeField]
    public TextMeshProUGUI Chest_Output_TMP;
     [SerializeField]
    public TextMeshProUGUI Chest_Item_Description_TMP;
    [SerializeField]
    public RawImage Chest_Item_PFP;
    
    [SerializeField]
    public GameObject Chest_OutputUI;
    public  ToolGenerator toolGenerator;
    public SpriteManager spriteManager;
    public TempPlayer player;

    private bool alreadyGenerated;
    private int lootCount;
    private LootChest myChest;
    private Tool currentGeneratedTool; // current tool for equiping
    

    void Start()
    {
        Button btn = GenerateBTN.GetComponent<Button>();
		myChest = new LootChest();
		btn.onClick.AddListener(Open);

        if (EquipBTN != null)
        {
            EquipBTN.onClick.AddListener(OnEquipButtonPressed);
        }

        Chest_OutputUI.SetActive(false);
    }
 
    // call this function to open chest 
    public void Open(){

        // generates a new chest with loot if 1: chest has not been opened 2: chest allows for regeneration of loot upon open
        // else display existing chest's already generated loot 
        if(!alreadyGenerated)
        {
            Debug.Log("New chest created");
            myChest = LootChestGeneration(myChest);
            drops = new List<Tool>(myChest.drops);
           
            alreadyGenerated = true;
           
            displayOutput(myChest);
            CacheGeneratedTool(myChest);
            //SpawnLootDrops(myChest.drops); // places the generated loot into the world as game objects for testing
        }
        else
        {
            Debug.Log("Chest already Exists generating new one");
            myChest = LootChestGeneration(myChest);
            drops = new List<Tool>(myChest.drops);
            displayOutput(myChest);
            CacheGeneratedTool(myChest);
            //SpawnLootDrops(myChest.drops); // places the generated loot into the world as game objects for testing
        }
        
	}
    // c# object that stores data about object created in csv file
    
    public List<Tool> drops = new List<Tool>();
    Dictionary <string, int> lootRarities = new Dictionary<string, int>();
    Dictionary <string, List<Tool>> lootTable = new Dictionary<string, List<Tool>>();
    
   

    /* Returns a new chest instance with attached loot table paramaters. 
        If loot chest has not yet been generated, read from loot table csv and attach corresponding rarity weights.
       

        
    */
    public LootChest LootChestGeneration(LootChest chest)
    {
        if(!alreadyGenerated) //  This prevents a lootchest from having to reread the csv file every time it generates new loot
        {
            chest.lootCount = 1;
            chest.lootTable = toolGenerator.GetLootTable();
            chest.lootRarities = toolGenerator.GetLootRarities();
            Debug.Log("Chest Generated with " + chest.lootRarities.Count + " Rarities and " + chest.lootTable.Count + " total items.");
        }

        chest.drops = chest.GenerateLoot(chest.lootRarities, chest.lootCount);
        return chest;
    }
   
        // Takes in a LootChest and displays it's generated loot to the Unity UI 
    public void displayOutput(LootChest currentChest)
    {
        string output = "";
        Chest_OutputUI.SetActive(true);
        foreach(var drop in currentChest.drops)
        {
            output = drop.Name + "\n" ;
        }

        Chest_Output_TMP.text = output;
        Chest_Item_PFP.texture = spriteManager.ToolRawImages[currentChest.drops[0].ImageIndex];
        Chest_Item_Description_TMP.text = "Description: " + currentChest.drops[0].Description; 
    }
    // This function allows for custom rarities that correspond to the loot table .csv file's rarities, as well as the corresponding weights of each rarity. 
   
    
    /* This is the main function that generates the loot from the loot table. 
      Takes in a Dictionary of custom rarities and their corresponding weights, and an integer for how many items to generate from the loot table, 
      outputs a list of gameItems corresponding to items in loot table
     
        Steps: 
            1: Initialize selectedRarity dictionary and calculate weighted sum of rarities
            2: Using weighted sum, generate a random number beftween 0 and sum.  
            3: take previous generated number and subtract it from the next higest rarity. If the remaining number is >= next rarity repeat with next lowest rarity. 
            4: If the number < next lowest rarity, then that is the selected rarity. 
            5: When a rarity is selected add it to the output dictionary. If already existing increment it's counter. 
            6: With the selected rarities, randomly select items from the loot table based on the number of items per rarity. 
    */
    public List<Tool> GenerateLoot(Dictionary<string, int> myRarity, int lootCount = 1 ) //
    {
        Dictionary <string, int> selectedRarity = new Dictionary<string, int>();
        List<Tool> selectedItems = new List<Tool>();
        System.Random rand = new System.Random();
        int weightedSum = 0;

        //Step 1
        foreach(var rarity in myRarity)
        {
            selectedRarity.Add(rarity.Key, 0);
            weightedSum += rarity.Value;
        }
        if(weightedSum <= 0) 
        {
            Debug.Log("Error: No loot available.");
            return selectedItems;
        }

        for(int i = 0; i < lootCount; i++) // number of items to gererate
        {
            //Step 2: 
            int roll = rand.Next(0,weightedSum); 

            //Step 3: 
            foreach(var rarity in myRarity)
            {
                roll -= rarity.Value;
                if(roll < 0)
                {
                    //Step 5
                    if(selectedRarity.ContainsKey(rarity.Key))
                    {
                        selectedRarity[rarity.Key] += 1;
                    }
                    else
                    {
                        selectedRarity.Add(rarity.Key,1);
                    }
                    break;
                }
            }
        }
        // Step 6: 
        foreach(var r in selectedRarity)
        {
            for(int i = 0; i < r.Value; i++)
            {
                var itemList = lootTable[r.Key]; 
                Tool selectedItem = itemList[rand.Next(0,itemList.Count)];
                selectedItems.Add(selectedItem);
            }
        }   
        return selectedItems;
    }

    // closes the Chest's UI output Window
    public void closeOutputWindow()
    {
        Chest_OutputUI.SetActive(false);
    }

    // Equips the generated loot from the chest to the player when the equip button is pressed.
    // Right now requires both the player and tool generator to be in the same scene.
    // May need to change how this works if we want the lootbox to be in its own scene (we probably will)
    public void OnEquipButtonPressed()
    {
        if (player == null)
        {
            Debug.LogWarning("LootChest: Player reference is missing.");
            return;
        }

        if (toolGenerator == null)
        {
            Debug.LogWarning("LootChest: ToolGenerator reference is missing.");
            return;
        }

        if (spriteManager == null)
        {
            Debug.LogWarning("LootChest: SpriteManager reference is missing.");
            return;
        }

        Tool generatedTool = getGeneratedTool();
        if (generatedTool == null)
        {
            Debug.LogWarning("LootChest: No generated tool to equip.");
            return;
        }
        Debug.Log("CHECKCHECK");

        Sprite generatedToolSprite = GetGeneratedToolSprite(generatedTool);

        bool equipped = player.EquipToolData(generatedTool, toolGenerator, generatedToolSprite);
        Debug.Log(equipped
            ? $"LootChest: Equipped '{generatedTool.Name}'."
            : "LootChest: Equip failed.");
    }

    private Sprite GetGeneratedToolSprite(Tool generatedTool)
    {
        if (generatedTool == null)
        {
            return null;
        }

        if (spriteManager.toolSprites == null)
        {
            Debug.LogWarning("LootChest: SpriteManager.toolSprites is not assigned.");
            return null;
        }

        if (generatedTool.ImageIndex < 0 || generatedTool.ImageIndex >= spriteManager.toolSprites.Length)
        {
            Debug.LogWarning($"LootChest: No sprite found for image index {generatedTool.ImageIndex}.");
            return null;
        }

        return spriteManager.toolSprites[generatedTool.ImageIndex];
    }

    private void CacheGeneratedTool(LootChest chest)
    {
        if (chest != null && chest.drops != null && chest.drops.Count > 0)
        {
            currentGeneratedTool = chest.drops[0];
        }
        else
        {
            currentGeneratedTool = null;
        }
    }

    /* not used anymore. was just for testing if toolBase would inherit values from the lootchest.
    private void SpawnLootDrops(List<Tool> dropsToSpawn)
    {
        if (toolGenerator == null || dropsToSpawn == null || dropsToSpawn.Count == 0)
        {
            return;
        }

        for (int i = 0; i < dropsToSpawn.Count; i++)
        {
            Tool drop = dropsToSpawn[i];
            toolGenerator.SpawnToolFromData(drop, Vector3.zero, Quaternion.identity, null, null);
        }
    }
    */

    public Tool getGeneratedTool()
    {
        if(drops != null && drops.Count > 0)
        {
            return drops[0];
        }

        if(currentGeneratedTool != null)
        {
            return currentGeneratedTool;
        }
        else
        {
            Debug.Log("No tool generated yet.");
            return null;
        }
    }
}

