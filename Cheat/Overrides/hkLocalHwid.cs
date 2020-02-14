using EgguWare.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EgguWare.Overrides
{
    public class hkLocalHwid
    {
        // the hwid sent to the server
        public static byte[] OV_getHwid()
        {
            List<byte> IdentifierBytes = new List<byte>();

            // instead of sending a random hwid each time you join a new server, which might not look legit, keep one hwid and let the user change it
            for (int i = 0; i < 20; i++)
                IdentifierBytes.Add((byte)T.Random.Next(0, 100));

            typeof(LocalHwid).GetField("cachedHwid", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, IdentifierBytes.ToArray());
            return IdentifierBytes.ToArray();
        }
    }
}
