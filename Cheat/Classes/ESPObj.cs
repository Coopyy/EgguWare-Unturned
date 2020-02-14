using EgguWare.Options.ESP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Classes
{
    public class ESPObj
    {
        public ESPObject Target;
        public object Object;
        public GameObject GObject;
        public ESPOptions Options;

        public ESPObj(ESPObject t, object o, GameObject go, ESPOptions opt)
        {
            Target = t;
            Object = o;
            GObject = go;
            Options = opt;
        }
    }
}
