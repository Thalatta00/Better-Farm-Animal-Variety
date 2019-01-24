using BetterFarmAnimalVariety.Models;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.FarmAnimals
{
    class VoidFarmAnimals : SpecialFarmAnimals
    {
        public const string VOID = "Void";
        public const int QUEST_ID = 27; // Goblin Problem - end game quest as of SDV v.1.3.27

        private int AllowInShop;

        public VoidFarmAnimals(BetterPlayer player, ModConfig config) : base(player)
        {
            this.AllowInShop = config.VoidFarmAnimalsInShop;
        }

        public override string GetPrefix()
        {
            return VoidFarmAnimals.VOID;
        }

        public bool IsInShop(byte condition)
        {
            return this.AllowInShop == condition;
        }

        public bool IsAlwaysInShop()
        {
            return this.IsInShop(ModConfig.VOID_FARM_ANIMALS_IN_SHOP_ALWAYS);
        }

        public bool IsNeverInShop()
        {
            return this.IsInShop(ModConfig.VOID_FARM_ANIMALS_IN_SHOP_NEVER);
        }

        public bool CanPurchaseFromShop()
        {
            if (this.IsAlwaysInShop())
                return true;

            if (this.IsNeverInShop())
                return false;

            if (!this.HasCompletedQuest(VoidFarmAnimals.QUEST_ID))
                return false;

            // Use the same chance used for the Vanilla Blue Chickens
            return this.RollChance(BlueFarmAnimals.CHANCE_VALUE);
        }

        public List<string> SanitizeForShop(List<string> types, byte safety = SpecialFarmAnimals.SAFETY_UNSAFE)
        {
            List<string> ClonedTypes = new List<string>(types);

            // Make sure we account for the Void Chicken rarity
            if (!this.CanPurchaseFromShop())
                types = this.RemoveSpecialTypesFromList(types);

            // Make sure we didn't remove everything if the safety is on
            // If we did, pretend the sanitize didn't happen!
            // Scenario: types only contained Void Chicken and the config does not allow them in the shop
            if (safety == SpecialFarmAnimals.SAFETY_UNSAFE && types.Count < 1)
                types = ClonedTypes;

            return types;
        }
    }
}
