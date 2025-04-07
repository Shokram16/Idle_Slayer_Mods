using Il2Cpp;
using UnityEngine;
using HarmonyLib;

namespace Enhanced_Divinities;

public class Enhanced_Divinities : MonoBehaviour
{
    private void Awake()
    {
        Plugin.Logger.Msg($"Enhaced Divinities Started");

    }
    [HarmonyPatch(typeof(DivinitiesManager), "PrestigeMinion")]
    public class Patch_PrestigeMinion
    {
        static void Prefix(ref Minion minion)
        {
            if (Plugin.Settings.MinionLevels.Value > 0)
                minion.level = Plugin.Settings.MinionLevels.Value;
            else
                minion.level = minion.maxLevel;
        }
    }

}
