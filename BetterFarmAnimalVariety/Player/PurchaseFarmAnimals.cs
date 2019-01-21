using BetterFarmAnimalVariety.Models;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Player
{
    class PurchaseFarmAnimals
    {
        private ModEntry Mod;

        public PurchaseFarmAnimals (ModEntry mod)
        {
            this.Mod = mod;
        }

        public BetterFarmAnimal RandomizeFarmAnimal(FarmAnimalStock farmAnimalStock)
        {
            // We need to randomize after the FarmAnimal is created to avoid restrictions in FarmAnimal.cs code (ex. "Blue Cow")
            List<string> Types = this.Mod.AnimalShop.GetAvailableFarmAnimalStockTypes(farmAnimalStock, AnimalShop.SANITIZE_REMOVE);

            if (Types.Count < 1)
                return null;

            // Randomly select an eligible type
            string Type = Types.ElementAt(Game1.random.Next(Types.Count));

            return new BetterFarmAnimal(Type, this.Mod.Player.GetNewID(), this.Mod.Player.MyID);
        }

        public bool CanAfford(Item stockAnimal)
        {
            return this.Mod.Player.CanAfford(stockAnimal.salePrice());
        }
    }
}
