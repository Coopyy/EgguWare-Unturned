using EgguWare.Classes;
using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Menu.Tabs
{
    public class AimbotTab
    {
        public static AimbotOptions SelectedObject = AimbotOptions.Silent;
        public static void Tab()
        {
            SelectedObject = (AimbotOptions)GUILayout.Toolbar((int)SelectedObject, Main.buttons5.ToArray());
            switch (SelectedObject)
            {
                case AimbotOptions.Silent:
                    G.Settings.AimbotOptions.SilentAim = GUILayout.Toggle(G.Settings.AimbotOptions.SilentAim, "Silent Aim");
                    if (G.Settings.AimbotOptions.SilentAim)
                    {
                        //G.Settings.AimbotOptions.SilentAimInfo = GUILayout.Toggle(G.Settings.AimbotOptions.SilentAimInfo, "Show Vulnerable Targets");
                        G.Settings.AimbotOptions.ExpandHitboxes = GUILayout.Toggle(G.Settings.AimbotOptions.ExpandHitboxes, "Expand Hitboxes");
                        if (G.Settings.AimbotOptions.ExpandHitboxes)
                        {
                            GUILayout.Label("Aimpoint Multiplier: " + G.Settings.AimbotOptions.AimpointMultiplier);
                            G.Settings.AimbotOptions.AimpointMultiplier = (int)GUILayout.HorizontalSlider(G.Settings.AimbotOptions.AimpointMultiplier, 1, 3);
                            GUILayout.Space(2);

                            GUILayout.Label("Hitbox Width: " + G.Settings.AimbotOptions.HitboxSize + "m");
                            G.Settings.AimbotOptions.HitboxSize = (int)GUILayout.HorizontalSlider(G.Settings.AimbotOptions.HitboxSize, 1, 15);
                            GUILayout.Space(2);
                        }

                        GUILayout.Label("Chance To Hit: " + G.Settings.AimbotOptions.HitChance + "%");
                        G.Settings.AimbotOptions.HitChance = (int)GUILayout.HorizontalSlider(G.Settings.AimbotOptions.HitChance, 1, 100);
                        GUILayout.Space(2);

                        if (GUILayout.Button("Silent Aim Limb: " + Enum.GetName(typeof(TargetLimb1), G.Settings.AimbotOptions.TargetL)))
                            G.Settings.AimbotOptions.TargetL = G.Settings.AimbotOptions.TargetL.Next();

                        G.Settings.AimbotOptions.SilentAimLimitFOV = GUILayout.Toggle(G.Settings.AimbotOptions.SilentAimLimitFOV, "Pixel FOV Limit");
                        if (G.Settings.AimbotOptions.SilentAimLimitFOV)
                        {
                            GUILayout.Label("Pixels: " + G.Settings.AimbotOptions.SilentAimFOV);
                            G.Settings.AimbotOptions.SilentAimFOV = (int)GUILayout.HorizontalSlider(G.Settings.AimbotOptions.SilentAimFOV, 1, 1200);
                            G.Settings.AimbotOptions.SilentAimDrawFOV = GUILayout.Toggle(G.Settings.AimbotOptions.SilentAimDrawFOV, "Draw Pixel FOV Circle");
                        }
                    }
                    break;
                case AimbotOptions.Aimlock:
                    G.Settings.AimbotOptions.Aimlock = GUILayout.Toggle(G.Settings.AimbotOptions.Aimlock, "Aimlock");
                    if (G.Settings.AimbotOptions.Aimlock)
                    {
                        if (GUILayout.Button("Aimlock Key: " + G.Settings.AimbotOptions.AimlockKey.ToString()))
                            G.Settings.AimbotOptions.AimlockKey = KeyCode.None;

                        //G.Settings.AimbotOptions.OnlyVisible = GUILayout.Toggle(G.Settings.AimbotOptions.OnlyVisible, "Only Aim At Visible Targets");
                        G.Settings.AimbotOptions.AimlockLimitFOV = GUILayout.Toggle(G.Settings.AimbotOptions.AimlockLimitFOV, "Pixel FOV Limit");
                        if (G.Settings.AimbotOptions.AimlockLimitFOV)
                        {
                            GUILayout.Label("Pixels: " + G.Settings.AimbotOptions.AimlockFOV);
                            G.Settings.AimbotOptions.AimlockFOV = (int)GUILayout.HorizontalSlider(G.Settings.AimbotOptions.AimlockFOV, 1, 1200);
                            G.Settings.AimbotOptions.AimlockDrawFOV = GUILayout.Toggle(G.Settings.AimbotOptions.AimlockDrawFOV, "Draw Pixel FOV Circle");
                        }

                    }
                    break;

            }
            if (G.Settings.AimbotOptions.AimlockKey == KeyCode.None)
            {
                Event e = Event.current;
                G.Settings.AimbotOptions.AimlockKey = e.keyCode;
            }
        }
    }
}
