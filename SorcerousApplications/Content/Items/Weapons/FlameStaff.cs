using Microsoft.Xna.Framework;
using SorcerousApplications.Content.Items;
using SorcerousApplications.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Items.Weapons
{
	public class FlameStaff : ModItem
	{
		// How far the held staff sprite sits from the player (passed to hold projectile via velocity)
		public const float HoldoutDistance = 34f;

		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 70;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 10;

			Item.width = 42;
			Item.height = 40;

			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.shoot = ModContent.ProjectileType<FlameStaffHoldProjectile>();
			Item.shootSpeed = 0f;

			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(gold: 2);
			Item.UseSound = SoundID.Item20;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			type = ModContent.ProjectileType<FlameStaffHoldProjectile>();

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
				.AddIngredient(ItemID.Sapphire, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}