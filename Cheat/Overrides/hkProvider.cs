using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgguWare.Overrides
{
    public class hkProvider
    {
        public static void OV_OnApplicationQuit(CSteamID steamid)
		{
            Provider.disconnect();
            Process.GetCurrentProcess().Kill();
        }
    }
}
