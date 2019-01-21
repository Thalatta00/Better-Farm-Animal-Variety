using StardewModdingAPI;

namespace BetterFarmAnimalVariety.Models
{
    class ContentPack
    {
        public IContentPack IContentPack;
        public ContentPackContent Content;

        public ContentPack(IContentPack contentPack)
        {
            this.IContentPack = contentPack;
            this.Content = contentPack.ReadJsonFile<ContentPackContent>("content.json");
        }
    }
}
