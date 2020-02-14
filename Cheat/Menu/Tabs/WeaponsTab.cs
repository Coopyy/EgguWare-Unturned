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
            GUILayout.Space(0);
            GUILayout.BeginArea(new Rect(10, 35, 260, 400), style: "box", text: "Modifiers");
            G.Settings.WeaponOptions.NoSpread = GUILayout.Toggle(G.Settings.WeaponOptions.NoSpread, "Remove Spread");
            G.Settings.WeaponOptions.NoRecoil = GUILayout.Toggle(G.Settings.WeaponOptions.NoRecoil, "Remove Recoil");
            G.Settings.WeaponOptions.NoSway = GUILayout.Toggle(G.Settings.WeaponOptions.NoSway, "Remove Sway");
            G.Settings.WeaponOptions.RemoveBurstDelay = GUILayout.Toggle(G.Settings.WeaponOptions.RemoveBurstDelay, "Remove Burst Delay");
            G.Settings.WeaponOptions.RemoveHammerDelay = GUILayout.Toggle(G.Settings.WeaponOptions.RemoveHammerDelay, "Remove Hammer Delay");
            G.Settings.WeaponOptions.InstantReload = GUILayout.Toggle(G.Settings.WeaponOptions.InstantReload, "Remove Reload Delay");
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(280, 35, 260, 400), style: "box", text: "Other");
            G.Settings.WeaponOptions.WeaponInfo = GUILayout.Toggle(G.Settings.WeaponOptions.WeaponInfo, "Show Weapon Info");
            G.Settings.WeaponOptions.DamageIndicators = GUILayout.Toggle(G.Settings.WeaponOptions.DamageIndicators, "Damage Indicators");
            if (G.Settings.WeaponOptions.DamageIndicators)
                G.Settings.WeaponOptions.DamageIndicatorDamageScaling = GUILayout.Toggle(G.Settings.WeaponOptions.DamageIndicatorDamageScaling, "Scale Color By Damage");

            G.Settings.WeaponOptions.HitmarkerBonk = GUILayout.Toggle(G.Settings.WeaponOptions.HitmarkerBonk, "Hitmarker Bonk™️");
            G.Settings.WeaponOptions.TracerLines = GUILayout.Toggle(G.Settings.WeaponOptions.TracerLines, "Bullet Tracers");
            if (G.Settings.WeaponOptions.TracerLines)
            {
                GUILayout.Label("Tracer Expire Time: " + G.Settings.WeaponOptions.TracerTime);
                G.Settings.WeaponOptions.TracerTime = (int)GUILayout.HorizontalSlider(G.Settings.WeaponOptions.TracerTime, 1, 10);
            }
            GUILayout.EndArea();
        }
    }
}
