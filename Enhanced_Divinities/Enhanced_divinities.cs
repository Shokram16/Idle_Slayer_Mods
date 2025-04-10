using Il2Cpp;
using UnityEngine;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine.UI;

namespace Enhanced_Divinities;

public class Enhanced_Divinities : MonoBehaviour
{
    private void Awake()
    {
        Plugin.Logger.Msg($"Enhaced Divinities Started");

    }

    // Patch Single Minion Prestige
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

    // Patch All Minions Prestige If Prestige Popup is open
    [HarmonyPatch(typeof(Button), "Press")]
    public class Patch_ConfirmButtonPress
    {
        static void Prefix(Button __instance)
        {
            if (__instance.name == "Confirm Button" && PrestigeContext.IsPrestigePopupOpen)
            {
                Plugin.Logger.Msg(ConsoleColor.Yellow, "[!] Se presionó el Confirm Button de Prestige");

                Il2CppArrayBase<Minion> minions = (Il2CppReferenceArray<Minion>)Resources.FindObjectsOfTypeAll<Minion>();

                foreach (var minion in minions)
                {
                    if (minion.level > 1)
                    {
                        int minionLevel = Plugin.Settings.MinionLevels.Value == 0 ? minion.maxLevel : Plugin.Settings.MinionLevels.Value;

                        minion.level = minionLevel;
                        Plugin.Logger.Msg($"Leveling Minion: {minion.name}");
                        Plugin.Logger.Msg($"Leveling lvl: {minion.level}");
                    }
                }
                PrestigeContext.IsPrestigePopupOpen = false;
            }
        }
    }

    // Sets Prestige Popup to true
    [HarmonyPatch(typeof(UnityEngine.UI.Button), "Press")]
    public class Patch_PrestigeButton
    {
        static void Prefix(UnityEngine.UI.Button __instance)
        {
            if (__instance.name != "Prestige Button") return;

            PrestigeContext.IsPrestigePopupOpen = true;
        }
    }

    // Sets Prestige Popup to false
    [HarmonyPatch(typeof(UnityEngine.UI.Button), "Press")]
    public class Patch_CancelButton
    {
        static void Prefix(UnityEngine.UI.Button __instance)
        {
            if (__instance.name != "Cancel Button") return;

            PrestigeContext.IsPrestigePopupOpen = false;
        }
    }
}
