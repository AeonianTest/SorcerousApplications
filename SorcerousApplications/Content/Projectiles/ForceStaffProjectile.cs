using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Content.Projectiles
{
	public class ForceStaffProjectile : ModProjectile
	{
		public const int Lifetime = 180;

		public override void SetDefaults()
		{
			Projectile.width = 35;
			Projectile.height = 35;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;

			// -1 = pierce unlimited enemies; tileCollide false = pass through blocks
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Projectile.timeLeft = Lifetime;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Projectile.rotation += 0.15f;

			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
				dust.noGravity = true;
				dust.scale = 1.1f;
			}
		}
	}
}