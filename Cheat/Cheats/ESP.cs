using EgguWare.Attributes;
using EgguWare.Classes;
using EgguWare.Menu;
using EgguWare.Overrides;
using EgguWare.Utilities;
using HighlightingSystem;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets = EgguWare.Utilities.AssetUtilities;

namespace EgguWare.Cheats
{
    [Comp]
    public class ESP : MonoBehaviour
    {
        public static List<ItemClumpObject> ItemClumps = new List<ItemClumpObject>();
        public static List<ESPObj> EObjects = new List<ESPObj>();
        private Vector2 scroll = new Vector2(0, 0);
        void OnGUI()
        {
            if (!Provider.isConnected || Provider.isLoading || (PlayerCam.IsFullScreen && PlayerCam.player != null))
                return;
            #region Item Clump Boxes
            if (G.Settings.ItemOptions.Enabled && G.Settings.GlobalOptions.ListClumpedItems && !G.BeingSpied)
            {
                for (int i = 0; i < ItemClumps.Count; i++)
                {
                    ItemClumpObject itemClumpObject = ItemClumps[i];

                    Vector3 pos = G.MainCamera.WorldToScreenPoint(itemClumpObject.WorldPos);
                    pos.y = Screen.height - pos.y;
                    if (pos.z >= 0 && (Vector3.Distance(Player.player.transform.position, itemClumpObject.WorldPos) <= G.Settings.ItemOptions.MaxDistance))
                    {
                        string s = "";
                        foreach (InteractableItem item in itemClumpObject.ClumpedItems)
                        {
                            Color c1 = ItemTool.getRarityColorHighlight(item.asset.rarity);
                            Color32 c = new Color32((byte)(c1.r * 255), (byte)(c1.g * 255), (byte)(c1.b * 255), 255);
                            s += $"<color=#{Colors.ColorToHex(c)}>{item.asset.itemName}</color>\n";
                        }
                        Vector2 TextHeight = GUIStyle.none.CalcSize(new GUIContent($"<size=10>{s}</size>"));
                        GUILayout.BeginArea(new Rect(pos.x, pos.y, TextHeight.x + 10, TextHeight.y), style: "box");
                        GUILayout.Label($"<size=10>{s}</size>");
                        GUILayout.EndArea();
                    }
                }
            }
            GUI.skin = null;
            #endregion
            for (int i = 0; i < EObjects.Count; i++)
            {
                ESPObj obj = EObjects[i];
                #region Checks
                if (obj.GObject != null && (!obj.Options.Enabled || T.GetDistance(obj.GObject.transform.position) > obj.Options.MaxDistance || (obj.Target == ESPObject.Item && (!T.IsItemWhitelisted((InteractableItem)obj.Object, G.Settings.MiscOptions.ESPWhitelist) || Items.IsAlreadyClumped((InteractableItem)obj.Object)))))
                {
                    Highlighter h = obj.GObject.GetComponent<Highlighter>();
                    if (h != null)
                        h.ConstantOffImmediate();
                }

                if (obj.GObject == null || !T.InScreenView(G.MainCamera.WorldToViewportPoint(obj.GObject.transform.position)) || !obj.Options.Enabled || T.GetDistance(obj.GObject.transform.position) > obj.Options.MaxDistance)
                    continue;
                if (G.BeingSpied)
                {
                    Highlighter h = obj.GObject.GetComponent<Highlighter>();
                    if (h != null)
                        h.ConstantOffImmediate();
                    T.RemoveShaders(obj.GObject);
                    continue;
                }
                if (obj.Target == ESPObject.Player && ((SteamPlayer)obj.Object).player.life.isDead)
                    continue;
                if (obj.Target == ESPObject.Zombie && ((Zombie)obj.Object).isDead)
                    continue;
                if (obj.Target == ESPObject.Vehicle && ((InteractableVehicle)obj.Object).isDead)
                    continue;
                if (obj.Target == ESPObject.Vehicle && G.Settings.GlobalOptions.OnlyUnlocked && ((InteractableVehicle)obj.Object).isLocked)
                    continue;
                if (obj.Target == ESPObject.Bed && G.Settings.GlobalOptions.OnlyUnclaimed && ((InteractableBed)obj.Object).isClaimed)
                    continue;
                if (obj.Target == ESPObject.Item && !T.IsItemWhitelisted((InteractableItem)obj.Object, G.Settings.MiscOptions.ESPWhitelist))
                    continue;
                if (obj.Target == ESPObject.Item && G.Settings.GlobalOptions.ListClumpedItems && Items.IsAlreadyClumped((InteractableItem)obj.Object))
                    continue;

                if (G.BeingSpied)
                {
                    Highlighter h = obj.GObject.GetComponent<Highlighter>();
                    if (h != null)
                        h.ConstantOffImmediate();
                    T.RemoveShaders(obj.GObject);
                    continue;
                }
                #endregion

                #region Globals
                string LabelText = $"<size={obj.Options.FontSize}>";
                string OutlineText = $"<size={obj.Options.FontSize}>";
                Color32 color = Colors.GetColor(Enum.GetName(typeof(ESPObject), obj.Target) + "_ESP");
                if (obj.Options.Distance)
                {
                    LabelText += $"<color=white>[{T.GetDistance(obj.GObject.transform.position)}]</color> ";
                    OutlineText += $"[{T.GetDistance(obj.GObject.transform.position)}] ";
                }
                #endregion

                #region Label Shit
                switch (obj.Target)
                {
                    case ESPObject.Player:
                        Player player = ((SteamPlayer)obj.Object).player;
                        switch (T.GetPriority(((SteamPlayer)obj.Object).playerID.steamID.m_SteamID))
                        {
                            case Priority.FRIENDLY:
                                color = Colors.GetColor("Friendly_Player_ESP");
                                break;
                            case Priority.MARKED:
                                color = Colors.GetColor("Marked_Player_ESP");
                                break;
                        }

                        if (obj.Options.Name)
                        {
                            LabelText += ((SteamPlayer)obj.Object).playerID.characterName;
                            OutlineText += ((SteamPlayer)obj.Object).playerID.characterName;
                        }
                        if (G.Settings.GlobalOptions.Weapon)
                        {
                            string Weapon = player.equipment.asset != null ? ((SteamPlayer)obj.Object).player.equipment.asset.itemName : "None";
                            LabelText += $"<color=white> - {Weapon}</color>";
                            OutlineText += " - " + Weapon;
                        }
                        break;
                    case ESPObject.Item:
                        if (obj.Options.Name)
                        {
                            LabelText += ((InteractableItem)obj.Object).asset.itemName;
                            OutlineText += ((InteractableItem)obj.Object).asset.itemName;
                        }
                        break;
                    case ESPObject.Vehicle:
                        if (obj.Options.Name)
                        {
                            LabelText += ((InteractableVehicle)obj.Object).asset.vehicleName;
                            OutlineText += ((InteractableVehicle)obj.Object).asset.vehicleName;
                        }
                        if (G.Settings.GlobalOptions.VehicleLocked)
                        {
                            if (((InteractableVehicle)obj.Object).isLocked)
                            {
                                LabelText += $"<color=white> - Locked</color>";
                                OutlineText += " - Locked";
                            }
                            else
                            {
                                LabelText += $"<color=white> - </color><color=ff5a00>Unlocked</color>";
                                OutlineText += " - Unlocked";
                            }
                        }
                        break;
                    case ESPObject.Bed:
                        if (obj.Options.Name)
                        {
                            LabelText += Enum.GetName(typeof(ESPObject), obj.Target);
                            OutlineText += Enum.GetName(typeof(ESPObject), obj.Target);
                        }
                        if (G.Settings.GlobalOptions.Claimed)
                        {
                            if (((InteractableBed)obj.Object).isClaimed)
                            {
                                LabelText += $"<color=white> - Claimed</color>";
                                OutlineText += " - Claimed";
                            }
                            else
                            {
                                LabelText += $"<color=white> - </color><color=ff5a00>Unclaimed</color>";
                                OutlineText += " - Unclaimed";
                            }
                        }
                        break;
                    default:
                        if (obj.Options.Name)
                        {
                            LabelText += Enum.GetName(typeof(ESPObject), obj.Target);
                            OutlineText += Enum.GetName(typeof(ESPObject), obj.Target);
                        }
                        break;
                }
                #endregion

                #region Draw
                LabelText += "</size>";
                OutlineText += "</size>";

                if (obj.Options.Tracers) T.DrawSnapline(obj.GObject.transform.position, color);
                if (!String.IsNullOrEmpty(LabelText))
                    T.DrawESPLabel(obj.GObject.transform.position, color, Color.black, LabelText, OutlineText);
                if (obj.Options.Box)
                {
                    if (obj.Target == ESPObject.Player)
                    {
                        Vector3 p = obj.GObject.transform.position;
                        Vector3 s = obj.GObject.transform.localScale;
                        if (p != null & s != null)
                            T.Draw3DBox(new Bounds(p + new Vector3(0, 1.1f, 0), s + new Vector3(0, .95f, 0)), color);
                    }
                    else
                        T.Draw3DBox(obj.GObject.GetComponent<Collider>().bounds, color);
                }
                if (obj.Options.Glow)
                {
                    Highlighter h = obj.GObject.GetComponent<Highlighter>() ?? obj.GObject.AddComponent<Highlighter>();
                    h.occluder = true;
                    h.overlay = true;
                    h.ConstantOnImmediate(color);
                }
                else
                {
                    Highlighter h = obj.GObject.GetComponent<Highlighter>();
                    if (h != null)
                        h.ConstantOffImmediate();
                }
                #endregion
            }
            // sometimes shows localplayer glowing after your car explodes 
            Highlighter hi = Player.player?.gameObject?.GetComponent<Highlighter>();
            if (hi != null)
                hi.ConstantOffImmediate();
        }

        public static void ApplyChams(ESPObj gameObject, Color vis, Color invis)
        {
            switch (gameObject.Options.ChamType)
            {
                case ShaderType.Flat:
                    T.ApplyShader(Assets.Shaders["Chams"], gameObject.GObject, vis, invis);
                    break;
                case ShaderType.Material:
                    T.ApplyShader(Assets.Shaders["chamsLit"], gameObject.GObject, vis, invis);
                    break;
                default:
                    T.RemoveShaders(gameObject.GObject);
                    break;
            }
        }
    }
}
