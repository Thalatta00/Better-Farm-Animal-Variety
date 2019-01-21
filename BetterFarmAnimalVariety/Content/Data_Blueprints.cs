namespace BetterFarmAnimalVariety.Content
{
    class Data_Blueprints : Data
    {
        public const int NAME_INDEX = 4;
        public const int DESCRIPTION_INDEX = 5;

        private const string FILE_PATH = "Data\\Blueprints";

        public Data_Blueprints()
        {
            this.Entries = this.Load();
        }

        public override string GetFilePath()
        {
            return Data_Blueprints.FILE_PATH;
        }
    }
}
