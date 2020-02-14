using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class GUIWindow
    {
        private static Vector2 scrollPosition3 = new Vector2(0, 0);
        public static bool GUISkinMenuOpen = false;
        public static void Window(int windowID)
        {
            scrollPosition3 = GUILayout.BeginScrollView(scrollPosition3, style: "SelectedButtonDropdown");
            if (AssetUtilities.GetSkins().Count == 0)
                GUILayout.Label("put gui skins in /Unturned/Unturned_Data/GUISkins/");
            foreach (string skin in AssetUtilities.GetSkins())
            {
                string s = skin;
                if (s == G.Settings.MiscOptions.UISkin)
                    s = $"<b>{s}</b>";
                if (GUILayout.Button(s))
                    AssetUtilities.LoadGUISkinFromName(skin);
            }
            GUILayout.EndScrollView();
            GUILayout.Space(2);
            if (GUILayout.Button("Load Default GUI"))
            {
                G.Settings.MiscOptions.UISkin = "";
                AssetUtilities.Skin = AssetUtilities.VanillaSkin;
            }
            if (GUILayout.Button("Close Window"))
                GUISkinMenuOpen = !GUISkinMenuOpen;
            GUI.DragWindow();
        }
    }
}
