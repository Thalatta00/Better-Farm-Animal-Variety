using BetterFarmAnimalVariety.Content;
using BetterFarmAnimalVariety.FarmAnimals;
using BetterFarmAnimalVariety.Models;
using StardewValley;
using StardewValley.Events;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Player
{
    class BreedFarmAnimals
    {
        public const byte NAMING_EVENT_NONE = 0;
        public const byte NAMING_EVENT_ANIMAL_BIRTH = 1;
        public const byte NAMING_EVENT_ANIMAL_HATCHED = 2;

        private ModEntry Mod;
        private Data_FarmAnimals FarmAnimalData;
        private string[] AvailableFarmAnimals;

        public BreedFarmAnimals(ModEntry mod)
        {
            this.Mod = mod;
            this.FarmAnimalData = new Data_FarmAnimals();
            this.AvailableFarmAnimals = this.LoadFarmAnimalsFromConfig();
        }

        private string[] LoadFarmAnimalsFromConfig()
        {
            return this.Mod.Config.GetFarmAnimalTypes();
        }

        private List<string> Sanitize(List<string> types)
        {
            // Remove anything that isn't available via the config
            types.RemoveAll(item => !this.IsAvailable(item));

            // Remove Blue <FarmAnimal>s if we need to
            types = new BlueFarmAnimals(this.Mod.Player).Sanitize(types, SpecialFarmAnimals.SAFETY_SAFE);

            return types;
        }

        public BetterFarmAnimal CreateFromParent(StardewValley.FarmAnimal parent, string name)
        {
            // @TODO: Randomize based on farm animal data/category (ex. Dairy Cow)?
            return this.CreateBaby(name, parent.type, (AnimalHouse)parent.home.indoors, parent.myID);
        }

        public void CreateFromIncubator(AnimalHouse animalHouse, string name)
        {
            Incubator Incubator;

            try
            {
                Incubator = new Incubator(animalHouse);
            }
            catch
            {
                // Could not find an incubator; do nothing.
                return;
            }

            List<string> Types = Incubator.DetermineHatchlingTypes();

            // Remove types that should not be here
            Types = this.Sanitize(Types);

            // Create the animal if we could pull the type
            this.CreateRandomBaby(name, Types, animalHouse);

            // Reset the incubator regardless if a baby was hatched or not
            Incubator.ResetIncubatingItem();
        }

        private void CreateRandomBaby(string name, List<string> Types, AnimalHouse animalHouse, long parentID = BetterFarmAnimal.PARENT_ID_DEFAULT)
        {
            if (Types.Count < 1)
                return;

            // Randomize an eligible animal type
            string Type = Types.ElementAt(Game1.random.Next(Types.Count));

            // Create the baby!
            this.CreateBaby(name, Type, animalHouse, parentID);
        }

        private BetterFarmAnimal CreateBaby(string name, string type, AnimalHouse animalHouse, long parentID = BetterFarmAnimal.PARENT_ID_DEFAULT)
        {
            BetterFarmAnimal Baby = new BetterFarmAnimal(type, this.Mod.Player.GetNewID(), this.Mod.Player.MyID);

            Baby.SynchronizeNames(name);
            Baby.AssignParent(parentID);
            Baby.RandomizeLocation(animalHouse);
            Baby.AddToAnimalHouse(animalHouse);

            return Baby;
        }

        private bool IsAvailable(string type)
        {
            return this.AvailableFarmAnimals.Contains(type);
        }

        public int DetermineNamingEvent()
        {
            if (this.IsNamingNewlyHatchedFarmAnimal())
                return BreedFarmAnimals.NAMING_EVENT_ANIMAL_HATCHED;

            if (this.IsNamingNewlyBornFarmAnimal())
                return BreedFarmAnimals.NAMING_EVENT_ANIMAL_BIRTH;

            return BreedFarmAnimals.NAMING_EVENT_NONE;
        }

        private bool IsNamingNewlyHatchedFarmAnimal()
        {
            return Incubator.CanExistInLocation(Game1.currentLocation);
        }

        private bool IsNamingNewlyBornFarmAnimal()
        {
            if (this.IsNamingNewlyHatchedFarmAnimal())
                return false;

            // Could be purchasing an animal, receiving a pet or a farm event
            // We only want to show this on the farm event
            if (this.IsFarmEvent())
                return false;

            QuestionEvent QuestionEvent = Game1.farmEvent as QuestionEvent;

            // Make sure the event was actually set up
            if (QuestionEvent.animal == null)
                return false;

            return true;
        }

        private bool IsFarmEvent()
        {
            return Game1.farmEvent == null || !(Game1.farmEvent is QuestionEvent);
        }
    }
}
