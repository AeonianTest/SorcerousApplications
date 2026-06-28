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
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            var recipe1 = CreateRecipe();
            
            recipe1.AddIngredient(ItemID.Wood, 3);
            recipe1.AddTile(TileID.WorkBenches);
            recipe1.Register();
        }
    }    
}
