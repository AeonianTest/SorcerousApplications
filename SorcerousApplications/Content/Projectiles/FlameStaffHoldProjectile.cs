using Microsoft.Xna.Framework;
using SorcerousApplications.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Projectiles
{
	public class FlameStaffHoldProjectile : ModProjectile
	{
		private enum StaffPhase
		{
			Charge = 0,
			Burst = 1,
		}

		public const int MaxChargeTime = 60;
		public const int MaxBurstTime = 60;
		public const int FlamesEveryNTicks = 2;

		public const float ConeSpreadRadians = 0.1f;
		public const int FlamesPerWave = 4;
		public const float FlameSpeed = 30f;

		// Distance from player center along aim
		public const float ChargeEffectOffset = 46f;
		public const float MuzzleOffset = 46f;

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
			Projectile.friendly = false; // this projectile does not deal damage, flame burst projectile does
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
				case StaffPhase.Burst:
					RunBurstPhase(player);
					break;
			}
		}

		private void UpdateHoldoutPosition(Player player)
		{
			Vector2 playerCenter = GetPlayerCenter(player);
			Vector2 holdout = GetAimDirection(player) * FlameStaff.HoldoutDistance;

			Projectile.Center = playerCenter + holdout;
			Projectile.velocity = holdout;
			Projectile.rotation = holdout.ToRotation() + MathHelper.PiOver2;
			Projectile.spriteDirection = holdout.X < 0 ? -1 : 1;
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
			Lighting.AddLight(chargePoint, 0.1f * light, 0.25f * light, 1f * light);

			if (Main.rand.NextBool(3))
			{
				Dust chargeDust = Dust.NewDustDirect(chargePoint, 8, 8, DustID.BlueCrystalShard);
				chargeDust.velocity = (chargePoint - playerCenter) * 0.02f;
				chargeDust.noGravity = true;
				chargeDust.scale = 0.8f + chargePercent;
			}

			if (Timer >= MaxChargeTime)
			{
				CurrentPhase = StaffPhase.Burst;
				Timer = 0f;
			}
		}

		private void RunBurstPhase(Player player)
		{
			Timer++;

			Vector2 muzzle = GetPointAlongAim(player, MuzzleOffset);
			Lighting.AddLight(muzzle, 0.4f, 1.8f, 2.4f);

			if (Main.myPlayer == Projectile.owner && Timer % FlamesEveryNTicks == 0)
				SpawnFlameCone(player, muzzle);

			if (Timer >= MaxBurstTime)
				Projectile.Kill();
		}

		private void SpawnFlameCone(Player player, Vector2 muzzle)
		{
			Vector2 aim = GetAimDirection(player);
			var source = Projectile.GetSource_FromThis();

			for (int i = 0; i < FlamesPerWave; i++)
			{
				Vector2 velocity = aim.RotatedByRandom(ConeSpreadRadians * 0.5f);
				velocity *= Main.rand.NextFloat(0.85f, 1.15f) * FlameSpeed;

				Projectile.NewProjectile(
					source,
					muzzle + Main.rand.NextVector2Circular(4f, 4f),
					velocity,
					ModContent.ProjectileType<FlameBurstProjectile>(),
					Projectile.damage,
					Projectile.knockBack,
					Projectile.owner);
			}
		}

		public override bool ShouldUpdatePosition() => false;
	}
}