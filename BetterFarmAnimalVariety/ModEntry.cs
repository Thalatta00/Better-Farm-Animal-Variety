using BetterFarmAnimalVariety.Editors;
using BetterFarmAnimalVariety.Loaders;
using BetterFarmAnimalVariety.Menus;
using BetterFarmAnimalVariety.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public ModConfig Config;
        public BetterPlayer Player;
        public AnimalShop AnimalShop;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.LoadConfig();

            // Asset Editors
            this.Helper.Content.AssetEditors.Add(new AnimalBirthEditor(this));
            this.Helper.Content.AssetEditors.Add(new AnimalShopEditor(this));

            // Content Packs
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                ContentPack ContentPack = new ContentPack(contentPack);

                this.Helper.Content.AssetEditors.Add(new ContentPackEditor(this, ContentPack));
                this.Helper.Content.AssetLoaders.Add(new ContentPackLoader(ContentPack));
            }

            // Events
            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
            this.Helper.Events.Display.RenderingActiveMenu += this.Display_RenderingActiveMenu;
            this.Helper.Events.Input.ButtonPressed += this.Input_ButtonPressed;
        }

        private ModConfig LoadConfig()
        {
            // Load up the config
            ModConfig Config = this.Helper.ReadConfig<ModConfig>();

            // Set up the default values
            Config.UpdateFarmAnimalValuesFromAppSettings();

            return Config;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Player = new BetterPlayer(Game1.player, this.Helper.Multiplayer.GetNewID);

            // Set up their Animal Shop
            this.AnimalShop = new AnimalShop(this);
        }

        private void Display_RenderingActiveMenu(object sender, RenderingActiveMenuEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady || Game1.activeClickableMenu == null)
                return;

            NamingMenu NamingMenu = Game1.activeClickableMenu as NamingMenu;

            if (NamingMenu == null)
                return;

            if (NamingMenu.GetType() != typeof(StardewValley.Menus.NamingMenu))
                return;

            NameFarmAnimalMenu NameFarmAnimalMenu = new NameFarmAnimalMenu(this, NamingMenu);

            NameFarmAnimalMenu.HandleChange();
        }
        
        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // We only care about left mouse clicks right now
            if (e.Button != SButton.MouseLeft)
                    return;

            ActiveClickableMenu ActiveClickableMenu = new ActiveClickableMenu(this);

            if (!ActiveClickableMenu.IsOpen())
                return;

            // Purchasing a new animal
            PurchaseAnimalsMenu PurchaseAnimalsMenu = ActiveClickableMenu.GetMenu() as PurchaseAnimalsMenu;

            if (PurchaseAnimalsMenu == null)
                return;

            PurchaseFarmAnimalMenu PurchaseFarmAnimalMenu = new PurchaseFarmAnimalMenu(this, PurchaseAnimalsMenu);

            PurchaseFarmAnimalMenu.HandleTap(e);
        }

        public void Log(string message, LogLevel logLevel = LogLevel.Debug)
        {
            this.Monitor.Log(message, logLevel);

            if (logLevel.Equals(LogLevel.Trace))
                System.Diagnostics.Trace.WriteLine(message);
            else
                System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
