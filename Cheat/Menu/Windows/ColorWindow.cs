using EgguWare.Classes;
using EgguWare.Menu.Tabs;
using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class ColorWindow
    {
        public static bool ColorMenuOpen = false;
        public static void Window(int windowID)
        {
            Color32 c = Colors.GetColor(SettingsTab.SelectedColorIdentifier);
            //T.DrawColor(new Rect(10, 23, 230, 104), new Color32(30, 30, 30, 255));
            //T.DrawColor(new Rect(12, 25, 226, 100), c);

            T.DrawColorLayout(c, new GUILayoutOption[] { GUILayout.Height(104) });

            Color32 cc = new Color32() { a = 255 };
            GUILayout.Label("R: " + c.r);
            cc.r = (byte)GUILayout.HorizontalSlider(c.r, 0, 255);
            GUILayout.Space(2);

            GUILayout.Label("G: " + c.g);
            cc.g = (byte)GUILayout.HorizontalSlider(c.g, 0, 255);
            GUILayout.Space(2);

            GUILayout.Label("B: " + c.b);
            cc.b = (byte)GUILayout.HorizontalSlider(c.b, 0, 255);
            GUILayout.Space(2);

            Colors.SetColor(SettingsTab.SelectedColorIdentifier, cc);

            if (GUILayout.Button("View Colors"))
                Main.SelectedTab = MenuTab.Settings;
            if (GUILayout.Button("Close Window"))
                ColorMenuOpen = !ColorMenuOpen;
            GUI.DragWindow();
        }
    }
}
