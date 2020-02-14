using EgguWare.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Utilities
{
    public class Colors
    {
        public static void AddColors()
        {
            foreach (ESPObject val in Enum.GetValues(typeof(ESPObject)))
            {
                string name = Enum.GetName(typeof(ESPObject), val);
                AddColor(name + "_ESP", Color.red);
                AddColor(name + "_Chams_Visible_Color", Color.yellow);
                AddColor(name + "_Chams_Occluded_Color", Color.red);
            }

            AddColor("Friendly_Player_ESP", Color.cyan);
            AddColor("Marked_Player_ESP", new Color32(255, 128, 0, 255));
            AddColor("Chams_Visible_Color", Color.yellow);
            AddColor("Chams_Occluded_Color", Color.red);
            AddColor("Friendly_Chams_Visible_Color", Color.cyan);
            AddColor("Friendly_Chams_Occluded_Color", new Color32(0, 128, 255, 255));
            AddColor("Bullet_Tracer_Color", Color.blue);
            AddColor("Silent_Aim_FOV_Circle", Color.white);
            AddColor("Aimlock_FOV_Circle", Color.white);
            AddColor("Extended_Hitbox_Circle", Color.red);
            AddColor("Item_FOV_Circle", Color.green);
        }

        public static Color32 GetColor(string identifier)
        {
            if (G.Settings.GlobalOptions.GlobalColors.TryGetValue(identifier, out var toret))
                return toret;

            return Color.magenta;
        }

        public static void AddColor(string id, Color32 c)
        {
            if (!G.Settings.GlobalOptions.GlobalColors.ContainsKey(id))
                G.Settings.GlobalOptions.GlobalColors.Add(id, c);
        }

        public static void SetColor(string id, Color32 c) => G.Settings.GlobalOptions.GlobalColors[id] = c;

        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
    }
}
