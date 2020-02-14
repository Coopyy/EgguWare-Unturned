using EgguWare.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Options.ESP
{
    public class OtherOptions
    {
        // Custom Options
        #region Bed
        public bool Claimed = true;
        public bool OnlyUnclaimed = false;
        #endregion
        #region Items
        public bool ListClumpedItems = false;
        public float DistanceThreshhold = 3;
        public int CountThreshhold = 5;
        #endregion
        #region Player
        public bool Weapon = true;
        // public bool Bone;
        #endregion
        #region Vehicle
        public bool VehicleLocked = true;
        public bool OnlyUnlocked = false;
        #endregion
        #region Other
        public Dictionary<string, Color32> GlobalColors = new Dictionary<string, Color32>();
        #endregion
    }
}
