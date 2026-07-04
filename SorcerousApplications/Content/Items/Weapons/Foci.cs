using SorcerousApplications.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items.Weapons
{
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod good reference
	public class Foci : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 6;
			
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
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
