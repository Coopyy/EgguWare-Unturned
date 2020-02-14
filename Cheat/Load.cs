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
    public static GameObject CheatObject;
    public static void Start()
    {
        CheatObject = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(CheatObject);
        CheatObject.AddComponent<Manager>();
    }
}
