using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Menus
{
    class ActiveClickableMenu : BaseMenu
    {
        private IClickableMenu ClickableMenu;

        public ActiveClickableMenu(ModEntry mod, IClickableMenu clickableMenu = null) : base(mod)
        {
            this.ClickableMenu = clickableMenu ?? Game1.activeClickableMenu;
        }

        public IClickableMenu GetMenu()
        {
            return this.ClickableMenu;
        }

        public bool IsOpen()
        {
            return this.ClickableMenu != null;
        }

        public void Exit()
        {
            if (!this.IsOpen())
                return;

            // Does the following: Game1.activeClickableMenu = (IClickableMenu)null;
            Game1.exitActiveMenu();
            this.ClickableMenu = null;
        }

        public void ExitWithSound(string sound)
        {
            this.Exit();
            this.PlaySound(sound);
        }

        private void PlaySound(string sound)
        {
            Game1.playSound(sound);
        }

        public void ExitWithoutSound()
        {
            if (this.IsOpen())
                return;

            this.ClickableMenu.exitThisMenuNoSound();
        }

        private IReflectedField<T> GetField<T>(string field)
        {
            return ((IReflectedField<T>)this.Mod.Helper.Reflection.GetField<T>((object)this.ClickableMenu, field, true));
        }

        public T GetValue<T>(string field)
        {
            return this.GetField<T>(field).GetValue();
        }

        public void SetValue<T>(string field, T value)
        {
            this.GetField<T>(field).SetValue(value);
        }
    }
}
