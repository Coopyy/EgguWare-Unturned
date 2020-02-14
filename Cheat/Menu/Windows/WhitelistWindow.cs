using EgguWare.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Menu.Windows
{
    public class WhitelistWindow
    {
        public static bool WhitelistMenuOpen = false;
        public static void Window(int windowID)
        {
            ItemWhitelistObject whitelistToEdit = Cheats.Items.editingaip ? G.Settings.MiscOptions.AIPWhitelist : G.Settings.MiscOptions.ESPWhitelist;
            string s = Cheats.Items.editingaip ? "Pickup" : "Display";

            whitelistToEdit.filterItems = GUILayout.Toggle(whitelistToEdit.filterItems, "Filter Item Whitelist");
            if (whitelistToEdit.filterItems)
            {
                GUILayout.Space(3);
                GUILayout.BeginVertical(style: "SelectedButtonDropdown");
                whitelistToEdit.allowGun = GUILayout.Toggle(whitelistToEdit.allowGun, s + " Guns");
                whitelistToEdit.allowMelee = GUILayout.Toggle(whitelistToEdit.allowMelee, s + " Melees");
                whitelistToEdit.allowBackpack = GUILayout.Toggle(whitelistToEdit.allowBackpack, s + " Backpacks");
                whitelistToEdit.allowClothing = GUILayout.Toggle(whitelistToEdit.allowClothing, s + " Clothing");
                whitelistToEdit.allowFuel = GUILayout.Toggle(whitelistToEdit.allowFuel, s + " Fuel");
                whitelistToEdit.allowFoodWater = GUILayout.Toggle(whitelistToEdit.allowFoodWater, s + " Food/Water");
                whitelistToEdit.allowAmmo = GUILayout.Toggle(whitelistToEdit.allowAmmo, s + " Ammo");
                whitelistToEdit.allowMedical = GUILayout.Toggle(whitelistToEdit.allowMedical, s + " Medical");
                whitelistToEdit.allowThrowable = GUILayout.Toggle(whitelistToEdit.allowThrowable, s + " Throwables");
                whitelistToEdit.allowAttachments = GUILayout.Toggle(whitelistToEdit.allowAttachments, s + " Attachments");
                GUILayout.EndVertical();
            }
            if (GUILayout.Button("Close Window"))
                WhitelistMenuOpen = !WhitelistMenuOpen;
            GUI.DragWindow();
        }
    }
}
