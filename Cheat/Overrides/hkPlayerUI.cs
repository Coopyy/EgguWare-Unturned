using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkPlayerUI
    {
        // hitmarker bonk
        public static void OV_hitmark(int index, Vector3 point, bool worldspace, EPlayerHit newHit)
        {
            if (!PlayerUI.window.isEnabled || ((bool)(typeof(PlayerUI).GetField("isOverlayed", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)) && !((bool)(typeof(PlayerUI).GetField("isReverting", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)))))
            {
                return;
            }
            if (index < 0 || index >= PlayerLifeUI.hitmarkers.Length)
            {
                return;
            }
            if (!Provider.modeConfigData.Gameplay.Hitmarkers)
            {
                return;
            }
            HitmarkerInfo hitmarkerInfo = PlayerLifeUI.hitmarkers[index];
            hitmarkerInfo.lastHit = Time.realtimeSinceStartup;
            hitmarkerInfo.hit = newHit;
            hitmarkerInfo.point = point;
            hitmarkerInfo.worldspace = (worldspace || OptionsSettings.hitmarker);
            if (newHit == EPlayerHit.CRITICAL) // if headshot
            {
                if (G.Settings.WeaponOptions.HitmarkerBonk)
                    MainCamera.instance.GetComponent<AudioSource>().PlayOneShot(AssetUtilities.BonkClip, 2 /* volume */);
                else
                    MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/General/Hit"), 0.5f /* volume */);
            }
        }
    }
}
