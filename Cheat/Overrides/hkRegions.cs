using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Overrides
{
    public class hkRegions
    {
        public static void OV_getRegionsInRadius(Vector3 center, float radius, List<RegionCoordinate> result)
		{
            radius = 19; // can be changed by a user, good idea yes
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(center, out b, out b2))
			{
				return;
			}
			result.Add(new RegionCoordinate(b, b2));
			Vector3 vector;
			Regions.tryGetPoint((int)b, (int)b2, out vector);
			byte b3 = b;
			byte b4 = b2;
			if (center.x - radius <= vector.x)
			{
				b3 -= 1;
			}
			else if (center.x + radius >= vector.x + (float)Regions.REGION_SIZE)
			{
				b3 += 1;
			}
			if (center.z - radius <= vector.z)
			{
				b4 -= 1;
			}
			else if (center.z + radius >= vector.z + (float)Regions.REGION_SIZE)
			{
				b4 += 1;
			}
			if (b3 != b && Regions.checkSafe((int)b3, (int)b2))
			{
				result.Add(new RegionCoordinate(b3, b2));
			}
			if (b4 != b2 && Regions.checkSafe((int)b, (int)b4))
			{
				result.Add(new RegionCoordinate(b, b4));
			}
			if (b3 != b && b4 != b2 && Regions.checkSafe((int)b3, (int)b4))
			{
				result.Add(new RegionCoordinate(b3, b4));
			}
		}
    }
}
