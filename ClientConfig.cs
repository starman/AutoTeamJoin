using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AutoTeamJoin
{
	class ClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Your current team")]
		[Tooltip("Use the slider to pick your team")]
		[OptionStrings(new string[] {"No Team", "Red", "Green", "Blue", "Yellow", "Pink"})]
		[DrawTicks]
		[SliderColor(0, 0, 0)]
		[DefaultValue("Red")]
		public string TeamName;

	}
}