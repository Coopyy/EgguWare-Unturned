using EgguWare.Classes;
using EgguWare.Menu.Windows;
using EgguWare.Options.ESP;
using EgguWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Menu.Tabs
{
    public class VisualsTab
    {
        public static ESPObject SelectedObject = ESPObject.Player;
        public static ESPOptions SelectedOptions = G.Settings.PlayerOptions;
        public static void Tab()
        {
            SelectedObject = (ESPObject)GUILayout.Toolbar((int)SelectedObject, Main.buttons2.ToArray());

            switch (SelectedObject)
            {
                case ESPObject.Player:
                    SelectedOptions = G.Settings.PlayerOptions;
                    DrawGlobals(G.Settings.PlayerOptions, "Players", true);
                    G.Settings.GlobalOptions.Weapon = GUILayout.Toggle(G.Settings.GlobalOptions.Weapon, "Show Weapon");
                    //G.Settings.GlobalOptions.Bone = GUILayout.Toggle(G.Settings.GlobalOptions.Bone, "Skeleton ESP"); do this oen later
                    DrawGlobals2(G.Settings.PlayerOptions);
                    break;
                case ESPObject.Storage:
                    SelectedOptions = G.Settings.StorageOptions;
                    DrawGlobals(G.Settings.StorageOptions, "Storages");
                    DrawGlobals2(G.Settings.StorageOptions);
                    break;
                case ESPObject.Vehicle:
                    SelectedOptions = G.Settings.VehicleOptions;
                    DrawGlobals(G.Settings.VehicleOptions, "Vehicles");
                    G.Settings.GlobalOptions.VehicleLocked = GUILayout.Toggle(G.Settings.GlobalOptions.VehicleLocked, "Show Lock State");
                    G.Settings.GlobalOptions.OnlyUnlocked = GUILayout.Toggle(G.Settings.GlobalOptions.OnlyUnlocked, "Only Display Unlocked Vehicles");
                    DrawGlobals2(G.Settings.VehicleOptions);
                    break;
                case ESPObject.Zombie:
                    SelectedOptions = G.Settings.ZombieOptions;
                    DrawGlobals(G.Settings.ZombieOptions, "Zombies");
                    DrawGlobals2(G.Settings.ZombieOptions);
                    break;
                case ESPObject.Bed:
                    SelectedOptions = G.Settings.BedOptions;
                    DrawGlobals(G.Settings.BedOptions, "Beds");
                    G.Settings.GlobalOptions.Claimed = GUILayout.Toggle(G.Settings.GlobalOptions.Claimed, "Show Claimed State");
                    G.Settings.GlobalOptions.OnlyUnclaimed = GUILayout.Toggle(G.Settings.GlobalOptions.OnlyUnclaimed, "Only Display Unclaimed Beds");
                    DrawGlobals2(G.Settings.BedOptions);
                    break;
                case ESPObject.Item:

                    DrawGlobals(G.Settings.ItemOptions, "Items");
                    SelectedOptions = G.Settings.ItemOptions;
                    G.Settings.GlobalOptions.ListClumpedItems = GUILayout.Toggle(G.Settings.GlobalOptions.ListClumpedItems, "List Clumped Items");
                    if (G.Settings.GlobalOptions.ListClumpedItems)
                    {
                        GUILayout.Label("Clump Item Distance Minimum: " + Math.Round(G.Settings.GlobalOptions.DistanceThreshhold, 1).ToString() + "m");
                        G.Settings.GlobalOptions.DistanceThreshhold = GUILayout.HorizontalSlider(G.Settings.GlobalOptions.DistanceThreshhold, 0.1f, 15);
                        GUILayout.Label("Item Count Minimum: " + G.Settings.GlobalOptions.CountThreshhold);
                        G.Settings.GlobalOptions.CountThreshhold = (int)GUILayout.HorizontalSlider(G.Settings.GlobalOptions.CountThreshhold, 2, 10);
                    }
                    if (GUILayout.Button("Open Whitelist Menu"))
                    {
                        Cheats.Items.editingaip = false;
                        WhitelistWindow.WhitelistMenuOpen = true;
                    }
                    DrawGlobals2(G.Settings.ItemOptions);
                    break;
                case ESPObject.Flag:
                    SelectedOptions = G.Settings.FlagOptions;
                    DrawGlobals(G.Settings.FlagOptions, "Claim Flags");
                    DrawGlobals2(G.Settings.FlagOptions);
                    break;
            }
        }

        private static void DrawGlobals(ESPOptions options, string objname, bool isplayer = false)
        {
            GUILayout.Space(2);
            options.Enabled = GUILayout.Toggle(options.Enabled, objname + " - Enabled");
            options.Box = GUILayout.Toggle(options.Box, "Box");
            options.Glow = GUILayout.Toggle(options.Glow, "Glow");
            options.Tracers = GUILayout.Toggle(options.Tracers, "Snaplines");
            options.Name = GUILayout.Toggle(options.Name, "Name");
            options.Distance = GUILayout.Toggle(options.Distance, "Distance");
        }

        private static void DrawGlobals2(ESPOptions options)
        {
            if (GUILayout.Button("Cham Type: " + Enum.GetName(typeof(ShaderType), options.ChamType)))
                options.ChamType = options.ChamType.Next();
            GUILayout.Space(2);
            GUILayout.Label("Max Render Distance: " + options.MaxDistance);
            options.MaxDistance = (int)GUILayout.HorizontalSlider(options.MaxDistance, 0, 3000);
            GUILayout.Space(2);
            GUILayout.Label("Font Size: " + options.FontSize);
            options.FontSize = (int)GUILayout.HorizontalSlider(options.FontSize, 0, 24);
        }
    }
}
