using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EgguWare.Utilities
{
    public class AssetUtilities
    {
        public static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        public static GUISkin Skin;
        public static GUISkin VanillaSkin;
        public static void GetAssets()
        {
            if (!Directory.Exists(Application.dataPath + "/GUISkins/"))
                Directory.CreateDirectory(Application.dataPath + "/GUISkins/");

            AssetBundle Bundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(Application.dataPath + "/EgguWare.assets"));

            foreach (Shader s in Bundle.LoadAllAssets<Shader>())
                Shaders.Add(s.name, s);

            VanillaSkin = Bundle.LoadAllAssets<GUISkin>().First();
            if (!String.IsNullOrEmpty(G.Settings.MiscOptions.UISkin))
                LoadGUISkinFromName(G.Settings.MiscOptions.UISkin);
            else
                Skin = VanillaSkin;
        }
        public static void LoadGUISkinFromName(string name)
        {
            if (File.Exists(Application.dataPath + "/GUISkins/" + name + ".assets"))
            {
                AssetBundle tempAsset = AssetBundle.LoadFromMemory(File.ReadAllBytes(Application.dataPath + "/GUISkins/" + name + ".assets"));
                Skin = tempAsset.LoadAllAssets<GUISkin>().First();
                tempAsset.Unload(false);
                G.Settings.MiscOptions.UISkin = name;
            }
            else
            {
                Skin = VanillaSkin;
                G.Settings.MiscOptions.UISkin = "";
            }
        }
        public static List<string> GetSkins(bool Extensions = false)
        {
            List<string> files = new List<string>();
            DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/GUISkins/");
            FileInfo[] Files = d.GetFiles("*.assets");
            foreach (FileInfo file in Files)
            {
                if (Extensions)
                    files.Add(file.Name.Substring(0, file.Name.Length));
                else
                    files.Add(file.Name.Substring(0, file.Name.Length - 7));
            }
            return files;
        }
    }
}
