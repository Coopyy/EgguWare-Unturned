using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EgguWare.Classes;
using EgguWare.Options;
using UnityEngine;

namespace EgguWare
{
    public class G
    {
        // some global variables
        public static Camera MainCamera;
        public static Config Settings = new Config();
        public static bool BeingSpied = false;
        public static bool UnrestrictedMovement = false;
    }
}
