using EgguWare.Classes;
using EgguWare.Menu;
using EgguWare.Menu.Windows;
using EgguWare.Overrides;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace EgguWare.Utilities
{
    public class T
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetCurrentHwProfile(IntPtr fProfile);

        [StructLayout(LayoutKind.Sequential)]
        class HWProfile
        {
            public Int32 dwDockInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 39)]
            public string szHwProfileGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szHwProfileName;
        }

        public static Vector2 DropdownCursorPos;
        public static SteamPlayer[] ConnectedPlayers;
        public static float LastMovementCheck;
        public static Material DrawMaterial;
        private static readonly Texture2D backgroundTexture = Texture2D.whiteTexture;
        private static readonly GUIStyle textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };

        public static SteamPlayer GetSteamPlayer(Player player)
        {
            foreach (var user in Provider.clients)
            {
                if (user.player == player)
                    return user;
            }

            return null;
        }

        public static bool InScreenView(Vector3 scrnpt)
        {
            if (scrnpt.z <= 0f || scrnpt.x <= 0f || scrnpt.x >= 1f || scrnpt.y <= 0f || scrnpt.y >= 1f)
                return false;

            return true;
        }

        public static float GetDistance(Vector3 endpos)
        {
            return (float)System.Math.Round(Vector3.Distance(Player.player.look.aim.position, endpos));
        }

        public static bool VisibleFromCamera(Vector3 pos)
        {
            Vector3 dir = (pos - MainCamera.instance.transform.position).normalized;
            Physics.Raycast(MainCamera.instance.transform.position, dir, out RaycastHit result, Mathf.Infinity, RayMasks.DAMAGE_CLIENT);
            return DamageTool.getPlayer(result.transform);
        }

        public static void AimAt(Vector3 pos)
        {
            Player.player.transform.LookAt(pos);
            Player.player.transform.eulerAngles = new Vector3(0f, Player.player.transform.rotation.eulerAngles.y, 0f);
            Camera.main.transform.LookAt(pos);
            float x = Camera.main.transform.localRotation.eulerAngles.x;
            if (x <= 90f && x <= 270f)
            {
                x = Camera.main.transform.localRotation.eulerAngles.x + 90f;
            }
            else if (x >= 270f && x <= 360f)
            {
                x = Camera.main.transform.localRotation.eulerAngles.x - 270f;
            }
            Player.player.look.GetType().GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.look, x);
            Player.player.look.GetType().GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.look, Player.player.transform.rotation.eulerAngles.y);
        }

        public static IEnumerator Notify(string message, Color color, float displayTime)
        {
            var started = Time.realtimeSinceStartup;
            while (true)
            {
                yield return new WaitForEndOfFrame();

                PlayerUI.hint(null, EPlayerMessage.INTERACT, message, color);

                if (Time.realtimeSinceStartup - started > displayTime)
                    yield break;
            }
        }

        public static void DrawSnapline(Vector3 worldpos, Color color)
        {
            Vector3 pos = G.MainCamera.WorldToScreenPoint(worldpos);
            pos.y = Screen.height - pos.y;
            GL.PushMatrix();
            GL.Begin(1);
            DrawMaterial.SetPass(0);
            GL.Color(color);
            GL.Vertex3(Screen.width / 2, Screen.height, 0f);
            GL.Vertex3(pos.x, pos.y, 0f);
            GL.End();
            GL.PopMatrix();
        }

        public static void DrawESPLabel(Vector3 worldpos, Color textcolor, Color outlinecolor, string text, string outlinetext = null)
        {
            GUIContent content = new GUIContent(text);
            if (outlinetext == null) outlinetext = text;
            GUIContent content1 = new GUIContent(outlinetext);
            GUIStyle style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleCenter;
            Vector2 size = style.CalcSize(content);
            Vector3 pos = G.MainCamera.WorldToScreenPoint(worldpos);
            pos.y = Screen.height - pos.y;
            if (pos.z >= 0)
            {
                GUI.color = Color.black;
                GUI.Label(new Rect((pos.x - size.x / 2) + 1, pos.y + 1, size.x, size.y), content1);
                GUI.Label(new Rect((pos.x - size.x / 2) - 1, pos.y - 1, size.x, size.y), content1);
                GUI.Label(new Rect((pos.x - size.x / 2) + 1, pos.y - 1, size.x, size.y), content1);
                GUI.Label(new Rect((pos.x - size.x / 2) - 1, pos.y + 1, size.x, size.y), content1);
                GUI.color = textcolor;
                GUI.Label(new Rect(pos.x - size.x / 2, pos.y, size.x, size.y), content);
                GUI.color = Main.GUIColor;
            }
        }

        public static void DrawOutlineLabel(Vector2 rect, Color textcolor, Color outlinecolor, string text, string outlinetext = null)
        {
            GUIContent content = new GUIContent(text);
            if (outlinetext == null) outlinetext = text;
            GUIContent content1 = new GUIContent(outlinetext);
            GUIStyle style = GUI.skin.label;
            Vector2 size = style.CalcSize(content);
            GUI.color = Color.black;
            GUI.Label(new Rect((rect.x) + 1, rect.y + 1, size.x, size.y), content1);
            GUI.Label(new Rect((rect.x) - 1, rect.y - 1, size.x, size.y), content1);
            GUI.Label(new Rect((rect.x) + 1, rect.y - 1, size.x, size.y), content1);
            GUI.Label(new Rect((rect.x) - 1, rect.y + 1, size.x, size.y), content1);
            GUI.color = textcolor;
            GUI.Label(new Rect(rect.x, rect.y, size.x, size.y), content);
            GUI.color = Main.GUIColor;
        }

        public static void Draw3DBox(Bounds b, Color color)
        {
            Vector3[] pts = new Vector3[8];
            pts[0] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[1] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[2] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[3] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
            pts[4] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[5] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[6] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[7] = G.MainCamera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

            for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

            GL.PushMatrix();
            GL.Begin(1);
            DrawMaterial.SetPass(0);
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Begin(1);
            DrawMaterial.SetPass(0);
            GL.Color(color);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[2].x, pts[2].y, 0f);
            GL.Vertex3(pts[0].x, pts[0].y, 0f);
            GL.Vertex3(pts[3].x, pts[3].y, 0f);
            GL.Vertex3(pts[1].x, pts[1].y, 0f);
            GL.Vertex3(pts[7].x, pts[7].y, 0f);
            GL.Vertex3(pts[5].x, pts[5].y, 0f);
            GL.Vertex3(pts[6].x, pts[6].y, 0f);
            GL.Vertex3(pts[4].x, pts[4].y, 0f);

            GL.End();
            GL.PopMatrix();

        }


        // thanks
        public static void DrawColor(Rect position, Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none, textureStyle);
            GUI.backgroundColor = backgroundColor;
        }

        public static void DrawColorLayout(Color color, GUILayoutOption[] options = null)
        {
            GUI.skin = AssetUtilities.Skin;
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Button(" ", textureStyle, options);
            GUI.backgroundColor = backgroundColor;
        }

        public static Priority GetPriority(ulong id)
        {
            G.Settings.Priority.TryGetValue(id, out Priority value);
            return value;
        }

        public static void ApplyShader(Shader shader, GameObject pgo, Color32 VisibleColor, Color32 OccludedColor)
        {
            if (shader == null) return;

            Renderer[] rds = pgo.GetComponentsInChildren<Renderer>();

            for (int j = 0; j < rds.Length; j++)
            {
                Material[] materials = rds[j].materials;

                for (int k = 0; k < materials.Length; k++)
                {
                    materials[k].shader = shader;
                    materials[k].SetColor("_ColorVisible", VisibleColor);
                    materials[k].SetColor("_ColorBehind", OccludedColor);
                }
            }
        }

        public static void Log(string s) =>
            File.AppendAllText("EgguWare.log", "[" + DateTime.Now.ToLongTimeString() + "] " + s + Environment.NewLine);

        public static void RemoveShaders(GameObject pgo)
        {
            if (Shader.Find("Standard") == null) return;

            Renderer[] rds = pgo.GetComponentsInChildren<Renderer>();

            for (int j = 0; j < rds.Length; j++)
            {
                if (!(rds[j].material.shader != Shader.Find("Standard"))) continue;

                Material[] materials = rds[j].materials;

                for (int k = 0; k < materials.Length; k++)
                {
                    materials[k].shader = Shader.Find("Standard");
                }
            }
        }

        public static Vector3 GetLimbPosition(Transform target, string objName)
        {
            var componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
            var result = Vector3.zero;

            if (componentsInChildren == null) return result;

            foreach (var transform in componentsInChildren)
            {
                if (transform.name.Trim() != objName) continue;

                result = transform.position + new Vector3(0f, 0.4f, 0f);
                break;
            }

            return result;
        }

        public static void OverrideMethod(Type defaultClass, Type overrideClass, string method, BindingFlags bindingflag1, BindingFlags bindingflag2, BindingFlags overrideflag1, BindingFlags overrideflag2)
        {
            string overriddenmethod = "OV_" + method;

            var MethodToOverride = defaultClass.GetMember(method, MemberTypes.Method, bindingflag1 | bindingflag2).Cast<MethodInfo>();

            OverrideHelper.RedirectCalls(MethodToOverride.ToArray()[0], overrideClass.GetMethod(overriddenmethod, overrideflag1 | overrideflag2));
        }

        public static float? GetGunDistance()
        {
            ItemGunAsset currentGun = Player.player?.equipment?.asset as ItemGunAsset;
            return currentGun?.range ?? 15.5f;
        }

        public static float GetDamage(Player player, ELimb limb)
        {
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
            return currentGun.objectDamage * currentGun.playerDamageMultiplier.damage * DamageTool.getPlayerArmor(limb, player);
        }

        public static bool IsItemWhitelisted(InteractableItem item, ItemWhitelistObject itemWhitelistObject)
        {
            if (itemWhitelistObject.filterItems)
            {
                if (itemWhitelistObject.allowGun && item.asset is ItemGunAsset)
                    return true;
                else if (itemWhitelistObject.allowBackpack && item.asset is ItemBackpackAsset)
                    return true;
                else if (itemWhitelistObject.allowAmmo && (item.asset is ItemMagazineAsset || item.asset is ItemCaliberAsset))
                    return true;
                else if (itemWhitelistObject.allowAttachments && (item.asset is ItemBarrelAsset || item.asset is ItemOpticAsset))
                    return true;
                else if (itemWhitelistObject.allowClothing && item.asset is ItemClothingAsset)
                    return true;
                else if (itemWhitelistObject.allowFuel && item.asset is ItemFuelAsset)
                    return true;
                else if (itemWhitelistObject.allowMedical && item.asset is ItemMedicalAsset)
                    return true;
                else if (itemWhitelistObject.allowMelee && item.asset is ItemMeleeAsset)
                    return true;
                else if (itemWhitelistObject.allowThrowable && item.asset is ItemThrowableAsset)
                    return true;
                else if (itemWhitelistObject.allowFoodWater && (item.asset is ItemFoodAsset || item.asset is ItemWaterAsset))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public static ELimb GetLimb(TargetLimb limb)
        {
            if (limb == TargetLimb.GLOBAL)
                return ELimb.SKULL;
            else if (limb == TargetLimb.RANDOM)
            {
                ELimb[] array = (ELimb[])Enum.GetValues(typeof(ELimb));
                return array[Random.Next(0, array.Length)];
            }
            else
                return (ELimb)Enum.Parse(typeof(TargetLimb), Enum.GetName(typeof(TargetLimb), limb));
        }

        public static Player GetNearestPlayer(int? pixelfov = null, int? distance = null)
        {
            // end player
            Player returnplayer = null;

            for (int i = 0; i < ConnectedPlayers.Length; i++)
            {
                SteamPlayer loopplayer = ConnectedPlayers[i];
                #region Validity Checks
                if (loopplayer == null)
                    continue;
                if (loopplayer.playerID.steamID == Provider.client)
                    continue;
                if (loopplayer.player.life.isDead)
                    continue;
                if (distance != null && Vector3.Distance(Player.player.look.aim.position, loopplayer.player.transform.position) > distance)
                    continue;
                if (GetPriority(loopplayer.playerID.steamID.m_SteamID) == Priority.FRIENDLY)
                    continue;
                #endregion
                Vector3 HeadScreenPoint1 = G.MainCamera.WorldToScreenPoint(GetLimbPosition(loopplayer.player.transform, "Skull"));
                if (HeadScreenPoint1.z <= 0)
                    continue;

                int ToLoopPlayerPixels = (int)Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(HeadScreenPoint1.x, HeadScreenPoint1.y));
                if (pixelfov != null && ToLoopPlayerPixels > pixelfov)
                    continue;

                if (returnplayer == null) { returnplayer = loopplayer.player; continue; }
                Vector3 HeadScreenPoint2 = G.MainCamera.WorldToScreenPoint(GetLimbPosition(returnplayer.transform, "Skull"));
                int ToReturnPlayerPixels = (int)Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(HeadScreenPoint2.x, HeadScreenPoint2.y));

                if (pixelfov != null && ToReturnPlayerPixels > pixelfov)
                    returnplayer = null;

                if (ToLoopPlayerPixels < ToReturnPlayerPixels)
                    returnplayer = loopplayer.player;
            }
            return returnplayer;
        }

        public static void DrawCircle(Color Col, Vector2 Center, float Radius)
        {
            GL.PushMatrix();
            DrawMaterial.SetPass(0);
            GL.Begin(1);
            GL.Color(Col);
            for (float num = 0f; num < 6.28318548f; num += 0.05f)
            {
                GL.Vertex(new Vector3(Mathf.Cos(num) * Radius + Center.x, Mathf.Sin(num) * Radius + Center.y));
                GL.Vertex(new Vector3(Mathf.Cos(num + 0.05f) * Radius + Center.x, Mathf.Sin(num + 0.05f) * Radius + Center.y));
            }
            GL.End();
            GL.PopMatrix();
        }

        public static void Dropdown(float height, float width, string title, System.Action code)
        {
            DropdownWindow.DropdownOpen = true;
            Main.DropdownPos = new Rect(Main.CursorPos.x, Main.CursorPos.y, width, height);
            DropdownWindow.DropdownAction = code;
            Main.DropdownTitle = title;
        }

        public static IEnumerator CheckVerification(Vector3 LastPos)
        {
            if (Time.realtimeSinceStartup - LastMovementCheck < 0.8f)
                yield break;

            LastMovementCheck = Time.realtimeSinceStartup;
            Player.player.transform.position = new Vector3(0, -1337, 0);
            yield return new WaitForSeconds(3);

            if (Vector3.Distance(Player.player.transform.position, LastPos) < 10)
                G.UnrestrictedMovement = false;
            else
            {
                G.UnrestrictedMovement = true;
                Player.player.transform.position = LastPos + new Vector3(0, 5, 0);
            }
        }



        public static Random Random = new Random();
    }
}
