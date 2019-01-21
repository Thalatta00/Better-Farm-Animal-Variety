using System.Collections.Generic;

namespace BetterFarmAnimalVariety.FarmAnimals
{
    class FarmAnimalTypes
    {
        // Bases including extension for male animals
        public const string COW = "Cow";
        public const string BULL = "Bull";
        public const string RAM = "Ram";
        public const string BILLY_GOAT = "Billy Goat";
        public const string CHICKEN = "Chicken";
        public const string ROOSTER = "Rooster";
        public const string DRAKE = "Drake";

        // Vanilla
        public const string WHITE_COW = "White Cow";
        public const string BROWN_COW = "Brown Cow";
        public const string SHEEP = "Sheep";
        public const string GOAT = "Goat";
        public const string PIG = "Pig";
        public const string WHITE_CHICKEN = "White Chicken";
        public const string BROWN_CHICKEN = "Brown Chicken";
        public const string BLUE_CHICKEN = "Blue Chicken";
        public const string VOID_CHICKEN = "Void Chicken";
        public const string DUCK = "Duck";
        public const string RABBIT = "Rabbit";
        public const string DINOSAUR = "Dinosaur";

        // Unused
        public const string HOG = "Hog";

        // Not vanilla, but assume a <Special> <Male> is the counterpart to the <Special> <Female> if it exists
        public static readonly List<string> BASE_TYPES = new List<string> {
            FarmAnimalTypes.COW,
            FarmAnimalTypes.BULL,
            FarmAnimalTypes.SHEEP,
            FarmAnimalTypes.GOAT,
            FarmAnimalTypes.BILLY_GOAT,
            FarmAnimalTypes.RAM,
            FarmAnimalTypes.PIG,
            FarmAnimalTypes.HOG,
            FarmAnimalTypes.CHICKEN,
            FarmAnimalTypes.ROOSTER,
            FarmAnimalTypes.DUCK,
            FarmAnimalTypes.DRAKE,
            FarmAnimalTypes.RABBIT,
            FarmAnimalTypes.DINOSAUR,
        };
    }
}
