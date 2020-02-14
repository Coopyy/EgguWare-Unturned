using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkPlayer : MonoBehaviour
    {
		public static float LastSpy;

        public void OV_askScreenshot(CSteamID steamid)
        {
            if (Player.player.channel.checkServer(steamid))
                StartCoroutine(takeScreenshot());
        }

        private IEnumerator takeScreenshot()
        {
            if (Time.realtimeSinceStartup - LastSpy < 0.5 || G.BeingSpied)
                yield break;

            G.BeingSpied = true;

            LastSpy = Time.realtimeSinceStartup;

            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();

            Texture2D screenshotRaw =
                new Texture2D(Screen.width, Screen.height, (TextureFormat)3, false)
                {
                    name = "Screenshot_Raw",
                    hideFlags = (HideFlags)61
                };

            screenshotRaw.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);

            Texture2D screenshotFinal = new Texture2D(640, 480, (TextureFormat)3, false)
            {
                name = "Screenshot_Final",
                hideFlags = (HideFlags)61
            };

            Color[] oldColors = screenshotRaw.GetPixels();
            Color[] newColors = new Color[screenshotFinal.width * screenshotFinal.height];
            float widthRatio = screenshotRaw.width / (float)screenshotFinal.width;
            float heightRatio = screenshotRaw.height / (float)screenshotFinal.height;

            for (int i = 0; i < screenshotFinal.height; i++)
            {
                int num = (int)(i * heightRatio) * screenshotRaw.width;
                int num2 = i * screenshotFinal.width;
                for (int j = 0; j < screenshotFinal.width; j++)
                {
                    int num3 = (int)(j * widthRatio);
                    newColors[num2 + j] = oldColors[num + num3];
                }
            }

            screenshotFinal.SetPixels(newColors);
            byte[] data = screenshotFinal.EncodeToJPG(33);

            if (data.Length < 30000)
            {
                Player.player.channel.longBinaryData = true;
                Player.player.channel.openWrite();
                Player.player.channel.write(data);
                Player.player.channel.closeWrite("tellScreenshotRelay", ESteamCall.SERVER,
                    ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                Player.player.channel.longBinaryData = false;
            }

            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            G.BeingSpied = false;
        }
    }
}
