using EgguWare.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EgguWare.Utilities
{
    public static class AttributeUtilities
    {
        public static void LinkAttributes()
        {
            foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
            {
                // Collect and add components marked with the attribute
                if (T.IsDefined(typeof(Comp), false))
                    Load.CO.AddComponent(T);
            }
        }
    }
}