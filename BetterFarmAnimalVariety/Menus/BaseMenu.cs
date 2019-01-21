using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BetterFarmAnimalVariety.Menus
{
    class BaseMenu
    {
        protected ModEntry Mod;

        public BaseMenu(ModEntry mod)
        {
            this.Mod = mod;
        }

        protected Point GetCursorCoordinates(ButtonPressedEventArgs e)
        {
            int X = (int)e.Cursor.ScreenPixels.X;
            int Y = (int)e.Cursor.ScreenPixels.Y;

            return new Point(X, Y);
        }

        protected void SuppressButton(SButton button)
        {
            this.Mod.Helper.Input.Suppress(button);
        }
    }
}
