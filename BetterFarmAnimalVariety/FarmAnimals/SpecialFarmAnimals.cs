using BetterFarmAnimalVariety.Models;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.FarmAnimals
{
    abstract class SpecialFarmAnimals
    {
        public const byte SAFETY_UNSAFE = 0;
        public const byte SAFETY_SAFE = 1;

        protected BetterPlayer Player;

        public SpecialFarmAnimals(BetterPlayer player)
        {
            this.Player = player;
        }

        public abstract string GetPrefix();
        
        public bool RollChance(double chance)
        {
            return Game1.random.NextDouble() >= chance;
        }

        public bool HasSeenEvent(int eventID)
        {
            return this.Player.HasSeenEvent(eventID);
        }
        public bool HasCompletedQuest(int questID)
        {
            return this.Player.HasCompletedQuest(questID);
        }

        protected string DetermineSpecialType(string type)
        {
            return this.GetPrefix() + " " + type;
        }

        private List<string> DetermineSpecialTypes()
        {
            return FarmAnimalTypes.BASE_TYPES.Select(type => this.DetermineSpecialType(type)).ToList();
        }

        public List<string> RemoveSpecialTypesFromList(List<string> types)
        {
            List<string> toRemove = this.DetermineSpecialTypes();

            // Didn't pass the logic test so we should remove it from the list of items if it was there
            types.RemoveAll(item => toRemove.Contains(item));

            return types;
        }
    }
}
