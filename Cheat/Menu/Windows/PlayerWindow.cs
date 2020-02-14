using EgguWare.Cheats;
using EgguWare.Classes;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class PlayerWindow
    {
        public static SteamPlayer selectedplayer;
        private static Vector2 scrollPosition1 = new Vector2(0, 0);
        public static void Window(int windowID)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(5));
            GUILayout.Label("Name", GUILayout.Width(174));
            GUILayout.Label("Status", GUILayout.Width(92));
            GUILayout.Label("Target Limb", GUILayout.Width(92));
            GUILayout.Label("Mute Status", GUILayout.Width(92));
            GUILayout.Label("Player Cam", GUILayout.Width(92));
            //GUILayout.Label("EW User", GUILayout.Width(94));
            GUILayout.EndHorizontal();

            scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1/*, GUILayout.Width(480)*/);

            for (var i = 0; i < Provider.clients.Count; i++)
            {
                var player = Provider.clients[i];
                if (player.player == Player.player)
                    continue;

                #region variables
                if (!G.Settings.Priority.ContainsKey(player.playerID.steamID.m_SteamID))
                    G.Settings.Priority.Add(player.playerID.steamID.m_SteamID, Priority.NONE);
                if (!G.Settings.Mute.ContainsKey(player.playerID.steamID.m_SteamID))
                    G.Settings.Mute.Add(player.playerID.steamID.m_SteamID, Mute.NONE);
                if (!G.Settings.TargetLimb.ContainsKey(player.playerID.steamID.m_SteamID))
                    G.Settings.TargetLimb.Add(player.playerID.steamID.m_SteamID, TargetLimb.GLOBAL);

                G.Settings.Priority.TryGetValue(player.playerID.steamID.m_SteamID, out var priority);
                G.Settings.Mute.TryGetValue(player.playerID.steamID.m_SteamID, out var mute);
                G.Settings.TargetLimb.TryGetValue(player.playerID.steamID.m_SteamID, out var targetLimb);
                #endregion

                GUILayout.BeginHorizontal();

                GUILayout.TextField($"{player.playerID.characterName}", GUILayout.Width(180));

                int w = 90;
                string s = Enum.GetName(typeof(Priority), priority);
                if (priority == Priority.FRIENDLY)
                    s = $"<color=#{Colors.ColorToHex(Colors.GetColor("Friendly_Player_ESP"))}>{s}</color>";
                if (priority == Priority.MARKED)
                    s = $"<color=#{Colors.ColorToHex(Colors.GetColor("Marked_Player_ESP"))}>{s}</color>";
                if (GUILayout.Button(s, GUILayout.Width(w)))
                    G.Settings.Priority[player.playerID.steamID.m_SteamID] = priority.Next();

                s = Enum.GetName(typeof(TargetLimb), targetLimb);
                if (targetLimb != TargetLimb.GLOBAL)
                    s = $"<color=red>{s}</color>";
                if (GUILayout.Button(s.Replace("_", " "), GUILayout.Width(w)))
                    G.Settings.TargetLimb[player.playerID.steamID.m_SteamID] = targetLimb.Next();

                s = Enum.GetName(typeof(Mute), mute);
                if (mute != Mute.NONE)
                    s = $"<color=cyan>{s}</color>";
                if (GUILayout.Button(s, GUILayout.Width(w)))
                    G.Settings.Mute[player.playerID.steamID.m_SteamID] = mute.Next();

                bool b = PlayerCam.player == player;
                s = (b).ToString().ToUpper();
                if (b)
                    s = $"<color=cyan>{s}</color>";
                if (GUILayout.Button(s, GUILayout.Width(w)))
                {
                    if (b)
                        PlayerCam.player = null;
                    else
                        PlayerCam.player = player;
                }

                /*b = SocialUtilities.IsEgguwareUser(player.playerID.steamID.m_SteamID.ToString());
                s = (b).ToString().ToUpper();
                if (b)
                    s = $"<color=cyan>{s}</color>";
                GUILayout.Button(s, GUILayout.Width(w));*/

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
    }
}
