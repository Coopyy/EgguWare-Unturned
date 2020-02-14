using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Menu.Tabs
{
    public class WeaponsTab
    {
        public static void Tab()
        {
            G.Settings.WeaponOptions.NoSpread = GUILayout.Toggle(G.Settings.WeaponOptions.NoSpread, "Remove Spread");
            G.Settings.WeaponOptions.NoRecoil = GUILayout.Toggle(G.Settings.WeaponOptions.NoRecoil, "Remove Recoil");
            G.Settings.WeaponOptions.NoSway = GUILayout.Toggle(G.Settings.WeaponOptions.NoSway, "Remove Sway");
            GUILayout.Space(3);
            G.Settings.WeaponOptions.WeaponInfo = GUILayout.Toggle(G.Settings.WeaponOptions.WeaponInfo, "Show Weapon Info");
            G.Settings.WeaponOptions.DamageIndicators = GUILayout.Toggle(G.Settings.WeaponOptions.DamageIndicators, "Damage Indicators");
            G.Settings.WeaponOptions.TracerLines = GUILayout.Toggle(G.Settings.WeaponOptions.TracerLines, "Bullet Tracers");
            if (G.Settings.WeaponOptions.TracerLines)
            {
                GUILayout.Label("Tracer Expire Time: " + G.Settings.WeaponOptions.TracerTime);
                G.Settings.WeaponOptions.TracerTime = (int)GUILayout.HorizontalSlider(G.Settings.WeaponOptions.TracerTime, 1, 10);
            }
        }
    }
}
