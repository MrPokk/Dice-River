using System;
namespace BitterECS.Extra
{
    public static class PathProject
    {
#if UNITY_EDITOR || UNITY_2017_1_OR_NEWER
        public static string RootPath { get; set; } = "Resources";
        public static string DataPath { get; set; } = UnityEngine.Application.dataPath;
        public static string ProductName { get; set; } = UnityEngine.Application.productName;
#else
        public static string RootPath { get; set; } = "Resources";
        public static string DataPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public static string ProductName { get; set; } = "App";
#endif

        public const string ENTITIES = "Entities/";
        public const string SETTINGS = "!Settings/";
        public const string UI = "UI/";
        public const string DICES = ENTITIES + "Dices/";
        public const string HAZARD = ENTITIES + "Hazards/";
        public const string PICKUPS = ENTITIES + "Pickups/";
        public const string PREFAB_OBJECTS = ENTITIES + "Prefabs/";
        public const string GRIDS = PREFAB_OBJECTS + "Grids/";
        public const string HAND = PREFAB_OBJECTS + "Hand/";
        public const string WORLD = PREFAB_OBJECTS + "World/";
        public const string UI_POPUPS = UI + "!Popups/";
        public const string UI_PREFABS = UI + "Prefabs/";
        public const string UI_SCREENS = UI + "!Screens/";
        public const string UI_ICON = UI_PREFABS + "Icon/";
        public const string RIVER_OBJECTS = WORLD + "River/";
    }
}
