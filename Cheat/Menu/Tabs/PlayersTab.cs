using EgguWare.Cheats;
using EgguWare.Classes;
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
    public class PlayersTab
    {
        public static SteamPlayer selectedplayer = null;
        private static Vector2 scrollPosition1 = new Vector2(0, 0);
        public static void Tab()
        {
            GUILayout.Space(0);
            GUILayout.BeginArea(new Rect(10, 35, 530, 400), style: "box", text: "Online Players");
            scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1/*, GUILayout.Width(480)*/);
            for (var i = 0; i < Provider.clients.Count; i++)
            {
                var player = Provider.clients[i];
                if (player.player == Player.player)
                    continue;

                if (selectedplayer == player)
                {
                    #region variables
                    if (!G.Settings.Priority.ContainsKey(player.playerID.steamID.m_SteamID))
                        G.Settings.Priority.Add(player.playerID.steamID.m_SteamID, Priority.None);
                    if (!G.Settings.Mute.ContainsKey(player.playerID.steamID.m_SteamID))
                        G.Settings.Mute.Add(player.playerID.steamID.m_SteamID, Mute.None);
                    if (!G.Settings.TargetLimb.ContainsKey(player.playerID.steamID.m_SteamID))
                        G.Settings.TargetLimb.Add(player.playerID.steamID.m_SteamID, TargetLimb.GLOBAL);

                    G.Settings.Priority.TryGetValue(player.playerID.steamID.m_SteamID, out var priority);
                    G.Settings.Mute.TryGetValue(player.playerID.steamID.m_SteamID, out var mute);
                    G.Settings.TargetLimb.TryGetValue(player.playerID.steamID.m_SteamID, out var targetLimb);
                    #endregion

                    string s = "";
                    if (priority == Priority.Friendly)
                        s += $"<color=#{Colors.ColorToHex(Colors.GetColor("Friendly_Player_ESP"))}>[FRIENDLY] </color>";
                    if (priority == Priority.Marked)
                        s += $"<color=#{Colors.ColorToHex(Colors.GetColor("Marked_Player_ESP"))}>[MARKED] </color>";
                    if (mute != Mute.None)
                        s += $"<color=cyan>[MUTED] </color>";
                    if (targetLimb != TargetLimb.GLOBAL)
                        s += $"<color=red>[LIMB] </color>";
                    if (PlayerCam.player == player)
                        s += "<color=cyan>[CAM] </color>";

                    if (GUILayout.Button(s + player.playerID.characterName, style: "SelectedButton"))
                        selectedplayer = null;
                    GUILayout.BeginVertical(style: "SelectedButtonDropdown");
                    GUILayout.TextField(player.playerID.steamID.m_SteamID.ToString(), style: "label");

                    if (GUILayout.Button("Limb: " + Enum.GetName(typeof(TargetLimb), targetLimb)))
                        G.Settings.TargetLimb[player.playerID.steamID.m_SteamID] = targetLimb.Next();
                    if (GUILayout.Button("Mute: " + Enum.GetName(typeof(Mute), mute)))
                        G.Settings.Mute[player.playerID.steamID.m_SteamID] = mute.Next();
                    if (GUILayout.Button("Status: " + Enum.GetName(typeof(Priority), priority)))
                        G.Settings.Priority[player.playerID.steamID.m_SteamID] = priority.Next();

                    if (GUILayout.Button("Camera"))
                    {
                        if (PlayerCam.player == player)
                            PlayerCam.player = null;
                        else
                            PlayerCam.player = player;
                    }
                    GUILayout.Button("Add Record");
                    GUILayout.EndVertical();
                }
                else
                {
                    if (GUILayout.Button("" + player.playerID.characterName))
                        selectedplayer = player;
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}
