using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.Collections.Generic;

namespace AutoTeamJoin
{
	public class AutoTeamJoin : Mod
	{
	}

	public class AutoTeamJoinlayer : ModPlayer
	{
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

			IDictionary<string, int> teamIDs = new Dictionary<string, int>();
			teamIDs.Add("No Team", 0);
			teamIDs.Add("Red", 1);
			teamIDs.Add("Green", 2);
			teamIDs.Add("Blue", 3);
			teamIDs.Add("Yellow", 4);
			teamIDs.Add("Pink", 5);

			int team = teamIDs[GetInstance<ClientConfig>().TeamName];

			if (Main.player[Main.myPlayer].team != team)
			{
				Main.player[Main.myPlayer].team = team;
				NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Main.myPlayer);
			}
		}
	}
}