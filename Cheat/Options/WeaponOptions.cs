using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgguWare.Options
{
    public class WeaponOptions
    {
        public bool NoSpread = false;
        public bool NoRecoil = false;
        public bool NoSway = false;
        public bool WeaponInfo = true;
        public bool TracerLines = true;
        public int TracerTime = 4;
        public bool DamageIndicators = true;
        public bool DamageIndicatorDamageScaling = true;
        public bool HitmarkerBonk = true;

        public bool RemoveHammerDelay = false;
        public bool RemoveBurstDelay = false;
        public bool InstantReload = false;
    }
}
