using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Projectiles
{
	public class FlameBurstProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;

			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			
			Projectile.tileCollide = true;
			Projectile.timeLeft = 60; 
			Projectile.alpha = 80;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.94f;

			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
				dust.noGravity = true;
				dust.scale = 1.1f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 120);
		}
	}
}