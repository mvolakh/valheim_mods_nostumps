using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace NoStumps
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class NoStumps : BaseUnityPlugin
    {
        private const string modGUID = "merry.nostumps";
        private const string modName = "NoStumps";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> dropEnabled;

        void Awake()
        {
            modEnabled = Config.Bind<bool>("General", "Enable", true, "Enable/disable the mod");
            dropEnabled = Config.Bind<bool>("General", "EnableDrop", true, "Enable/disable stumps dropping loot.");

            if (!modEnabled.Value) 
                return;

            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(TreeBase), "SpawnLog")]
        static class TreeBase_SpawnLog_Patch
        {
            static void Prefix(ref GameObject ___m_stubPrefab)
            {
                if (!dropEnabled.Value)
                {
                    ___m_stubPrefab = null;
                }
                else
                {
                    Destructible comp = ___m_stubPrefab.GetComponent<Destructible>();
                    if (comp)
                        comp.m_ttl = 0.01f;
                }
            }
        }
    }
}
