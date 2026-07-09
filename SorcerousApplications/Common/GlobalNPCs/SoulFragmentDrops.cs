using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SorcerousApplications.Common.GlobalNPCs
{
	public class SoulFragmentDrops : GlobalNPC
	{
        // Consider adding variants of these mob types to here
		private static readonly Dictionary<int, int> DropChanceDenominators = new()
		{
			[NPCID.Ghost] = 5,
			[NPCID.Skeleton] = 10,
			[NPCID.Zombie] = 50,
		};

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (!DropChanceDenominators.TryGetValue(npc.type, out int chanceDenominator))
				return;

			npcLoot.Add(ItemDropRule.Common(
				ModContent.ItemType<Content.Items.SoulFragment>(),
				chanceDenominator: chanceDenominator));
		}
	}
}
