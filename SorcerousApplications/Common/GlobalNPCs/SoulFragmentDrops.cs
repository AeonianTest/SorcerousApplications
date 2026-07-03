using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace SorcerousApplications.Common.GlobalNPCs
{
    public class SoulFragmentDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Consider Biome specific drops as well.
            if (npc.type == NPCID.Skeleton || npc.type == NPCID.Ghost)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.SoulFragment>(), chanceDenominator: 10));
            }
        }
    }
}