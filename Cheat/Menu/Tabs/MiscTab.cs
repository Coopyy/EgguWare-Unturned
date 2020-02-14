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
        public static bool Roadkillall = false;
        public static void Tab()
        {
            SelectedObject = (MiscOptions)GUILayout.Toolbar((int)SelectedObject, Main.buttons3.ToArray());

            switch (SelectedObject)
            {
                case MiscOptions.Game:
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
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Spam Text:", GUILayout.Width(74));
                    G.Settings.MiscOptions.SpamText = GUILayout.TextField(G.Settings.MiscOptions.SpamText);
                    GUILayout.EndHorizontal();
                    G.Settings.MiscOptions.Spam = GUILayout.Toggle(G.Settings.MiscOptions.Spam, "Enable Spam");
                    GUILayout.Space(5);
                    G.Settings.MiscOptions.AutoItemPickup = GUILayout.Toggle(G.Settings.MiscOptions.AutoItemPickup, "Auto Item Pickup");
                    if (GUILayout.Button("Open Whitelist Menu"))
                    {
                        Cheats.Items.editingaip = true;
                        WhitelistWindow.WhitelistMenuOpen = true;
                    }
                    if (GUILayout.Button("GUI Skin Changer"))
                        GUIWindow.GUISkinMenuOpen = !GUIWindow.GUISkinMenuOpen;
                    GUILayout.Space(5);
                    //G.Settings.MiscOptions.ShowEgguwareUser = GUILayout.Toggle(G.Settings.MiscOptions.ShowEgguwareUser, "Display Self as Egguware User");
                    break;
                case MiscOptions.Unrestricted_Movement:
                    if (!G.UnrestrictedMovement)
                    {
                        if (GUILayout.Button("Check Movement Verification"))
                            Misc.instance.StartCoroutine(T.CheckVerification(Player.player.transform.position));
                    }
                    else
                    {
                        if (GUILayout.Button("Disable Movement Modifiers"))
                            G.UnrestrictedMovement = false;
                        //Roadkillall = GUILayout.Toggle(Roadkillall, "Roadkill Everyone");
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
                    break;
            }
        }
    }
}
