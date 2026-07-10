using Microsoft.Xna.Framework;
using SorcerousApplications.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Projectiles
{
	public class ForceStaffHoldProjectile : ModProjectile
	{
		private enum StaffPhase
		{
			Charge = 0,
			Fire = 1,
		}

		public const int MaxChargeTime = 60;
		public const float ChargeEffectOffset = 46f;
		public const float MuzzleOffset = 46f;

		// Pixels per tick for the orb — range ≈ OrbSpeed × ForceStaffProjectile.Lifetime
		public const float OrbSpeed = 10f;

		public ref float Timer => ref Projectile.ai[0];

		private StaffPhase CurrentPhase
		{
			get => (StaffPhase)(int)Projectile.ai[1];
			set => Projectile.ai[1] = (int)value;
		}

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.active || player.dead || player.noItems || player.CCed)
			{
				Projectile.Kill();
				return;
			}

			player.heldProj = Projectile.whoAmI;
			UpdateHoldoutPosition(player);
			KeepPlayerArmExtended(player);

			switch (CurrentPhase)
			{
				case StaffPhase.Charge:
					RunChargePhase(player);
					break;
				case StaffPhase.Fire:
					RunFirePhase(player);
					break;
			}
		}

		private void UpdateHoldoutPosition(Player player)
		{
			Vector2 playerCenter = GetPlayerCenter(player);
			Vector2 holdout = GetAimDirection(player) * ForceStaff.HoldoutDistance;

			Projectile.Center = playerCenter + holdout;
			Projectile.velocity = holdout;

			Projectile.direction = holdout.X < 0 ? -1 : 1;
			Projectile.spriteDirection = Projectile.direction;

			// 45° sprite
			Projectile.rotation = holdout.ToRotation()
				+ MathHelper.PiOver2
				- MathHelper.PiOver4 * Projectile.spriteDirection;
		}

		private void KeepPlayerArmExtended(Player player)
		{
			player.ChangeDir(Projectile.spriteDirection);
			player.SetDummyItemTime(2);
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
			Projectile.timeLeft = 2;
		}

		private static Vector2 GetPlayerCenter(Player player)
		{
			return player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
		}

		private static Vector2 GetAimDirection(Player player)
		{
			Vector2 aim = Main.MouseWorld - GetPlayerCenter(player);
			if (aim == Vector2.Zero)
				aim = Vector2.UnitX * player.direction;

			return Vector2.Normalize(aim);
		}

		private static Vector2 GetPointAlongAim(Player player, float distanceFromPlayer)
		{
			return GetPlayerCenter(player) + GetAimDirection(player) * distanceFromPlayer;
		}

		private void RunChargePhase(Player player)
		{
			if (!player.channel)
			{
				Projectile.Kill();
				return;
			}

			Timer++;
			float chargePercent = Timer / MaxChargeTime;

			Vector2 playerCenter = GetPlayerCenter(player);
			Vector2 chargePoint = GetPointAlongAim(player, ChargeEffectOffset);

			float light = 0.4f + chargePercent * 1.2f;
			Lighting.AddLight(chargePoint, 0.8f * light, 2.3f * light, 1.2f * light);

			if (Main.rand.NextBool(3))
			{
				Dust chargeDust = Dust.NewDustDirect(chargePoint, 8, 8, DustID.GreenTorch);
				chargeDust.velocity = (chargePoint - playerCenter) * 0.02f;
				chargeDust.noGravity = true;
				chargeDust.scale = 0.8f + chargePercent;
			}

			if (Timer >= MaxChargeTime)
			{
				CurrentPhase = StaffPhase.Fire;
				Timer = 0f;
			}
		}

		private void RunFirePhase(Player player)
		{
			// Fire once on the local client, then this controller is done
			if (Main.myPlayer != Projectile.owner)
			{
				Projectile.Kill();
				return;
			}

			SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_188"), Projectile.Center);

			Vector2 muzzle = GetPointAlongAim(player, MuzzleOffset);
			Vector2 aim = GetAimDirection(player);

			// Flying projectile velocity = direction × speed
			Vector2 orbVelocity = aim * OrbSpeed;

			Projectile.NewProjectile(
				Projectile.GetSource_FromThis(),
				muzzle,
				orbVelocity,
				ModContent.ProjectileType<ForceStaffProjectile>(),
				Projectile.damage,
				Projectile.knockBack,
				Projectile.owner);

			Projectile.Kill();
		}

		public override bool ShouldUpdatePosition() => false;
	}
}