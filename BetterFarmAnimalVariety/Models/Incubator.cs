using BetterFarmAnimalVariety.Content;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Models
{
    class Incubator
    {
        private const string COOP = "Coop";
        private const string INCUBATOR = "Incubator";

        private const StardewValley.Object HELD_OBJECT_DEFAULT = null;
        private const int ITEM_INDEX_DEFAULT = 101;
        private const int INCUBATING_EGG_X_DEFAULT = 0;
        private const int INCUBATING_EGG_Y_DEFAULT = -1;

        private StardewValley.Object Self;
        public AnimalHouse AnimalHouse;

        public Incubator(StardewValley.Object @object, AnimalHouse animalHouse)
        {
            this.Self = @object;
            this.AnimalHouse = animalHouse;
        }

        public Incubator(AnimalHouse animalHouse)
        {
            StardewValley.Object @object = this.FindInAnimalHouse(animalHouse);

            if (@object == null)
                throw new ArgumentException("Could not find an incubator", "animalHouse");

            this.Self = @object;
            this.AnimalHouse = animalHouse;
        }

        private StardewValley.Object FindInAnimalHouse(AnimalHouse animalHouse)
        {
            // Try to get the reference for the incubator object
            foreach (StardewValley.Object @object in animalHouse.objects.Values)
            {
                if (@object.bigCraftable && @object.Name.Contains(Incubator.INCUBATOR) && @object.heldObject != null)
                    return @object;
            }

            return null;
        }

        public void ResetIncubatingItem()
        {
            this.ResetSelfIncubatingItem();
            this.ResetAnimalHouseIncubatingItem();
        }

        private void ResetSelfIncubatingItem()
        {
            // Remove the item from the Incubator
            this.Self.heldObject.Set(Incubator.HELD_OBJECT_DEFAULT);
            this.Self.ParentSheetIndex = Incubator.ITEM_INDEX_DEFAULT;
        }

        private void ResetAnimalHouseIncubatingItem()
        {
            // DEFAULT the animal house
            this.AnimalHouse.incubatingEgg.X = Incubator.INCUBATING_EGG_X_DEFAULT;
            this.AnimalHouse.incubatingEgg.Y = Incubator.INCUBATING_EGG_Y_DEFAULT;
        }
       
        public string GetIncubatingItemIndex()
        {
            return this.Self.heldObject.Value.parentSheetIndex.ToString();
        }

        public static bool CanExistInLocation(GameLocation location)
        {
            // It comes with the Big Coop and the Deluxe Coop
            return location.Name != Incubator.COOP && location.Name.Contains(Incubator.COOP);
        }

        public List<string> DetermineHatchlingTypes()
        {
            // Grab the held item's index #
            string HeldItemIndex = this.GetIncubatingItemIndex();

            // Randomize an eligible animal type from parents
            List<string> Types = this.DetermineTypesFromPossibleParents();

            // @TODO: VERY SMALL random chance to get something you don't own? Leave to random event?
            if (Types.Count > 0)
                return Types;

            return this.DetermineTypesFromProduce();
        }

        public List<string> DetermineTypesFromPossibleParents()
        {
            List<string> Types = new List<string>();

            Dictionary<long, StardewValley.FarmAnimal> possibleParents = this.AnimalHouse.animals.Pairs.ToDictionary(pair => pair.Key, pair => pair.Value);

            // Validate the potential types against what is in the coop
            foreach (KeyValuePair<long, StardewValley.FarmAnimal> entry in possibleParents)
            {
                // Already has this type
                if (Types.Contains(entry.Value.type))
                    continue;

                // Babies cannot be parents
                if (entry.Value.isBaby())
                    continue;

                // Must be an adult and must produce this item
                if ((new Data_FarmAnimals()).ProducesItem((string)entry.Value.type, this.GetIncubatingItemIndex()))
                    Types.Add(entry.Value.type);
            }

            return Types;
        }

        public List<string> DetermineTypesFromProduce()
        {
            return (new Data_FarmAnimals()).FindTypesByProduce(this.GetIncubatingItemIndex());
        }
    }
}
