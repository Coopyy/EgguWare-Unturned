using EgguWare.Attributes;
using EgguWare.Classes;
using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Cheats
{
    [Comp]
    public class Items : MonoBehaviour
    {
        public static bool editingaip = false;
        void Start() => StartCoroutine(RefreshClumpedItems());
        void Update()
        {
            if (G.Settings.MiscOptions.AutoItemPickup)
            {
                Collider[] array = Physics.OverlapSphere(Player.player.transform.position, 19f, RayMasks.ITEM);
                for (int i = 0; i < array.Length; i++)
                {
                    Collider collider = array[i];
                    if (collider == null || collider.GetComponent<InteractableItem>() == null || collider.GetComponent<InteractableItem>().asset == null) continue;
                    InteractableItem item = collider.GetComponent<InteractableItem>();

                    if (T.IsItemWhitelisted(item, G.Settings.MiscOptions.AIPWhitelist))
                        item.use();
                }
            }
        }

        IEnumerator RefreshClumpedItems()
        {
            while (true)
            {
                if (G.Settings.ItemOptions.Enabled && G.Settings.GlobalOptions.ListClumpedItems)
                {
                    ESP.ItemClumps.Clear();
                    InteractableItem[] worlditems = FindObjectsOfType<InteractableItem>();
                    for (int a = 0; a < worlditems.Length; a++)
                    {
                        InteractableItem i = worlditems[a];

                        if (!T.IsItemWhitelisted(i, G.Settings.MiscOptions.ESPWhitelist) || IsAlreadyClumped(i))
                            continue;

                        Collider[] array = Physics.OverlapSphere(i.transform.position, G.Settings.GlobalOptions.DistanceThreshhold, RayMasks.ITEM);
                        List<InteractableItem> tempitems = new List<InteractableItem>();
                        for (int iq = 0; iq < array.Length; iq++)
                        {
                            Collider collider = array[iq];
                            if (collider == null || collider.GetComponent<InteractableItem>() == null || collider.GetComponent<InteractableItem>().asset == null) continue;
                            InteractableItem item = collider.GetComponent<InteractableItem>();
                            if (!T.IsItemWhitelisted(item, G.Settings.MiscOptions.ESPWhitelist) || IsAlreadyClumped(i))
                                continue;
                            tempitems.Add(item);
                        }
                        if (tempitems.Count >= G.Settings.GlobalOptions.CountThreshhold)
                            ESP.ItemClumps.Add(new ItemClumpObject(tempitems, i.transform.position));
                    }
                }

                yield return new WaitForSeconds(4);
            }
        }

        public static bool IsAlreadyClumped(InteractableItem item)
        {
            foreach (ItemClumpObject clumpObjects in ESP.ItemClumps)
                if (clumpObjects.ClumpedItems.Contains(item))
                    return true;
            return false;
        }
    }
}
