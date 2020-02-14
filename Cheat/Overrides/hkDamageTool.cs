using EgguWare.Cheats;
using EgguWare.Classes;
using EgguWare.Utilities;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkDamageTool
    {
        public static RaycastInfo OV_raycast(Ray ray, float range, int mask)
        {
            return SetupRaycast(ray, range, mask);
        }

        public static RaycastInfo SetupRaycast(Ray ray, float range, int mask)
        {
            RaycastInfo info;
            if (G.Settings.AimbotOptions.SilentAim && SilAimRaycast(out RaycastInfo ri))
                info = ri;
            else
                info = OriginalRaycast(ray, range, mask);

            return info;
        }

        #region Original Raycast
        public static RaycastInfo OriginalRaycast(Ray ray, float range, int mask)
        {
            RaycastHit hit;
            PhysicsUtility.raycast(ray, out hit, range, mask, QueryTriggerInteraction.UseGlobal);
            RaycastInfo raycastInfo = new RaycastInfo(hit);
            raycastInfo.direction = ray.direction;
            if (raycastInfo.transform != null)
            {
                if (raycastInfo.transform.CompareTag("Barricade"))
                    raycastInfo.transform = DamageTool.getBarricadeRootTransform(raycastInfo.transform);

                else if (raycastInfo.transform.CompareTag("Structure"))
                    raycastInfo.transform = DamageTool.getStructureRootTransform(raycastInfo.transform);

                if (raycastInfo.transform.CompareTag("Enemy"))
                    raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);

                if (raycastInfo.transform.CompareTag("Zombie"))
                    raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);

                if (raycastInfo.transform.CompareTag("Animal"))
                    raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);

                raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);

                if (raycastInfo.transform.CompareTag("Vehicle"))
                    raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);

                if (raycastInfo.zombie != null && raycastInfo.zombie.isRadioactive)
                    raycastInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
                else
                    raycastInfo.material = DamageTool.getMaterial(hit.point, raycastInfo.transform, raycastInfo.collider);
            }
            return raycastInfo;
        }
        #endregion

        #region Sphere Silent Aim
        public static bool SilAimRaycast(out RaycastInfo info)
        {
            ItemGunAsset currentGun = Player.player.equipment.asset as ItemGunAsset;
            float Range = currentGun?.range ?? 15.5f;
            Transform t = (Player.player.look.perspective == EPlayerPerspective.FIRST ? Player.player.look.aim : G.MainCamera.transform);
            info = OriginalRaycast(new Ray(t.position, t.forward), Range, RayMasks.DAMAGE_CLIENT);
            Player aimplayer = null;
            int? fov = null;
            if (G.Settings.AimbotOptions.SilentAimLimitFOV)
                fov = G.Settings.AimbotOptions.SilentAimFOV;
            if (T.GetNearestPlayer(fov, (int)T.GetGunDistance()))
                aimplayer = T.GetNearestPlayer();
            else
                return false;

            if (G.Settings.AimbotOptions.HitChance != 100)
                if (!(T.Random.Next(0, 100) < G.Settings.AimbotOptions.HitChance))
                    return false;

            SphereComponent Component = aimplayer.gameObject.GetComponent<SphereComponent>();
            if (!Component)
                aimplayer.gameObject.AddComponent<SphereComponent>();
            Component.LastHit = Time.realtimeSinceStartup;

            Vector3 point;
            if (T.VisibleFromCamera(T.GetLimbPosition(aimplayer.gameObject.transform, "Skull")))
                point = T.GetLimbPosition(aimplayer.gameObject.transform, "Skull");
            else if (T.VisibleFromCamera(T.GetLimbPosition(aimplayer.gameObject.transform, "Spine")))
                point = T.GetLimbPosition(aimplayer.gameObject.transform, "Spine");
            else if (T.VisibleFromCamera(T.GetLimbPosition(aimplayer.gameObject.transform, "Right_Hip")))
                point = T.GetLimbPosition(aimplayer.gameObject.transform, "Right_Hip");
            else if (T.VisibleFromCamera(T.GetLimbPosition(aimplayer.gameObject.transform, "Left_Foot")))
                point = T.GetLimbPosition(aimplayer.gameObject.transform, "Left_Foot");
            else if (T.VisibleFromCamera(T.GetLimbPosition(aimplayer.gameObject.transform, "Right_Leg")))
                point = T.GetLimbPosition(aimplayer.gameObject.transform, "Right_Leg");
            else if (!GetPoint(aimplayer.gameObject, Player.player.look.aim.position, Range, out point))
                return false;

            ELimb lomb;
            if (G.Settings.AimbotOptions.TargetL == TargetLimb1.RANDOM)
                lomb = T.GetLimb(TargetLimb.RANDOM);
            else
                lomb = (ELimb)G.Settings.AimbotOptions.TargetL;
            if (G.Settings.TargetLimb.TryGetValue(aimplayer.channel.owner.playerID.steamID.m_SteamID, out TargetLimb limb))
                if (limb != TargetLimb.GLOBAL)
                    lomb = T.GetLimb(limb);


            info = new RaycastInfo(aimplayer.transform)
            {
                point = point,
                direction = Player.player.look.aim.forward,
                limb = lomb,
                material = EPhysicsMaterial.NONE,
                player = aimplayer,
            };
            return true;
        }
        public static bool GetPoint(GameObject Target, Vector3 StartPos, double MaxRange, out Vector3 Point)
        {
            Point = Vector3.zero;
            if (!G.Settings.AimbotOptions.ExpandHitboxes)
                return false;
            if (Target == null)
                return false;

            SphereComponent Component = Target.GetComponent<SphereComponent>();

            // big ponch range
            if (Vector3.Distance(Target.transform.position, StartPos) <= 15.5f)
            {
                Point = Player.player.transform.position;
                return true;
            }

            Vector3[] verts = Component.Sphere.GetComponent<MeshCollider>().sharedMesh.vertices;
            foreach (Vector3 vertex in verts)
            {
                Vector3 tVertex = Component.Sphere.transform.TransformPoint(vertex);

                float Distance = (float)Vector3.Distance(StartPos, tVertex);
                if (Distance > MaxRange)
                    continue;

                if (Physics.Raycast(StartPos, Vector3.Normalize(tVertex - StartPos), Distance, RayMasks.DAMAGE_CLIENT))
                    continue;
                Point = tVertex;
                return true;
            }

            return false;
        }
        #endregion
    }
}
