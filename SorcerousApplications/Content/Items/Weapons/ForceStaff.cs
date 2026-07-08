using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items.Weapons
{
    public class ForceStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 100;

            Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(gold: 2);
			Item.UseSound = SoundID.Item20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoulFragment>(), 5)
                .AddIngredient(ItemID.IronBar, 15)
                .AddIngredient(ItemID.Sapphire, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}