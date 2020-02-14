using EgguWare.Options.ESP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Options
{
    public class Config
    {
        public ESPOptions BedOptions = new ESPOptions();
        public ESPOptions PlayerOptions = new ESPOptions();
        public ESPOptions ItemOptions = new ESPOptions();
        public ESPOptions StorageOptions = new ESPOptions();
        public ESPOptions VehicleOptions = new ESPOptions();
        public ESPOptions ZombieOptions = new ESPOptions();
        public ESPOptions FlagOptions = new ESPOptions();

        public OtherOptions GlobalOptions = new OtherOptions();

        public AimbotOptions AimbotOptions = new AimbotOptions();
        public WeaponOptions WeaponOptions = new WeaponOptions();
        public MiscOptions MiscOptions = new MiscOptions();

        public Dictionary<ulong, Classes.Priority> Priority = new Dictionary<ulong, Classes.Priority>();
        public Dictionary<ulong, Classes.TargetLimb> TargetLimb = new Dictionary<ulong, Classes.TargetLimb>();
        public Dictionary<ulong, Classes.Mute> Mute = new Dictionary<ulong, Classes.Mute>();
    }
}
