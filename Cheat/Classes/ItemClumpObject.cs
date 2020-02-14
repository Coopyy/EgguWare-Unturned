using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Classes
{
    public class ItemClumpObject
    {
        public List<InteractableItem> ClumpedItems;
        public Vector3 WorldPos;

        public ItemClumpObject(List<InteractableItem> items, Vector3 pos)
        {
            ClumpedItems = items;
            WorldPos = pos;
        }
    }
}
