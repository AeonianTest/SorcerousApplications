using SorcerousApplications.Content.Projectiles;
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
			Item.mana = 2;

			// Rapier style = thrust animation; the projectile does the actual hit
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.width = 26;
			Item.height = 34;

			Item.knockBack = 2f;
			Item.noUseGraphic = true; 
			Item.noMelee = true;      
			Item.autoReuse = false;

			Item.shoot = ModContent.ProjectileType<FociProjectile>();
			Item.shootSpeed = 2.5f;

			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
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
