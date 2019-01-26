using BetterFarmAnimalVariety.Models;
using Newtonsoft.Json;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop;
using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Players;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;
using System.IO;

namespace BetterFarmAnimalVariety
{
    public class ModApi
    {
        private ModConfig Config;

        public ModApi(ModConfig config)
        {
            this.Config = config;
        }

        public Blue GetBlueFarmAnimals(Player player)
        {
            BlueConfig blueConfig = new BlueConfig(player.HasSeenEvent(Blue.EVENT_ID));
            return new Blue(blueConfig);
        }

        public Void GetVoidFarmAnimals(Player player)
        {
            VoidConfig voidConfig = new VoidConfig(this.Config.VoidFarmAnimalsInShop, player.HasCompletedQuest(Void.QUEST_ID));
            return new Void(voidConfig);
        }

        public AnimalShop GetAnimalShop(Player player)
        {
            Dictionary<Stock.Name, string[]> available = this.Config.MapFarmAnimalsToAvailableAnimalShopStock();
            StockConfig stockConfig = new StockConfig(available, this.GetBlueFarmAnimals(player), this.GetVoidFarmAnimals(player));
            Stock stock = new Stock(stockConfig);

            return new AnimalShop(stock);
        }
    }
}
