using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgguWare.Classes
{
    public enum MenuTab
    {
        Visuals,
        Aimbot,
        Misc,
        Weapons,
        Settings
    }

    public enum ESPObject
    {
        Player,
        Vehicle,
        Item,
        Zombie,
        Storage,
        Bed,
        Flag
    }

    public enum TargetLimb
    {
        LEFT_FOOT = 0,
        LEFT_LEG = 1,
        RIGHT_FOOT = 2,
        RIGHT_LEG = 3,
        LEFT_HAND = 4,
        LEFT_ARM = 5,
        RIGHT_HAND = 6,
        RIGHT_ARM = 7,
        LEFT_BACK = 8,
        RIGHT_BACK = 9,
        LEFT_FRONT = 10,
        RIGHT_FRONT = 11,
        SPINE = 12,
        SKULL = 13,
        RANDOM,
        GLOBAL
    }

    public enum TargetLimb1
    {
        LEFT_FOOT = 0,
        LEFT_LEG = 1,
        RIGHT_FOOT = 2,
        RIGHT_LEG = 3,
        LEFT_HAND = 4,
        LEFT_ARM = 5,
        RIGHT_HAND = 6,
        RIGHT_ARM = 7,
        LEFT_BACK = 8,
        RIGHT_BACK = 9,
        LEFT_FRONT = 10,
        RIGHT_FRONT = 11,
        SPINE = 12,
        SKULL = 13,
        RANDOM
    }

    public enum Priority
    {
        NONE = 1,
        FRIENDLY = 2,
        MARKED = 3
    }

    public enum Mute
    {
        NONE = 1,
        ALL = 2,
        GLOBAL = 3,
        AREA = 4,
        GROUP = 5
    }

    public enum MiscOptions
    {
        Game,
        Unrestricted_Movement
    }

    public enum SettingsOptions
    {
        Colors,
        Keybinds
    }

    public enum AimbotOptions
    {
        Silent,
        Aimlock
    }

    public enum ShaderType
    {
        Material,
        Flat,
        None
    }
}
