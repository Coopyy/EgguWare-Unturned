using EgguWare.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Options
{
    public class MiscOptions
    {
        public bool FreeCam = false;
        public bool FullBright = false;
        public bool VehicleNoClip = false;
        public int DayTime = 1200;
        public bool Spam = false;
        public string SpamText = "egguware lol";
        public bool ShowEgguwareUser = true;
        public bool AutoItemPickup = false;
        public string UISkin = "";
        public bool ShowVanishPlayers;
        public ItemWhitelistObject AIPWhitelist = new ItemWhitelistObject();
        public ItemWhitelistObject ESPWhitelist = new ItemWhitelistObject();
        public bool PlayerFlight = false;
        public float PlayerFlightSpeedMult = 1;
        public float RunspeedMult = 5;
        public float JumpMult = 10;
    }
}
