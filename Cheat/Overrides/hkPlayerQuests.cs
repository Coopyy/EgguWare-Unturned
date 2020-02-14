using System;
using SDG.Unturned;

namespace EgguWare.Overrides
{
	public class hkPlayerQuests
	{
		public bool OV_isMemberOfSameGroupAs(Player player)
		{
			return G.Settings.MiscOptions.AllOnMap && !G.BeingSpied;
		}
	}
}
