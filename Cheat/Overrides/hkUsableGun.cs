using EgguWare.Attributes;
using EgguWare.Cheats;
using EgguWare.Classes;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EgguWare.Overrides
{
    [Comp]
    public class hkUsableGun : MonoBehaviour
    {
        private static FieldInfo BulletsField;
        void Start()
        {
            BulletsField = typeof(UseableGun).GetField("bullets", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }

        public void OV_ballistics()
        {
            Useable PlayerUse = Player.player.equipment.useable;

            if (Time.realtimeSinceStartup - PlayerLifeUI.hitmarkers[0].lastHit > PlayerUI.HIT_TIME)
            {
                PlayerLifeUI.hitmarkers[0].hitBuildImage.isVisible = false;
                PlayerLifeUI.hitmarkers[0].hitCriticalImage.isVisible = false;
                PlayerLifeUI.hitmarkers[0].hitEntitiyImage.isVisible = false;
            }

            ItemGunAsset PAsset = (ItemGunAsset)Player.player.equipment.asset;
            PlayerLook Look = Player.player.look;

            if (PAsset.projectile != null)
                return;

            List<BulletInfo> Bullets = (List<BulletInfo>)BulletsField.GetValue(PlayerUse);

            if (Bullets.Count == 0)
                return;

            Transform t = (Player.player.look.perspective == EPlayerPerspective.FIRST ? Player.player.look.aim : G.MainCamera.transform);
            RaycastInfo ri = hkDamageTool.SetupRaycast(new Ray(t.position, t.forward), T.GetGunDistance().Value, RayMasks.DAMAGE_CLIENT);
            if (Provider.modeConfigData.Gameplay.Ballistics)
            {
                for (int i = 0; i < Bullets.Count; i++)
                {
                    BulletInfo bulletInfo = Bullets[i];
                    double distance = Vector3.Distance(Player.player.transform.position, ri.point);

                    if (bulletInfo.steps * PAsset.ballisticTravel < distance)
                        continue;

                    EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
                    PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
                    Player.player.input.sendRaycast(ri);
                    Weapons.AddTracer(ri);
                    Weapons.AddDamage(ri);
                    bulletInfo.steps = 254;
                }


                for (int k = Bullets.Count - 1; k >= 0; k--)
                {
                    BulletInfo bulletInfo = Bullets[k];
                    bulletInfo.steps += 1;
                    if (bulletInfo.steps >= PAsset.ballisticSteps)
                        Bullets.RemoveAt(k);
                }
            }
            else
            {
                for (int i = 0; i < Bullets.Count; i++)
                {
                    EPlayerHit eplayerhit = CalcHitMarker(PAsset, ref ri);
                    PlayerUI.hitmark(0, Vector3.zero, false, eplayerhit);
                    Player.player.input.sendRaycast(ri);
                    Weapons.AddTracer(ri);
                    Weapons.AddDamage(ri);
                }

                Bullets.Clear();
            }
            return;
        }

        public static EPlayerHit CalcHitMarker(ItemGunAsset PAsset, ref RaycastInfo ri)
        {
            EPlayerHit eplayerhit = EPlayerHit.NONE;

            if (ri == null || PAsset == null)
                return eplayerhit;

            if (ri.animal || ri.player || ri.zombie)
            {
                eplayerhit = EPlayerHit.ENTITIY;
                if (ri.limb == ELimb.SKULL)
                    eplayerhit = EPlayerHit.CRITICAL;
            }
            else if (ri.transform)
            {
                if (ri.transform.CompareTag("Barricade") && PAsset.barricadeDamage > 1f)
                {
                    InteractableDoorHinge component = ri.transform.GetComponent<InteractableDoorHinge>();
                    if (component != null)
                        ri.transform = component.transform.parent.parent;

                    if (!ushort.TryParse(ri.transform.name, out ushort id)) return eplayerhit;

                    ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);

                    if (itemBarricadeAsset == null || !itemBarricadeAsset.isVulnerable && !PAsset.isInvulnerable)
                        return eplayerhit;

                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;
                }
                else if (ri.transform.CompareTag("Structure") && PAsset.structureDamage > 1f)
                {
                    if (!ushort.TryParse(ri.transform.name, out ushort id2)) return eplayerhit;

                    ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);

                    if (itemStructureAsset == null || !itemStructureAsset.isVulnerable && !PAsset.isInvulnerable)
                        return eplayerhit;

                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;
                }
                else if (ri.transform.CompareTag("Resource") && PAsset.resourceDamage > 1f)
                {
                    if (!ResourceManager.tryGetRegion(ri.transform, out byte x, out byte y, out ushort index))
                        return eplayerhit;

                    ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);

                    if (resourceSpawnpoint == null || resourceSpawnpoint.isDead ||
                        resourceSpawnpoint.asset.bladeID != PAsset.bladeID) return eplayerhit;

                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;
                }
                else if (PAsset.objectDamage > 1f)
                {
                    InteractableObjectRubble component2 = ri.transform.GetComponent<InteractableObjectRubble>();

                    if (component2 == null) return eplayerhit;

                    ri.section = component2.getSection(ri.collider.transform);

                    if (component2.isSectionDead(ri.section) ||
                        !component2.asset.rubbleIsVulnerable && !PAsset.isInvulnerable) return eplayerhit;

                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;
                }
            }
            else if (ri.vehicle && !ri.vehicle.isDead && PAsset.vehicleDamage > 1f)
                if (ri.vehicle.asset != null && (ri.vehicle.asset.isVulnerable || PAsset.isInvulnerable))
                    if (eplayerhit == EPlayerHit.NONE)
                        eplayerhit = EPlayerHit.BUILD;

            return eplayerhit;
        }
    }
}
