using UnityEngine;

// no longer used, keeping for reference -josh
public class Tool_old
    {
        public string Rarity;
        public string Name;
        public string Description;
        public int Durability;
        public int MiningSpeed;
        public int MiningDamage;
        public int ImageIndex;

         public Tool_old(string rarity, string name, string description, int durability, int miningSpeed, int miningDamage, int imageIndex = 0)
        {
            Rarity = rarity;
            Name = name;
            Description = description;
            Durability = durability;
            MiningSpeed = miningSpeed;
            MiningDamage = miningDamage;
            ImageIndex = imageIndex;
        }
    }