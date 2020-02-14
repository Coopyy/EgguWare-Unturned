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
    public class hkItemManager
    {
        // makes "nearby items" show at full 19m radius
        public static void OV_getItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<InteractableItem> result)
        {
            if (ItemManager.regions == null)
            {
                return;
            }
            for (int i = 0; i < search.Count; i++)
            {
                RegionCoordinate regionCoordinate = search[i];
                if (ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
                {
                    for (int j = 0; j < ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops.Count; j++)
                    {
                        ItemDrop itemDrop = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops[j];
                        if ((itemDrop.model.position - center).sqrMagnitude < 361)
                        {
                            result.Add(itemDrop.interactableItem);
                        }
                    }
                }
            }
        }
    }
}
