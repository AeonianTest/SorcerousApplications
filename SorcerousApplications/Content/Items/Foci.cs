using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items
{
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod good reference
	public class Foci : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.SorcerousApplications.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			
			recipe.AddIngredient(ModContent.ItemType<SoulFragment>(), 5);
			recipe.AddIngredient(ItemID.IronBar, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
