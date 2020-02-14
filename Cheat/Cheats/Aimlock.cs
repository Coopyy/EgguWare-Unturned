using EgguWare.Attributes;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Cheats
{
    [Comp]
    public class Aimlock : MonoBehaviour
    {
        public static bool Aiming = false;
        void Update()
        {
            // nothing intersting here
            if (G.Settings.AimbotOptions.Aimlock)
            {
                if (Input.GetKeyDown(G.Settings.AimbotOptions.AimlockKey))
                    Aiming = true;
                if (Input.GetKeyUp(G.Settings.AimbotOptions.AimlockKey))
                    Aiming = false;

                if (Aiming)
                {
                    int? fov = null;
                    if (G.Settings.AimbotOptions.AimlockLimitFOV)
                        fov = G.Settings.AimbotOptions.AimlockFOV;
                    Player player = T.GetNearestPlayer(fov);
                    if (player != null)
                    {
                        Vector3 HeadPos = T.GetLimbPosition(player.transform, "Skull");
                        T.AimAt(HeadPos);
                    }
                }
            }
        }
        void OnGUI()
        {
            if (!G.BeingSpied && Provider.isConnected)
            {
                if (G.Settings.AimbotOptions.AimlockDrawFOV && G.Settings.AimbotOptions.AimlockLimitFOV && G.Settings.AimbotOptions.Aimlock)
                    T.DrawCircle(Colors.GetColor("Aimlock_FOV_Circle"), new Vector2(Screen.width / 2, Screen.height / 2), G.Settings.AimbotOptions.AimlockFOV);
                if (G.Settings.AimbotOptions.SilentAimDrawFOV && G.Settings.AimbotOptions.SilentAim && G.Settings.AimbotOptions.SilentAimLimitFOV)
                    T.DrawCircle(Colors.GetColor("Silent_Aim_FOV_Circle"), new Vector2(Screen.width / 2, Screen.height / 2), G.Settings.AimbotOptions.SilentAimFOV);
            }
        }
    }
}
