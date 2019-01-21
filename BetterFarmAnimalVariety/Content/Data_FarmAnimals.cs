using System.Collections.Generic;

namespace BetterFarmAnimalVariety.Content
{
    class Data_FarmAnimals : Data
    {
        private const string FILE_PATH = "Data\\FarmAnimals";

        public const byte DEFAULT_PRODUCE_INDEX = 2;
        public const byte DELUXE_PRODUCE_INDEX = 3;
        public const byte DISPLAY_NAME = 25;

        public Data_FarmAnimals()
        {
            this.Entries = this.Load();
        }

        public override string GetFilePath()
        {
            return Data_FarmAnimals.FILE_PATH;
        }

        public bool ProducesItem(string key, string produceIndex)
        {
            string[] DataArr = this.Split(this.GetEntries()[key]);

            return produceIndex.Equals(DataArr[Data_FarmAnimals.DEFAULT_PRODUCE_INDEX]) || produceIndex.Equals(DataArr[Data_FarmAnimals.DELUXE_PRODUCE_INDEX]);
        }

        public List<string> FindTypesByProduce(string produceIndex)
        {
            List<string> Types = new List<string>();
            Dictionary<string, string> Entries = this.GetEntries();

            // Grab the animal based on what they produce! Do this to not mix up White Chickens and Void Chickens, etc.
            foreach (KeyValuePair<string, string> entry in Entries)
            {
                // Check against default and deluxe produce
                if (this.ProducesItem(entry.Key, produceIndex))
                    Types.Add(entry.Key);
            }

            return Types;
        }
    }
}
