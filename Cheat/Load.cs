using EgguWare;
using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Load
{
    public static GameObject CO;
    public static void Start()
    {
        //create new gameobject
        CO = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(CO);
        //let manager use the unity functions
        CO.AddComponent<Manager>();
    }
}
