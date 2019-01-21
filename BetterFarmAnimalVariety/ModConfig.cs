using BetterFarmAnimalVariety.Helpers;
using BetterFarmAnimalVariety.Models;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety
{
    public class ModConfig
    {
        public const string COWS = "Cows";
        public const string CHICKENS = "Chickens";
        public const string SHEEP = "Sheep";
        public const string GOATS = "Goats";
        public const string PIGS = "Pigs";
        public const string DUCKS = "Ducks";
        public const string RABBITS = "Rabbits";
        public const string DINOSAURS = "Dinosaurs";

        public const byte VOID_FARM_ANIMALS_IN_SHOP_NEVER = 0;
        public const byte VOID_FARM_ANIMALS_IN_SHOP_QUEST_ONLY = 1;
        public const byte VOID_FARM_ANIMALS_IN_SHOP_ALWAYS = 2;

        public byte VoidFarmAnimalsInShop;
        public Dictionary<string, ConfigFarmAnimal> FarmAnimals;

        private AppSettings AppSettings;

        public ModConfig()
        {
            Dictionary<string, string> Settings = Properties.Settings.Default.Properties
              .Cast<System.Configuration.SettingsProperty>()
              .OrderBy(s => s.Name).ToDictionary(x => x.Name.ToString(), x => x.DefaultValue.ToString());

            this.AppSettings = new AppSettings(Settings);

            this.VoidFarmAnimalsInShop = ModConfig.VOID_FARM_ANIMALS_IN_SHOP_NEVER;

            this.InitializeFarmAnimals();
        }

        private List<string> GetFarmAnimalGroups()
        {
            return this.FarmAnimals.Keys.ToList<string>();
        }

        public string[] GetFarmAnimalTypes(string farmAnimalGroup = null)
        {
            if (farmAnimalGroup != null)
                return this.GetFarmAnimalTypesByGroup(farmAnimalGroup);

            List<string> Types = new List<string>();

            foreach (KeyValuePair<string, ConfigFarmAnimal> Entry in this.FarmAnimals)
                Types = Types.Concat(Entry.Value.GetTypes()).ToList<string>();

            return Types.ToArray();
        }

        public string[] GetFarmAnimalTypesByGroup(string farmAnimalGroup)
        {
            return this.FarmAnimals[farmAnimalGroup].GetTypes();
        }

        private void InitializeFarmAnimals()
        {
            this.FarmAnimals = new Dictionary<string, ConfigFarmAnimal>();

            this.UpdateFarmAnimalValuesFromAppSettings();
        }

        public void UpdateFarmAnimalValuesFromAppSettings()
        {
            List<AppSetting> FarmAnimalAppSettings = this.AppSettings.FindFarmAnimalAppSettings();

            foreach (AppSetting AppSetting in FarmAnimalAppSettings)
            {
                ConfigFarmAnimal ConfigFarmAnimal = new ConfigFarmAnimal(AppSetting);
                  
                if (this.FarmAnimals.ContainsKey(ConfigFarmAnimal.Group))
                {
                    // Preserve user preferences if the group already has data loaded from the user's config JSON
                    ConfigFarmAnimal.Name = this.FarmAnimals[ConfigFarmAnimal.Group].Name;
                    ConfigFarmAnimal.Description = this.FarmAnimals[ConfigFarmAnimal.Group].Description;
                    ConfigFarmAnimal.ShopIcon = this.FarmAnimals[ConfigFarmAnimal.Group].ShopIcon;
                    ConfigFarmAnimal.Types = this.FarmAnimals[ConfigFarmAnimal.Group].Types;

                    // Add the configuration to the farm animals config
                    this.FarmAnimals[ConfigFarmAnimal.Group] = ConfigFarmAnimal;
                }
                else
                {
                    this.FarmAnimals.Add(ConfigFarmAnimal.Group, ConfigFarmAnimal);
                }
            }
        }
    }
}