using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class DropdownWindow
    {
        public static bool DropdownOpen = false;
        public static System.Action DropdownAction;
        public static void Window(int windowID)
        {
            try { DropdownAction(); }
            catch (Exception e) { T.Log(e.ToString()); }
            GUILayout.Space(3);
            if (GUILayout.Button("Close"))
                DropdownOpen = false;
        }
    }
}
