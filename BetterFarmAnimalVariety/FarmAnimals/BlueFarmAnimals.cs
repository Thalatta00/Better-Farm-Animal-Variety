using BetterFarmAnimalVariety.Models;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.FarmAnimals
{
    class BlueFarmAnimals : SpecialFarmAnimals
    {
        public const string BLUE = "Blue";

        public const double CHANCE_VALUE = 0.25;
        public const int EVENT_ID = 3900074;

        public BlueFarmAnimals(BetterPlayer player) : base(player) { }

        public override string GetPrefix()
        {
            return BlueFarmAnimals.BLUE;
        }

        public bool AreAvailableToPlayer()
        {
            if (!this.HasSeenEvent(BlueFarmAnimals.EVENT_ID))
                return false;

            return this.RollChance(BlueFarmAnimals.CHANCE_VALUE);
        }

        public List<string> Sanitize(List<string> types, byte safety = SpecialFarmAnimals.SAFETY_UNSAFE)
        {
            List<string> ClonedTypes = new List<string>(types);

            // Make sure we account for the blue chicken rarity
            if (!this.AreAvailableToPlayer())
                types = this.RemoveSpecialTypesFromList(types);

            // Make sure we didn't remove everything if the safety is on
            // If we did, pretend the sanitize didn't happen!
            // Scenario: types only contained Blue Chicken and the chance failed
            if (safety == BlueFarmAnimals.SAFETY_SAFE && types.Count < 1)
                types = ClonedTypes;

            return types;
        }
    }
}
