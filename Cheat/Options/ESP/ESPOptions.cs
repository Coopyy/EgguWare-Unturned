using EgguWare.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Options.ESP
{
    // options for each esp object
    public class ESPOptions
    {
        public bool Enabled = false;
        public bool Glow = true;
        public bool Box = true;
        public bool Distance = true;
        public bool Name = true;
        public bool Tracers = true;
        public int MaxDistance = 400;
        public int FontSize = 11;
        public ShaderType ChamType = ShaderType.Material;
    }
}
