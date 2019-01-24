using BetterFarmAnimalVariety.Models;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;

namespace BetterFarmAnimalVariety.FarmAnimals
{
    public class FarmAnimalSprites
    {
        public const int STARTING_FRAME = 0;
        private const string ASSET_DIRECTORY = "Animals\\";
        private const string BABY = "Baby";

        private BetterFarmAnimal FarmAnimal;
        private string Type;

        public FarmAnimalSprites(string type)
        {
            this.Type = type;
        }

        public FarmAnimalSprites(BetterFarmAnimal FarmAnimal)
        {
            this.FarmAnimal = FarmAnimal;
            this.Type = FarmAnimal.type;
        }

        private string GetAssetDirectory()
        {
            return FarmAnimalSprites.ASSET_DIRECTORY;
        }

        private string GetBaby()
        {
            return FarmAnimalSprites.BABY;
        }

        private bool FarmAnimalExists()
        {
            return this.Type != null;
        }

        private string GetDefaultFilePath()
        {
            // Use the BabyWhite Chicken image as default for anything that could not be found
            return this.GetAssetDirectory() + this.GetBaby() + FarmAnimalTypes.WHITE_CHICKEN;
        }

        private string FormatAgedFilePath()
        {
            if (!this.FarmAnimalExists())
                throw new Exception();

            return this.FarmAnimal.isBaby() ? this.FormatBabyFilePath() : this.FormatAdultFilePath();
        }

        public string FormatAdultFilePath()
        {
            return this.FormatFilePath(this.Type);
        }

        public string FormatBabyFilePath()
        {
            // No space between
            return this.FormatFilePath(this.GetBaby() + this.Type);
        }

        public string FormatFilePath(string fileName)
        {
            // No space between
            return this.GetAssetDirectory() + fileName;
        }

        private string BuildFilePath()
        {
            return this.FormatAgedFilePath();
        }

        public string DetermineFilePath()
        {
            try
            {
                // Build the filepath
                string FilePath = this.BuildFilePath();

                // Check if we can load a <Baby>type image
                Game1.content.Load<Texture2D>(FilePath);

                // Success!
                return FilePath;
            }
            catch
            {
                // Vanilla Stardew uses the BabyWhite Chicken image for the BabyDuck
                // @TODO: Throw an exception to be caught in ModEntry to let the user know!
                return this.GetDefaultFilePath();
            }
        }
    }

}
