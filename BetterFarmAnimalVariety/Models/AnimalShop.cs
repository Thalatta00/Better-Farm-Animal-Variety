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

        private static readonly Dictionary<string, string> MappedConfigKeysToStockNames = new Dictionary<string, string>() {
            {
                ModConfig.COWS,
                AnimalShop.DAIRY_COW
            },
            {
                ModConfig.SHEEP,
                AnimalShop.SHEEP
            },
            {
                ModConfig.GOATS,
                AnimalShop.GOAT
            },
            {
                ModConfig.PIGS,
                AnimalShop.PIG
            },
            {
                ModConfig.CHICKENS,
                AnimalShop.CHICKEN
            },
            {
                ModConfig.DUCKS,
                AnimalShop.DUCK
            },
            {
                ModConfig.RABBITS,
                AnimalShop.RABBIT
            },
        };

        private BlueFarmAnimals BlueFarmAnimals;
        private VoidFarmAnimals VoidFarmAnimals;
        private Dictionary<string, string[]> AvailableFarmAnimalStock;

        public AnimalShop(BetterPlayer player, ModConfig config)
        {
            this.BlueFarmAnimals = new BlueFarmAnimals(player);
            this.VoidFarmAnimals = new VoidFarmAnimals(player, config);

            // Use the lists because the meat indices can't be relied upon (ex. mutton)
            this.AvailableFarmAnimalStock = new Dictionary<string, string[]>();
            
            foreach (KeyValuePair<string, string> entry in AnimalShop.MappedConfigKeysToStockNames)
                this.AvailableFarmAnimalStock.Add(entry.Value, config.GetFarmAnimalTypesByGroup(entry.Key));
        }

        public string MapConfigKeyToStockName(string configKey)
        {
            return AnimalShop.MappedConfigKeysToStockNames[configKey];
        }

        public FarmAnimalStock DetermineStock(string type)
        {
            return new FarmAnimalStock(this.DetermineStockName(type));
        }

        public string DetermineStockName(string type)
        {
            Dictionary<string, string[]> stock = this.GetAvailableFarmAnimalStock();

            foreach (KeyValuePair<string, string[]> entry in stock)
            {
                foreach (string stockType in entry.Value)
                {
                    if (type == stockType)
                        return entry.Key;
                }
            }

            return null;
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
                Types = this.BlueFarmAnimals.Sanitize(Types);

                // We also need to make sure we're not including Void <FarmAnimal>s in the shop if they don't want them
                Types = this.VoidFarmAnimals.SanitizeForShop(Types, SpecialFarmAnimals.SAFETY_SAFE);
            }

            return Types;
        }
    }
}
