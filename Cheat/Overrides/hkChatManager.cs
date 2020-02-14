using EgguWare.Classes;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkChatManager
    {
        public static void OV_receiveChatMessage(CSteamID speakerSteamID, string iconURL, EChatMode mode, Color color, bool isRich, string text)
        {
            G.Settings.Mute.TryGetValue(speakerSteamID.m_SteamID, out Mute MuteState);
            if (MuteState == Mute.All)
                return;
            if (MuteState == Mute.Global && mode == EChatMode.GLOBAL)
                return;
            if (MuteState == Mute.Area && mode == EChatMode.LOCAL)
                return;
            if (MuteState == Mute.Group && mode == EChatMode.GROUP)
                return;
            text = text.Trim();
            ControlsSettings.formatPluginHotkeysIntoText(ref text);
            if (OptionsSettings.streamer)
            {
                color = Color.white;
            }
            SteamPlayer speaker;
            if (speakerSteamID == CSteamID.Nil)
            {
                speaker = null;
            }
            else
            {
                if (!OptionsSettings.chatText && speakerSteamID != Provider.client)
                {
                    return;
                }
                speaker = PlayerTool.getSteamPlayer(speakerSteamID);
            }
            ReceivedChatMessage item = new ReceivedChatMessage(speaker, iconURL, mode, color, isRich, text);
            ChatManager.receivedChatHistory.Insert(0, item);
            if (ChatManager.receivedChatHistory.Count > Provider.preferenceData.Chat.History_Length)
            {
                ChatManager.receivedChatHistory.RemoveAt(ChatManager.receivedChatHistory.Count - 1);
            }
            if (ChatManager.onChatMessageReceived != null)
            {
                ChatManager.onChatMessageReceived();
            }
        }
    }
}
