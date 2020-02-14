using EgguWare.Attributes;
using EgguWare.Classes;
using EgguWare.Menu;
using EgguWare.Overrides;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Cheats
{
    [Comp]
    public class Weapons : MonoBehaviour
    {
        public Rect GunInfoWin = new Rect(Screen.width - 135, 50, 120, 50);
        public static List<IndicatorObject> DamageIndicators = new List<IndicatorObject>();
        public static List<TracerObject> TracerLines = new List<TracerObject>();
        public static Dictionary<ushort, float> SpreadBackup = new Dictionary<ushort, float>();
        public void Update()
        {
            if (Provider.isConnected && !Provider.isLoading)
            {
                if (Player.player?.equipment?.asset is ItemGunAsset)
                {
                    ItemGunAsset gun = (ItemGunAsset)Player.player?.equipment?.asset;

                    if (!SpreadBackup.ContainsKey(gun.id))
                        SpreadBackup.Add(gun.id, gun.spreadHip);

                    if (G.Settings.WeaponOptions.RemoveBurstDelay || G.Settings.WeaponOptions.RemoveHammerDelay || G.Settings.WeaponOptions.InstantReload)
                        Player.player.equipment.isBusy = false;
                    if (G.Settings.WeaponOptions.RemoveHammerDelay)
                        Player.player.equipment.useable.GetType().GetField("isHammering", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.equipment.useable, false);
                    if (G.Settings.WeaponOptions.RemoveHammerDelay)
                        Player.player.equipment.useable.GetType().GetField("needsRechamber", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.equipment.useable, false);
                    if (G.Settings.WeaponOptions.InstantReload)
                        Player.player.equipment.useable.GetType().GetField("reloadTime", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.player.equipment.useable, 0f);

                    if (G.Settings.WeaponOptions.NoSpread)
                    {
                        gun.spreadAim = 0f;
                        gun.spreadHip = 0f;
                    }
                    if (G.BeingSpied || !G.Settings.WeaponOptions.NoSpread) // revert crosshair on spy
                    {
                        SpreadBackup.TryGetValue(gun.id, out var backupspread);
                        gun.spreadHip = backupspread;
                        gun.spreadAim = backupspread;
                    }
                    typeof(UseableGun).GetMethod("updateCrosshair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Player.player.equipment.useable, null);
                    if (G.Settings.WeaponOptions.NoRecoil)
                    {
                        gun.recoilMax_x = 0f;
                        gun.recoilMax_y = 0f;
                        gun.recoilMin_x = 0f;
                        gun.recoilMin_y = 0f;
                    }

                    if (G.Settings.WeaponOptions.NoSway)
                    {
                        Player.player.animator.viewSway = Vector3.zero;
                    }
                }
            }
        }

        void OnGUI()
        {
            if (Provider.isConnected && !Provider.isLoading)
            {
                GUI.skin = AssetUtilities.Skin;
                if (!G.BeingSpied)
                {
                    if (G.Settings.WeaponOptions.WeaponInfo)
                        GunInfoWin = GUILayout.Window(6, GunInfoWin, GunInfoWindow, "Weapon Info");

                    #region Tracers
                    if (G.Settings.WeaponOptions.TracerLines && TracerLines.Count > 0)
                    {
                        T.DrawMaterial.SetPass(0);
                        GL.PushMatrix();
                        GL.LoadProjectionMatrix(G.MainCamera.projectionMatrix);
                        GL.modelview = G.MainCamera.worldToCameraMatrix;
                        GL.Begin(GL.LINES);
                        for (int i = TracerLines.Count - 1; i > -1; i--)
                        {
                            TracerObject t = TracerLines[i];
                            if (DateTime.Now - t.ShotTime > TimeSpan.FromSeconds(G.Settings.WeaponOptions.TracerTime))
                            {
                                TracerLines.Remove(t);
                                continue;
                            }

                            GL.Color(Colors.GetColor("Bullet_Tracer_Color"));

                            GL.Vertex(t.PlayerPos);
                            GL.Vertex(t.HitPos);
                        }
                        GL.End();
                        GL.PopMatrix();
                    }
                    #endregion
                    #region Damage Indicators
                    if (G.Settings.WeaponOptions.DamageIndicators && DamageIndicators.Count > 0)
                    {
                        T.DrawMaterial.SetPass(0);
                        for (int i = DamageIndicators.Count - 1; i > -1; i--)
                        {
                            IndicatorObject t = DamageIndicators[i];
                            if (DateTime.Now - t.ShotTime > TimeSpan.FromSeconds(3))
                            {
                                DamageIndicators.Remove(t);
                                continue;
                            }
                            GUI.color = Color.red;
                            Vector3 pos = G.MainCamera.WorldToScreenPoint(t.HitPos + new Vector3(0, 1, 0));
                            pos.y = Screen.height - pos.y;
                            if (pos.z >= 0)
                            {
                                GUIStyle style = GUI.skin.label;
                                style.alignment = TextAnchor.MiddleCenter;
                                Vector2 size = style.CalcSize(new GUIContent($"<b>{t.Damage}</b>"));
                                T.DrawOutlineLabel(new Vector2(pos.x - size.x / 2, pos.y - ((float)(DateTime.Now - t.ShotTime).TotalSeconds * 10)), Color.red, Color.black, $"<b>{t.Damage}</b>");
                                style.alignment = TextAnchor.MiddleLeft;
                            }
                            GUI.color = Main.GUIColor;
                        }
                    }
                    #endregion
                }
            }
        }

        void GunInfoWindow(int winid)
        {
            GUILayout.Label("Range: " + T.GetGunDistance());
            GUI.DragWindow();
        }

        public static void AddTracer(RaycastInfo ri)
        {
            if (G.Settings.WeaponOptions.TracerLines && ri.point != new Vector3(0, 0, 0))
            {
                TracerObject tracer = new TracerObject
                {
                    HitPos = ri.point,
                    PlayerPos = Player.player.look.aim.transform.position,
                    ShotTime = DateTime.Now
                };
                TracerLines.Add(tracer);
            }
        }

        public static void AddDamage(RaycastInfo ri)
        {
            if (G.Settings.WeaponOptions.DamageIndicators && ri.point != new Vector3(0, 0, 0))
            {
                ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
                if (currentGun != null && ri.player != null)
                {
                    IndicatorObject dmgi = new IndicatorObject
                    {
                        HitPos = ri.point,
                        Damage = Mathf.FloorToInt(DamageTool.getPlayerArmor(ri.limb, ri.player) * currentGun.playerDamageMultiplier.multiply(ri.limb)), // big maths
                        ShotTime = DateTime.Now
                    };
                    DamageIndicators.Add(dmgi);
                }
            }
        }
    }
}
