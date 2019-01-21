using StardewValley;
using StardewValley.Quests;
using System.Collections.Generic;
using System.Linq;

namespace BetterFarmAnimalVariety.Models
{
    public class BetterPlayer : Farmer
    {
        public Farmer Farmer;
        private BetterPlayer.GetNewMultiplayerIDBehavior GetNewMultiplayerID;

        public long MyID
        {
            get
            {
                return this.Farmer.UniqueMultiplayerID;
            }
        }

        public BetterPlayer(Farmer Farmer, BetterPlayer.GetNewMultiplayerIDBehavior getNewMultiplayerID = null)
        {
            this.Farmer = Farmer;
            this.GetNewMultiplayerID = getNewMultiplayerID;
        }

        public long GetNewID()
        {
            return this.GetNewMultiplayerID();
        }

        public bool CanAfford(int cost)
        {
            return this.Farmer.money >= cost;
        }

        public bool HasSeenEvent(int eventID)
        {
            return this.Farmer.eventsSeen.Contains(eventID);
        }

        public bool HasCompletedQuest(int questID)
        {
            // Check the quest log
            List<Quest> QuestLog = this.Farmer.questLog.ToList<Quest>();

            foreach (Quest Quest in QuestLog)
            {
                if (Quest.id == questID && Quest.completed)
                    return true;
            }

            return false;
        }

        public void ContinueCurrentEvent()
        {
            if (Game1.currentLocation.currentEvent != null)
                ++Game1.currentLocation.currentEvent.CurrentCommand;
        }

        public delegate long GetNewMultiplayerIDBehavior();
    }
}
