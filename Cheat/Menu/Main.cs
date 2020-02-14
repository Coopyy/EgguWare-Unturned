using EgguWare.Attributes;
using EgguWare.Classes;
using EgguWare.Menu.Tabs;
using EgguWare.Utilities;
using SDG.Unturned;
using EgguWare.Cheats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using AssetUtilities = EgguWare.Utilities.AssetUtilities;
using EgguWare.Menu.Windows;

namespace EgguWare.Menu
{
    [Comp]
    public class Main : MonoBehaviour
    {
        public static MenuTab SelectedTab = MenuTab.Visuals;
        public static bool MenuOpen = false;
        public static string DropdownTitle;
        public static Rect DropdownPos;
        public static Color GUIColor;
        private List<GUIContent> buttons = new List<GUIContent>();
        public static List<GUIContent> buttons2 = new List<GUIContent>();
        public static List<GUIContent> buttons3 = new List<GUIContent>();
        public static List<GUIContent> buttons4 = new List<GUIContent>();
        public static List<GUIContent> buttons5 = new List<GUIContent>();
        public static Rect CursorPos = new Rect(0, 0, 20f, 20f);

        private Texture _cursorTexture;
        private Rect windowRect;
        private Rect colorRect;
        private Rect playerRect;
        private Rect itemRect;
        private Rect configRect;
        private Rect guiRect;

        readonly string Name = "EgguWare";
        readonly string Version = "v0.9.8";

        void ResetWindowPos()
        {
            windowRect = new Rect(20, 20, 525, 425);
            colorRect = new Rect(20, 465, 250, 300);
            playerRect = new Rect(565, 20, 500, 350);
            itemRect = new Rect(565, 465, 200, 250);
            configRect = new Rect(1285, 20, 275, 315);
            guiRect = new Rect(1285, 355, 200, 250);
        }

        void Start()
        {
            GUIColor = GUI.color;
            ResetWindowPos();
            foreach (MenuTab val in Enum.GetValues(typeof(MenuTab)))
                buttons.Add(new GUIContent(Enum.GetName(typeof(MenuTab), val)));
            foreach (ESPObject val in Enum.GetValues(typeof(ESPObject)))
                buttons2.Add(new GUIContent(Enum.GetName(typeof(ESPObject), val)));
            foreach (MiscOptions val in Enum.GetValues(typeof(MiscOptions)))
                buttons3.Add(new GUIContent(Enum.GetName(typeof(MiscOptions), val).Replace("_", " ")));
            foreach (SettingsOptions val in Enum.GetValues(typeof(SettingsOptions)))
                buttons4.Add(new GUIContent(Enum.GetName(typeof(SettingsOptions), val)));
            foreach (AimbotOptions val in Enum.GetValues(typeof(AimbotOptions)))
                buttons5.Add(new GUIContent(Enum.GetName(typeof(AimbotOptions), val)));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                MenuOpen = !MenuOpen;
        }
        void OnGUI()
        {
            if (!G.BeingSpied)
            {
                if (!Provider.isConnected)
                    T.DrawOutlineLabel(new Vector2(125, 0), new Color32(34, 177, 76, 255), Color.black, $"<b>{Name} {Version}</b>");
                if (MenuOpen)
                {
                    GUI.skin = AssetUtilities.Skin;

                    if (_cursorTexture == null)
                        _cursorTexture = Resources.Load("UI/Cursor") as Texture;

                    GUI.depth = -1;
                    windowRect = GUILayout.Window(0, windowRect, MenuWindow, $"{Name} {Version}");
                    playerRect = GUILayout.Window(1, playerRect, PlayerWindow.Window, "Players");
                    configRect = GUILayout.Window(3, configRect, ConfigWindow.Window, "Configs");
                    if (ColorWindow.ColorMenuOpen)
                        colorRect = GUILayout.Window(2, colorRect, ColorWindow.Window, SettingsTab.SelectedColorIdentifier.Replace("_", " "));
                    if (WhitelistWindow.WhitelistMenuOpen)
                        itemRect = GUILayout.Window(4, itemRect, WhitelistWindow.Window, (Cheats.Items.editingaip ? "Pickup " : "ESP ") + "Whitelist");
                    if (GUIWindow.GUISkinMenuOpen)
                        guiRect = GUILayout.Window(5, guiRect, GUIWindow.Window, "GUI Skins");
                    if (DropdownWindow.DropdownOpen)
                        GUILayout.Window(9, DropdownPos, DropdownWindow.Window, DropdownTitle);
                    if (GUI.Button(new Rect(10, Screen.height - 50, 150, 50), "Reset Menu Positions"))
                        ResetWindowPos();

                    GUI.depth = -2;
                    CursorPos.x = Input.mousePosition.x;
                    CursorPos.y = Screen.height - Input.mousePosition.y;

                    GUI.DrawTexture(CursorPos, _cursorTexture);
                    Cursor.lockState = CursorLockMode.None;

                    if (PlayerUI.window != null)
                        PlayerUI.window.showCursor = true;

                    GUI.skin = null;
                }
            }
        }

        void MenuWindow(int windowID)
        {
            SelectedTab = (MenuTab)GUILayout.Toolbar((int)SelectedTab, buttons.ToArray());
            #region Display Selected Tab
            switch (SelectedTab)
            {
                case MenuTab.Visuals:
                    VisualsTab.Tab();
                    break;
                case MenuTab.Aimbot:
                    AimbotTab.Tab();
                    break;
                case MenuTab.Misc:
                    MiscTab.Tab();
                    break;
                case MenuTab.Weapons:
                    WeaponsTab.Tab();
                    break;
                case MenuTab.Settings:
                    SettingsTab.Tab();
                    break;
            }
            #endregion

            GUI.DragWindow();
        }
    }
}
