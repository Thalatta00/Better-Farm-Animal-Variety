using BetterFarmAnimalVariety.FarmAnimals;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Models
{
    public class AnimalShop
    {
        public const byte SANITIZE_KEEP = 0;
        public const byte SANITIZE_REMOVE = 1;

        public const string DAIRY_COW = "Dairy Cow";
        public const string SHEEP = "Sheep";
        public const string GOAT = "Goat";
        public const string PIG = "Pig";
        public const string CHICKEN = "Chicken";
        public const string DUCK = "Duck";
        public const string RABBIT = "Rabbit";

        private ModEntry Mod;
        private byte VoidFarmAnimalsInShop;
        private Dictionary<string, string[]> AvailableFarmAnimalStock;

        public AnimalShop(ModEntry mod)
        {
            this.Mod = mod;
            this.VoidFarmAnimalsInShop = mod.Config.VoidFarmAnimalsInShop;

            // Use the lists because the meat indices can't be relied upon (ex. mutton)
            this.AvailableFarmAnimalStock = new Dictionary<string, string[]>() {
                {
                    AnimalShop.DAIRY_COW,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.COWS)
                },
                {
                    AnimalShop.SHEEP,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.SHEEP)
                },
                {
                    AnimalShop.GOAT,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.GOATS)
                },
                {
                    AnimalShop.PIG,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.PIGS)
                },
                {
                    AnimalShop.CHICKEN,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.CHICKENS)
                },
                {
                    AnimalShop.DUCK,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.DUCKS)
                },
                {
                    AnimalShop.RABBIT,
                    mod.Config.GetFarmAnimalTypesByGroup(ModConfig.RABBITS)
                },
            };
        }

        public Dictionary<string, string[]> GetAvailableFarmAnimalStock()
        {
            return this.AvailableFarmAnimalStock;
        }

        public List<string> GetAvailableFarmAnimalStockTypes(FarmAnimalStock farmAnimalStock, byte sanitize = AnimalShop.SANITIZE_KEEP)
        {
            List<string> Types = this.GetAvailableFarmAnimalStock()[farmAnimalStock.Name].ToList();

            if (sanitize == AnimalShop.SANITIZE_REMOVE)
            {
                // Make sure we account for the Blue <FarmAnimal> rarity
                Types = new BlueFarmAnimals(this.Mod).Sanitize(Types);

                // We also need to make sure we're not including Void <FarmAnimal>s in the shop if they don't want them
                Types = new VoidFarmAnimals(this.Mod).SanitizeForShop(Types, SpecialFarmAnimals.SAFETY_SAFE);
            }

            return Types;
        }
    }
}
