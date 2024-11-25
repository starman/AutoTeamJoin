using static Terraria.ModLoader.ModContent;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader.Config;

namespace AutoTeamJoin
{
    public class AutoTeamJoin : Mod
    {
        internal static void SaveConfig(ModConfig cfg)
        {
            // code from tModLoader discord server posted by Ozzatron
            // in-game ModConfig saving from mod code is not supported yet in tmodloader, and subject to change, so we need to be extra careful.
            // This code only supports client configs, and doesn't call onchanged. It also doesn't support ReloadRequired or anything else.
            MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
            if (saveMethodInfo != null)
                saveMethodInfo.Invoke(null, new object[] { cfg });
            else
                ModContent.GetInstance<AutoTeamJoin>().Logger.Warn("In-game SaveConfig failed, code update required");
        }
    }

    public class AutoTeamJoinPlayer : ModPlayer
    {
        private int lastTeam;

        public override void OnEnterWorld()
        {
            base.OnEnterWorld();

            
            lastTeam = Main.player[Main.myPlayer].team;
        }

        public override void PreUpdate()
        {
            base.PreUpdate();

            IDictionary<int, string> teamNames = new Dictionary<int, string>
            {
                {0, "No Team"},
                {1, "Red"},
                {2, "Green"},
                {3, "Blue"},
                {4, "Yellow"},
                {5, "Pink"}
            };

            ClientConfig config = GetInstance<ClientConfig>();
            int configTeam = GetKeyFromValue(teamNames, config.TeamName);

            if (Main.player[Main.myPlayer].team != lastTeam)
            {
                lastTeam = Main.player[Main.myPlayer].team;

                if (teamNames.TryGetValue(Main.player[Main.myPlayer].team, out string newTeamName))
                {
                    config.TeamName = newTeamName;
                    AutoTeamJoin.SaveConfig(config);
                }

                return;
            }

            if (Main.player[Main.myPlayer].team == configTeam)
                return;

            Main.player[Main.myPlayer].team = configTeam;
            lastTeam = configTeam;
            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Main.myPlayer);
        }

        private int GetKeyFromValue(IDictionary<int, string> dictionary, string value)
        {
            foreach (var pair in dictionary)
            {
                if (pair.Value == value)
                    return pair.Key;
            }
            return 0;
        }
    }
}