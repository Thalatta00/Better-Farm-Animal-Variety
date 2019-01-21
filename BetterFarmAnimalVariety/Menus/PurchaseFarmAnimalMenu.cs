using BetterFarmAnimalVariety.Models;
using BetterFarmAnimalVariety.Player;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace BetterFarmAnimalVariety.Menus
{
    class PurchaseFarmAnimalMenu : BaseMenu
    {
        private PurchaseFarmAnimals PurchaseFarmAnimals;
        private PurchaseAnimalsMenu PurchaseAnimalsMenu;

        public PurchaseFarmAnimalMenu(ModEntry mod, PurchaseAnimalsMenu purchaseAnimalsMenu) : base(mod)
        {
            this.PurchaseFarmAnimals = new PurchaseFarmAnimals(mod);
            this.PurchaseAnimalsMenu = purchaseAnimalsMenu;
        }

        private void HandleManualClose()
        {
            (new ActiveClickableMenu(this.Mod)).ExitWithSound("bigDeSelect");
        }

        private bool HasOkButton()
        {
            return this.PurchaseAnimalsMenu.okButton != null;
        }

        private bool TappedOnOkButton(int x, int y)
        {
            return this.HasOkButton() && this.PurchaseAnimalsMenu.okButton.containsPoint(x, y);
        }

        private bool IsReadyToClose()
        {
            return this.PurchaseAnimalsMenu.readyToClose();
        }

        private bool IsOnStockSelectionMenu()
        {
            // Grab from parent menu
            ActiveClickableMenu ActiveClickableMenu = new ActiveClickableMenu(this.Mod);

            bool Freeze = ActiveClickableMenu.GetValue<bool>("freeze");

            if (Game1.globalFade || Freeze)
                return false;

            // Grab from parent menu
            bool IsOnFarm = ActiveClickableMenu.GetValue<bool>("onFarm");

            if (IsOnFarm)
                return false;

            return true;
        }

        public void HandleTap(ButtonPressedEventArgs e)
        {
            if (!this.IsOnStockSelectionMenu())
                return;

            this.SuppressButton(e.Button);

            // Get the clicked screen pixels
            Point Coords = this.GetCursorCoordinates(e);

            // Close menu
            if (this.TappedOnOkButton(Coords.X, Coords.Y) && this.IsReadyToClose())
            {
                this.HandleManualClose();
                return;
            }

            Item Stock = this.DetermineSelectedStock(Coords.X, Coords.Y);

            if (Stock == null)
                return;

            this.HandleStockSelection(Stock);
        }

        private StardewValley.Item DetermineSelectedStock(int x, int y)
        {
            foreach (ClickableTextureComponent StockComponent in this.PurchaseAnimalsMenu.animalsToPurchase)
            {
                if (this.StockContainsTap(StockComponent, x, y))
                    return StockComponent.item;
            }

            return (StardewValley.Item)null;
        }

        private bool StockContainsTap(ClickableTextureComponent stockComponent, int x, int y)
        {
            return stockComponent.containsPoint(x, y) && (stockComponent.item as StardewValley.Object).Type == null;
        }

        private void HandleStockSelection(Item stock)
        {
            if (!this.PurchaseFarmAnimals.CanAfford(stock))
                return;

            // Since we're not propogating the click, make sure we change the relevant parent info
            this.SetupForAfterFade();
            this.PropogateStockSelection(stock);
        }

        private void SetupForAfterFade()
        {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.PurchaseAnimalsMenu.setUpForAnimalPlacement), 0.02f);
            Game1.playSound("smallSelect");
        }

        private void PropogateStockSelection(Item stock)
        {
            ActiveClickableMenu ActiveClickableMenu = new ActiveClickableMenu(this.Mod);

            ActiveClickableMenu.SetValue<bool>("onFarm", true);
            ActiveClickableMenu.SetValue<int>("priceOfAnimal", stock.salePrice());

            // PurchaseAnimalsMenu.cs: public PurchaseAnimalsMenu(List<StardewValley.Object> stock)
            BetterFarmAnimal AnimalBeingPurchased = this.PurchaseFarmAnimals.RandomizeFarmAnimal(new FarmAnimalStock(stock.Name));

            // Update the animalBeingPurchased
            // !!! We have to convert to a base Farm Animal due to exceptions thrown by the day's save XML functions
            ActiveClickableMenu.SetValue<StardewValley.FarmAnimal>("animalBeingPurchased", AnimalBeingPurchased.ToFarmAnimal());
        }

    }
}
