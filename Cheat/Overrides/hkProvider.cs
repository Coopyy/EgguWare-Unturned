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
        public static bool OV_onApplicationWantsToQuit() //bypasses nolsons leave timer
		{
            return true;
        }
    }
}
