using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Projectiles
{
	public class FociProjectile : ModProjectile
	{
		// Thrust timing (in AI ticks; extraUpdates = 1 means 2 ticks per frame)
		public const int FadeInDuration = 6;
		public const int FadeOutDuration = 3;
		public const int TotalDuration = 14;

		public const int SpriteWidth = 26;
		public const int SpriteHeight = 34;

		public float CollisionWidth => 10f * Projectile.scale;

		public int Timer {
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(18);
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 360;
			Projectile.hide = true; 
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Timer++;
			if (Timer >= TotalDuration)
			{
				Projectile.Kill();
				return;
			}

			player.heldProj = Projectile.whoAmI;

			// Fade in, then fade out near end of thrust
			Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			// Position: player center + thrust direction × how long we've been active
			// shootSpeed from the item becomes Projectile.velocity magnitude/direction
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

			SetVisualOffsets();
		}

		private void SetVisualOffsets()
		{
			int halfSpriteWidth = SpriteWidth / 2;
			int halfSpriteHeight = SpriteHeight / 2;
			int halfProjWidth = Projectile.width / 2;
			int halfProjHeight = Projectile.height / 2;

			DrawOriginOffsetX = 0;
			DrawOffsetX = -(halfSpriteWidth - halfProjWidth);
			DrawOriginOffsetY = -(halfSpriteHeight - halfProjHeight);
		}

		public override bool ShouldUpdatePosition() => false;

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Line hit from handle toward tip — length scales with shootSpeed via velocity
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 6f;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 600);
		}
	}
}
