using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Loaders
{
    class ContentPackLoader : IAssetLoader
    {
        private ContentPack ContentPack;

        public ContentPackLoader(ContentPack contentPack)
        {
            this.ContentPack = contentPack;
        }

        /// <summary>Get whether this instance can load the initial version of the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanLoad<T>(IAssetInfo asset)
        {
            foreach (KeyValuePair<string, string> Entry in this.ContentPack.Content.Sprites)
            {
                if (asset.AssetNameEquals(this.ContentPack.Content.GetTargetAnimals_AssetName(Entry.Key)))
                    return true;
            }

            return false;
        }

        /// <summary>Load a matched asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public T Load<T>(IAssetInfo asset)
        {
            foreach (KeyValuePair<string, string> Entry in this.ContentPack.Content.Sprites)
            {
                if (asset.AssetNameEquals(this.ContentPack.Content.GetTargetAnimals_AssetName(Entry.Key)))
                    return this.ContentPack.IContentPack.LoadAsset<T>(Entry.Value); ;
            }

            throw new InvalidOperationException($"Unexpected asset '{asset.AssetName}'.");
        }
    }
}
