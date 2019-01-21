using BetterFarmAnimalVariety.Player;
using StardewValley;
using StardewValley.Events;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Menus
{
    class NameFarmAnimalMenu : BaseMenu
    {
        private BreedFarmAnimals BreedFarmAnimals;
        private NamingMenu NamingMenu;

        private string Title
        {
            get
            {
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestionEvent.cs.6692");
            }
        }

        public NameFarmAnimalMenu(ModEntry mod, NamingMenu namingMenu) : base(mod)
        {
            this.BreedFarmAnimals = new BreedFarmAnimals(mod);
            this.NamingMenu = namingMenu;
        }

        public bool IsCustomerNamingMenuOpen()
        {
            return this.NamingMenu.GetType() == typeof(BetterNamingMenu);
        }

        public void HandleChange()
        {
            if (this.IsCustomerNamingMenuOpen())
                return;

            NamingMenu.doneNamingBehavior DoneNaming = this.DetermineDoneNamingBehavior(this.BreedFarmAnimals.DetermineNamingEvent());

            (new ActiveClickableMenu(this.Mod)).ExitWithoutSound();

            if (DoneNaming != null)
                this.OpenCustomNamingMenu(DoneNaming, this.Title);
        }
        
        public NamingMenu.doneNamingBehavior DetermineDoneNamingBehavior(int NamingEvent)
        {
            switch (NamingEvent)
            {
                case BreedFarmAnimals.NAMING_EVENT_ANIMAL_BIRTH:
                    return new NamingMenu.doneNamingBehavior(this.HandleFarmAnimalBirth);
                case BreedFarmAnimals.NAMING_EVENT_ANIMAL_HATCHED:
                    return new NamingMenu.doneNamingBehavior(this.HandleFarmAnimalHatched);
                default:
                    return null;
            }
        }

        public void OpenCustomNamingMenu(NamingMenu.doneNamingBehavior DoneNaming, string Title, string DefaultName = null)
        {
            // Launch the custom one
            BetterNamingMenu BetterNamingMenu = new BetterNamingMenu(DoneNaming, Title, DefaultName);

            this.NamingMenu = BetterNamingMenu;

            Game1.activeClickableMenu = (IClickableMenu)this.NamingMenu;
        }

        private void HandleFarmAnimalHatched(string name)
        {
            // Create the baby animal
            this.BreedFarmAnimals.CreateFromIncubator(Game1.currentLocation as AnimalHouse, name);
            this.HandleAfterFarmAnimalIsCreated();
        }

        private void HandleFarmAnimalBirth(string name)
        {
            QuestionEvent QuestionEvent = Game1.farmEvent as QuestionEvent;

            // Create the baby animal
            this.BreedFarmAnimals.CreateFromParent(QuestionEvent.animal, name);

            // Specific for this animal event
            QuestionEvent.forceProceed = true;

            this.HandleAfterFarmAnimalIsCreated();
        }

        private void HandleAfterFarmAnimalIsCreated()
        {
            // Continue the current event if there is one
            this.Mod.Player.ContinueCurrentEvent();

            // Exit the active menu
            (new ActiveClickableMenu(this.Mod)).Exit();
            this.NamingMenu = (NamingMenu)null;
        }

    }
}
