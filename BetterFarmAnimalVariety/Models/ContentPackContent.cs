using BetterFarmAnimalVariety.Content;
using BetterFarmAnimalVariety.FarmAnimals;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Models
{
    class ContentPackContent
    {
        public string DataKey = null;
        public string DataValue = null;
        public Dictionary<string, string> DisplayNameLocalization = null;
        public Dictionary<string, string> Sprites = null;

        private const string DATA_FILE_PATH_FIND = "\\";
        private const string DATA_FILE_PATH_REPLACE = "/";

        private Data_FarmAnimals Data_FarmAnimals;

        public ContentPackContent()
        {
            this.Data_FarmAnimals = new Data_FarmAnimals();
        }

        public string GetLocalizedDisplayName(string locale)
        {
            if (!this.DisplayNameLocalization.ContainsKey(locale))
                return "";

            return this.DisplayNameLocalization[locale];
        }

        private string FormatAssetName(string path)
        {
            return path.Replace(ContentPackContent.DATA_FILE_PATH_FIND, ContentPackContent.DATA_FILE_PATH_REPLACE);
        }

        public string GetTargetData_FarmAnimalsAssetName()
        {
            return this.FormatAssetName(this.Data_FarmAnimals.GetFilePath());
        }

        public string GetTargetAnimals_AssetName(string type)
        {
            FarmAnimalSprites FarmAnimalSprites = new FarmAnimalSprites(this.DataKey);

            return this.FormatAssetName(FarmAnimalSprites.FormatFilePath(type));
        }
    }
}
