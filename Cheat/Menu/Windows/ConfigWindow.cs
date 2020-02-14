using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class ConfigWindow
    {
        private static string textfield = "";
        private static Vector2 scrollPosition2 = new Vector2(0, 0);
        public static void Window(int windowID)
        {
            GUILayout.Box($"Current Config: <b>{ConfigUtilities.SelectedConfig}</b>");
            if (GUILayout.Button("Save Current Config"))
                ConfigUtilities.SaveConfig(ConfigUtilities.SelectedConfig);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("New Config:", GUILayout.Width(75));
            textfield = GUILayout.TextField(textfield);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Create Config") && !String.IsNullOrEmpty(textfield))
            {
                ConfigUtilities.SaveConfig(textfield, true);
                textfield = "";
            }
            GUILayout.Space(5);
            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2);
            foreach (string configname in ConfigUtilities.GetConfigs())
            {
                string config = configname;
                if (config == ConfigUtilities.SelectedConfig)
                    config = $"<b>{config}</b>";
                if (GUILayout.Button(config))
                    ConfigUtilities.LoadConfig(config);
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
    }
}
