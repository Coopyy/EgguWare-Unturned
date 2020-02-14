using EgguWare.Classes;
using EgguWare.Menu.Windows;
using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Menu.Tabs
{
    public class SettingsTab
    {
        public static string SelectedColorIdentifier = "Player_ESP";
        private static Vector2 scrollPosition1 = new Vector2(0, 0);
        public static SettingsOptions SelectedObject = SettingsOptions.Colors;
        public static void Tab()
        {
            SelectedObject = (SettingsOptions)GUILayout.Toolbar((int)SelectedObject, Main.buttons4.ToArray());
            {
                switch (SelectedObject)
                {
                    case SettingsOptions.Colors:
                        if (G.Settings.GlobalOptions.GlobalColors.Count > 0)
                        {
                            scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1/*, GUILayout.Width(480)*/);
                            foreach (KeyValuePair<string, Color32> pair in G.Settings.GlobalOptions.GlobalColors)
                            {
                                string s = pair.Key.Replace("_", " ");
                                s = $"<color=#{Colors.ColorToHex(pair.Value)}>{s}</color>";
                                if (SelectedColorIdentifier == pair.Key)
                                    s = $"<b>{s}</b>";
                                if (GUILayout.Button(s))
                                {
                                    SelectedColorIdentifier = pair.Key;
                                    if (!ColorWindow.ColorMenuOpen)
                                        ColorWindow.ColorMenuOpen = true;
                                }
                            }
                            GUILayout.EndScrollView();
                        }
                        break;
                    case SettingsOptions.Keybinds:
                        GUILayout.Button("Keybinds here soon!");
                        break;
                }
            }
        }
    }
}