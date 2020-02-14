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
        public static string SelectedColorIdentifier = "";
        private static Vector2 scrollPosition1 = new Vector2(0, 0);
        private static string textfield = "New Config";
        private static Vector2 scrollPosition2 = new Vector2(0, 0);

        public static void Tab()
        {
            GUILayout.Space(0);
            GUILayout.BeginArea(new Rect(10, 35, 260, 400), style: "box", text: "Colors");
            if (G.Settings.GlobalOptions.GlobalColors.Count > 0)
            {
                scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1/*, GUILayout.Width(480)*/);

                List<string> keys = new List<string>(G.Settings.GlobalOptions.GlobalColors.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    Color32 color = Colors.GetColor(key);
                    string s = $"<color=#{Colors.ColorToHex(color)}>{key.Replace("_", " ")}</color>";

                    if (SelectedColorIdentifier == key)
                    {
                        if (GUILayout.Button(s, style: "SelectedButton"))
                            SelectedColorIdentifier = "";
                        GUILayout.BeginVertical(style: "SelectedButtonDropdown");
                        Color32 c = color;
                        Color32 cc = new Color32() { a = 255 };
                        GUILayout.Label("R: " + c.r);
                        cc.r = (byte)GUILayout.HorizontalSlider(c.r, 0, 255);
                        GUILayout.Space(2);

                        GUILayout.Label("G: " + c.g);
                        cc.g = (byte)GUILayout.HorizontalSlider(c.g, 0, 255);
                        GUILayout.Space(2);

                        GUILayout.Label("B: " + c.b);
                        cc.b = (byte)GUILayout.HorizontalSlider(c.b, 0, 255);
                        G.Settings.GlobalOptions.GlobalColors[key] = cc;
                        GUILayout.EndVertical();
                    }
                    else if (GUILayout.Button(s))
                        SelectedColorIdentifier = key;
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(280, 35, 260, 400), style: "box", text: $"Config: <b>{ConfigUtilities.SelectedConfig}</b>");

            textfield = GUILayout.TextField(textfield);
            if (GUILayout.Button("Create Config") && !String.IsNullOrEmpty(textfield))
            {
                ConfigUtilities.SaveConfig(textfield, true);
                textfield = "";
            }
            if (GUILayout.Button("Save Current Config"))
                ConfigUtilities.SaveConfig(ConfigUtilities.SelectedConfig);

            GUILayout.Space(5);
            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, style: "SelectedButtonDropdown");
            foreach (string configname in ConfigUtilities.GetConfigs())
            {
                string config = configname;
                if (config == ConfigUtilities.SelectedConfig)
                    config = $"<b>{config}</b>";
                if (GUILayout.Button(config))
                    ConfigUtilities.LoadConfig(config);
            }
            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }
    }
}