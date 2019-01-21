using BetterFarmAnimalVariety.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System.Collections.Generic;
using System.IO;

namespace BetterFarmAnimalVariety.Editors
{
    class AnimalShopEditor : IAssetEditor
    {
        private ModEntry Mod;

        public AnimalShopEditor(ModEntry mod)
        {
            this.Mod = mod;
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals("Strings/StringsFromCSFiles"))
                return true;

            if (asset.AssetNameEquals("LooseSprites/Cursors"))
                return true;

            return false;
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Strings/StringsFromCSFiles"))
            {
                this.Strings_StringsFromCSFiles(asset);
                return;
            }

            if (asset.AssetNameEquals("LooseSprites/Cursors"))
            {
                this.LooseSprites_Cursors(asset);
                return;
            }
        }

        private void Strings_StringsFromCSFiles(IAssetData asset)
        {
            IDictionary<string, string> Strings = asset.AsDictionary<string, string>().Data;

            foreach (KeyValuePair<string, ConfigFarmAnimal> Entry in this.Mod.Config.FarmAnimals)
            {
                // Set the new name
                if (!Entry.Value.IsDefault(Entry.Value.Name))
                    this.EditAnimalShopName(Entry.Value, Strings);

                // Set the new desciption
                if (!Entry.Value.IsDefault(Entry.Value.Description))
                    this.EditAnimalShopDescription(Entry.Value, Strings);
            }
        }

        private void LooseSprites_Cursors(IAssetData asset)
        {
            foreach (KeyValuePair<string, ConfigFarmAnimal> Entry in this.Mod.Config.FarmAnimals)
            {
                // Set the new icon
                if (!Entry.Value.IsDefault(Entry.Value.ShopIcon))
                    this.EditAnimalShopIcon(Entry.Value, asset);
            }
        }

        private void EditAnimalShopInfo(IDictionary<string, string> strings, string key, string value)
        {
            strings[key] = value;
        }

        private void EditAnimalShopName(ConfigFarmAnimal configFarmAnimal, IDictionary<string, string> strings)
        {
            this.EditAnimalShopInfo(strings, configFarmAnimal.DetermineAnimalShopNameKey(), configFarmAnimal.Name);
        }

        private void EditAnimalShopDescription(ConfigFarmAnimal configFarmAnimal, IDictionary<string, string> strings)
        {
            this.EditAnimalShopInfo(strings, configFarmAnimal.DetermineAnimalShopDescriptionKey(), configFarmAnimal.Description);
        }

        private void EditAnimalShopIcon(ConfigFarmAnimal configFarmAnimal, IAssetData asset)
        {
            Texture2D customTexture = this.Mod.Helper.Content.Load<Texture2D>(Path.Combine(Properties.Settings.Default.AssetsDirectory, configFarmAnimal.ShopIcon), ContentSource.ModFolder);
            Rectangle TargetArea = configFarmAnimal.GetAnimalShopIconToArea();

            asset.AsImage().PatchImage(customTexture, targetArea: TargetArea);
        }
    }
}
