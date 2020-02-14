using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using EgguWare.Classes;
using System.Collections;
using EgguWare.Cheats;
using EgguWare.Overrides;
using System.Reflection;
using System.IO;
using System.Threading;

namespace EgguWare.Utilities
{
    public class Manager : MonoBehaviour
    {
        void Start()
        {
            File.WriteAllText("EgguWare.log", "");
            #region Create ESP Line Material
            T.DrawMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = (HideFlags)61
            };
            T.DrawMaterial.SetInt("_SrcBlend", 5);
            T.DrawMaterial.SetInt("_DstBlend", 10);
            T.DrawMaterial.SetInt("_Cull", 0);
            T.DrawMaterial.SetInt("_ZWrite", 0);
            T.Log("Loading EgguWare");
            #endregion
            AttributeUtilities.LinkAttributes();
            T.Log("Adding Attributes");
            ConfigUtilities.CreateEnvironment();
            T.Log("Getting Config");
            AssetUtilities.GetAssets();
            T.Log("Getting Assets");
            Colors.AddColors();
            StartCoroutine(UpdateESPObjects());
            // default settings look like dogshit
            GraphicsSettings.outlineQuality = EGraphicQuality.ULTRA;
            #region Overrides   
            T.Log("Starting Overrides");
            // would be better to use harmony instead but im lazy LOL
            T.OverrideMethod(typeof(DamageTool), typeof(hkDamageTool), "raycast", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //DamageTool
            T.OverrideMethod(typeof(PlayerPauseUI), typeof(hkPlayerPauseUI), "onClickedExitButton", BindingFlags.Static, BindingFlags.NonPublic, BindingFlags.Public, BindingFlags.Static); //PlayerPauseUI
            T.OverrideMethod(typeof(Provider), typeof(hkProvider), "onApplicationWantsToQuit", BindingFlags.Instance, BindingFlags.NonPublic, BindingFlags.Public, BindingFlags.Static); //Provider
            T.OverrideMethod(typeof(Player), typeof(hkPlayer), "askScreenshot", BindingFlags.Public, BindingFlags.Instance, BindingFlags.Public, BindingFlags.Instance); //Player
            T.OverrideMethod(typeof(UseableGun), typeof(hkUsableGun), "ballistics", BindingFlags.NonPublic, BindingFlags.Instance, BindingFlags.Public, BindingFlags.Instance); //UsableGun
            T.OverrideMethod(typeof(ChatManager), typeof(hkChatManager), "receiveChatMessage", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //ChatManager
            T.OverrideMethod(typeof(LocalHwid), typeof(hkLocalHwid), "getHwid", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //LocalHwid
            T.OverrideMethod(typeof(ItemManager), typeof(hkItemManager), "getItemsInRadius", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //ItemManager
            T.OverrideMethod(typeof(Regions), typeof(hkRegions), "getRegionsInRadius", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //Regions
            T.OverrideMethod(typeof(PlayerUI), typeof(hkPlayerUI), "hitmark", BindingFlags.Static, BindingFlags.Public, BindingFlags.Public, BindingFlags.Static); //PlayerUI
            T.OverrideMethod(typeof(PlayerQuests), typeof(hkPlayerQuests), "isMemberOfSameGroupAs", BindingFlags.Public, BindingFlags.Instance, BindingFlags.Public, BindingFlags.Instance); //PlayerQuests
            T.Log("Overrides Complete");

            #endregion
        }
        void OnGUI()
        {
            #region Set Camera Once
            if (G.MainCamera == null)
                G.MainCamera = Camera.main;
            #endregion
        }

        #region Updates ESP Objects
        IEnumerator UpdateESPObjects()
        {
            // every 4 seconds refresh all world objects. DO NOT do this each frame
            while (true)
            {
                if (Provider.isConnected && G.MainCamera != null)
                {
                    List<SteamPlayer> TempPlayers = new List<SteamPlayer>();
                    List<ESPObj> TempObjects = new List<ESPObj>();

                    #region Items
                    if (G.Settings.ItemOptions.Enabled)
                    {
                        foreach (InteractableItem i in FindObjectsOfType<InteractableItem>())
                        {
                            if (!T.IsItemWhitelisted(i, G.Settings.MiscOptions.ESPWhitelist))
                                continue;
                            ESPObj obj = new ESPObj(ESPObject.Item, i, i.gameObject, G.Settings.ItemOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Item_Chams_Visible_Color"), Colors.GetColor("Item_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Claimflags
                    if (G.Settings.FlagOptions.Enabled)
                    {
                        foreach (InteractableClaim i in FindObjectsOfType<InteractableClaim>())
                        {
                            ESPObj obj = new ESPObj(ESPObject.Flag, i, i.gameObject, G.Settings.FlagOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Flag_Chams_Visible_Color"), Colors.GetColor("Flag_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Storages
                    if (G.Settings.StorageOptions.Enabled)
                    {
                        foreach (InteractableStorage i in FindObjectsOfType<InteractableStorage>())
                        {
                            ESPObj obj = new ESPObj(ESPObject.Storage, i, i.gameObject, G.Settings.StorageOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Storage_Chams_Visible_Color"), Colors.GetColor("Storage_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Zombies
                    if (G.Settings.ZombieOptions.Enabled)
                    {
                        foreach (Zombie i in FindObjectsOfType<Zombie>())
                        {
                            ESPObj obj = new ESPObj(ESPObject.Zombie, i, i.gameObject, G.Settings.ZombieOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Zombie_Chams_Visible_Color"), Colors.GetColor("Zombie_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Beds
                    if (G.Settings.BedOptions.Enabled)
                    {
                        foreach (InteractableBed i in FindObjectsOfType<InteractableBed>())
                        {
                            ESPObj obj = new ESPObj(ESPObject.Bed, i, i.gameObject, G.Settings.BedOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Bed_Chams_Visible_Color"), Colors.GetColor("Bed_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Vehicles
                    if (G.Settings.VehicleOptions.Enabled)
                    {
                        foreach (InteractableVehicle i in FindObjectsOfType<InteractableVehicle>())
                        {
                            if (G.Settings.GlobalOptions.OnlyUnlocked && i.isLocked)
                                continue;
                            ESPObj obj = new ESPObj(ESPObject.Vehicle, i, i.gameObject, G.Settings.VehicleOptions);
                            TempObjects.Add(obj);
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, Colors.GetColor("Vehicle_Chams_Visible_Color"), Colors.GetColor("Vehicle_Chams_Occluded_Color"));
                        }
                    }
                    #endregion
                    #region Players
                    foreach (SteamPlayer i in Provider.clients)
                    {
                        if (i != Player.player.channel.owner)
                        {
                            ESPObj obj = new ESPObj(ESPObject.Player, i, i.player.gameObject, G.Settings.PlayerOptions);
                            TempObjects.Add(obj);
                            Color oChams = Colors.GetColor("Player_Chams_Occluded_Color");
                            Color vChams = Colors.GetColor("Player_Chams_Visible_Color");
                            if (T.GetPriority(i.playerID.steamID.m_SteamID) == Priority.Friendly)
                            {
                                oChams = Colors.GetColor("Friendly_Chams_Occluded_Color");
                                vChams = Colors.GetColor("Friendly_Chams_Visible_Color");
                            }
                            if (!G.BeingSpied)
                                ESP.ApplyChams(obj, vChams, oChams);
                            TempPlayers.Add(i);
                        }
                    }
                    #endregion

                    T.ConnectedPlayers = TempPlayers.ToArray();
                    ESP.EObjects = TempObjects;

                }
                yield return new WaitForSeconds(4f);
            }
        }
        #endregion
    }
}
