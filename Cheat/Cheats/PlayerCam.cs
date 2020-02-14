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
    public class PlayerCam : MonoBehaviour
    {
        public static Rect viewport = new Rect((Screen.width - Screen.width / 4) - 10 , 30, Screen.width / 4, Screen.height / 4); //Viewport of the mirror camera
        public static GameObject cam_obj;
        public static Camera subCam;
        public static bool WasEnabled;
        public static bool Enabled = true;
        public static SteamPlayer player = null;
        public static bool IsFullScreen = false;

        public void Update()
        {
            if (!cam_obj || !subCam)
                return;

            if (G.BeingSpied)
                Enabled = false;
            else
                Enabled = true;

            if (Enabled)
                subCam.enabled = true;
            else
                subCam.enabled = false;

        }

        // the ui here looks really stupid someone fix
        void OnGUI()
        {
            GUI.skin = AssetUtilities.Skin;
            if (G.MainCamera == null)
                G.MainCamera = Camera.main;
            if (Enabled && player != null && Provider.isConnected && !G.BeingSpied)
            {
                GUI.color = new Color(1f, 1f, 1f, 0f);
                viewport = GUI.Window(98, viewport, DoMenu, "Player Cam");
                GUI.color = Color.white;
                if (IsFullScreen)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 100, 100, 50), "End Spectating"))
                        IsFullScreen = false;
                }
            }
            if (player == null || G.BeingSpied || !Provider.isConnected && (subCam != null && cam_obj != null))
            {
                Destroy(subCam);
                subCam = null;
                cam_obj = null;
            }
        }

        void DoMenu(int windowID)
        {
            if (cam_obj == null || subCam == null)
            {
                cam_obj = new GameObject();
                if (subCam != null)
                    Destroy(subCam);
                subCam = cam_obj.AddComponent<Camera>();
                subCam.CopyFrom(G.MainCamera);
                cam_obj.AddComponent<GUILayer>();
                subCam.enabled = true;
                subCam.rect = new Rect(0.6f, 0.6f, 0.6f, 0.4f);
                subCam.depth = 98;
                DontDestroyOnLoad(cam_obj);
            }
            subCam.transform.SetPositionAndRotation(T.GetLimbPosition(player.player.transform, "Skull") + new Vector3(0, 0.2f, 0) + (player.player.look.aim.forward / 1.6f), player.player.look.aim.rotation);

            float x, y, w, h;
            x = (viewport.x) / Screen.width;
            y = (viewport.y + 40) / Screen.height;
            w = (viewport.width) / Screen.width;
            h = (viewport.height) / Screen.height;
            if (IsFullScreen)
            {
                x = 0;
                y = 0;
                w = Screen.width;
                h = Screen.height;
            }
            y = 1 - y;
            y -= h;
            subCam.rect = new Rect(x, y, w, h);
            if (!IsFullScreen)
            {
                GUILayout.Space(-15);
                GUILayout.BeginHorizontal();
                GUILayout.Box(player.playerID.characterName, GUILayout.Height(25));
                if (GUILayout.Button("Full-Screen", GUILayout.Height(25)))
                    IsFullScreen = true;
                GUILayout.EndHorizontal();
            }

            GUI.DragWindow();
        }
    }
}
