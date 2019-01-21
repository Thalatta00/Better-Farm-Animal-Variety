using BetterFarmAnimalVariety.Content;
using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Editors
{
    class ContentPackEditor : IAssetEditor
    {
        private ModEntry Mod;
        private ContentPack ContentPack;

        public ContentPackEditor(ModEntry mod, ContentPack contentPack)
        {
            this.Mod = mod;
            this.ContentPack = contentPack;
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals(this.ContentPack.Content.GetTargetData_FarmAnimalsAssetName()))
                return true;

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals(this.ContentPack.Content.GetTargetData_FarmAnimalsAssetName()))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
                string Value = this.ContentPack.Content.DataValue;
                string LocalizedDisplayName = this.ContentPack.Content.GetLocalizedDisplayName(this.Mod.Helper.Translation.Locale);

                if (LocalizedDisplayName.Length > 0)
                {
                    string[] fields = Value.Split('/');
                    fields[Data_FarmAnimals.DISPLAY_NAME] = LocalizedDisplayName;
                    Value = string.Join("/", fields);
                }

                data[this.ContentPack.Content.DataKey] = Value;
            }
        }
    }
}
