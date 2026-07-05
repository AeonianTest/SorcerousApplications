using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items.Weapons
{
    public class FlameStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoulFragment>(), 5)
                .AddIngredient(ItemID.IronBar, 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}