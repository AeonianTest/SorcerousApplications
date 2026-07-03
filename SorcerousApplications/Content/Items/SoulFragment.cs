using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items
{
    public class SoulFragment : ModItem
    {
        
        // Defaults for testing
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(silver: 1);
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }    
}
