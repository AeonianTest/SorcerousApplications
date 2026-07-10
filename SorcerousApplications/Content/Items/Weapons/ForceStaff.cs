using Microsoft.Xna.Framework;
using SorcerousApplications.Content.Items;
using SorcerousApplications.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items.Weapons
{
	public class ForceStaff : ModItem
	{
		public const float HoldoutDistance = 34f;

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 40;
			Item.knockBack = 12f;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 10;

			Item.width = 42;
			Item.height = 40;

			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.shoot = ModContent.ProjectileType<ForceStaffHoldProjectile>();
			Item.shootSpeed = 0f;

			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(gold: 3);
			Item.UseSound = SoundID.Item20;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			type = ModContent.ProjectileType<ForceStaffHoldProjectile>();

			if (velocity == Vector2.Zero)
				velocity = Vector2.UnitX;
			velocity = Vector2.Normalize(velocity) * HoldoutDistance;

			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SoulFragment>(), 5)
				.AddIngredient(ItemID.IronBar, 15)
				.AddIngredient(ItemID.Emerald, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}