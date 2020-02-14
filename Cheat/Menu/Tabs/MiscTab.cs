using EgguWare.Cheats;
using EgguWare.Classes;
using EgguWare.Menu.Windows;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Menu.Tabs
{
    public class MiscTab
    {
        public static MiscOptions SelectedObject = MiscOptions.Game;
        public static void Tab()
        {
            GUILayout.Space(0);
            GUILayout.BeginArea(new Rect(10, 35, 260, 400), style: "box", text: "Game");
            G.Settings.MiscOptions.FreeCam = GUILayout.Toggle(G.Settings.MiscOptions.FreeCam, "Free Cam");
            /*G.Settings.MiscOptions.FullBright = GUILayout.Toggle(G.Settings.MiscOptions.FullBright, "Custom Day Time");
            if (G.Settings.MiscOptions.FullBright)
            {
                GUILayout.Space(2);
                GUILayout.Label("Time: " + G.Settings.MiscOptions.DayTime);
                G.Settings.MiscOptions.DayTime = (int)GUILayout.HorizontalSlider(G.Settings.MiscOptions.DayTime, 0, 3600);
            }*/
            G.Settings.MiscOptions.VehicleNoClip = GUILayout.Toggle(G.Settings.MiscOptions.VehicleNoClip, "Vehicle No-Clip");
            G.Settings.MiscOptions.ShowVanishPlayers = GUILayout.Toggle(G.Settings.MiscOptions.ShowVanishPlayers, "Show Vanished Players");
            G.Settings.MiscOptions.SpamText = GUILayout.TextField(G.Settings.MiscOptions.SpamText);
            G.Settings.MiscOptions.Spam = GUILayout.Toggle(G.Settings.MiscOptions.Spam, "Enable Spam");
            G.Settings.MiscOptions.GrabItemThroughWalls = GUILayout.Toggle(G.Settings.MiscOptions.GrabItemThroughWalls, "Take Through Walls");
            if (G.Settings.MiscOptions.GrabItemThroughWalls)
            {
                G.Settings.MiscOptions.LimitFOV = GUILayout.Toggle(G.Settings.MiscOptions.LimitFOV, "Pixel FOV Limit");
                if (G.Settings.MiscOptions.LimitFOV)
                {
                    GUILayout.Label("Pixels: " + G.Settings.MiscOptions.ItemGrabFOV);
                    G.Settings.MiscOptions.ItemGrabFOV = (int)GUILayout.HorizontalSlider(G.Settings.MiscOptions.ItemGrabFOV, 1, 1200);
                    G.Settings.MiscOptions.DrawFOVCircle = GUILayout.Toggle(G.Settings.MiscOptions.DrawFOVCircle, "Draw Pixel FOV Circle");
                }
            }
            G.Settings.MiscOptions.AutoItemPickup = GUILayout.Toggle(G.Settings.MiscOptions.AutoItemPickup, "Auto Item Pickup");
            if (GUILayout.Button("Open Whitelist Menu"))
            {
                Cheats.Items.editingaip = true;
                WhitelistWindow.WhitelistMenuOpen = true;
            }
            if (GUILayout.Button("GUI Skin Changer"))
                GUIWindow.GUISkinMenuOpen = !GUIWindow.GUISkinMenuOpen;
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(280, 35, 260, 400), style: "box", text: "Movement");
            if (!G.UnrestrictedMovement)
            {
                if (GUILayout.Button("Check Movement Verification"))
                    Misc.instance.StartCoroutine(T.CheckVerification(Player.player.transform.position));
            }
            else
            {
                if (GUILayout.Button("Disable Movement Modifiers"))
                    G.UnrestrictedMovement = false;
                G.Settings.MiscOptions.PlayerFlight = GUILayout.Toggle(G.Settings.MiscOptions.PlayerFlight, "Player No-Clip");
                if (G.Settings.MiscOptions.PlayerFlight)
                {
                    GUILayout.Label("Player Flight Multiplier: " + G.Settings.MiscOptions.PlayerFlightSpeedMult + "x");
                    G.Settings.MiscOptions.PlayerFlightSpeedMult = GUILayout.HorizontalSlider(G.Settings.MiscOptions.PlayerFlightSpeedMult, 0.1f, 100);
                }
                GUILayout.Space(5);
                GUILayout.Label("Walk Speed: " + G.Settings.MiscOptions.RunspeedMult);
                G.Settings.MiscOptions.RunspeedMult = GUILayout.HorizontalSlider(G.Settings.MiscOptions.RunspeedMult, 1, 500);
                GUILayout.Space(5);
                GUILayout.Label("Jump Modifier: " + G.Settings.MiscOptions.JumpMult);
                G.Settings.MiscOptions.JumpMult = GUILayout.HorizontalSlider(G.Settings.MiscOptions.JumpMult, 1, 150);
            }
            GUILayout.EndArea();
        }
    }
}
