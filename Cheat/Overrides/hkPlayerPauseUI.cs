using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkPlayerPauseUI
    {
        public static void OV_onClickedExitButton(SleekButton button) => Provider.disconnect(); // bypass leave time agan/* volume */
    }
}
