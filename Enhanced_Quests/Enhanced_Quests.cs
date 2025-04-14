using HarmonyLib;
using IdleSlayerMods.Common.Extensions;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Enhanced_Quests;

public class Enhaced_Quests : MonoBehaviour
{
    private static Il2CppSystem.Collections.Generic.List<Quest> _questsList;
    private static DailyQuestsManager _dailyQuestsManager;
    private static WeeklyQuestsManager _weeklyQuestsManager;
    private static GameObject _dailyQuestManagerObject;
    private static GameObject _gameObject;

    // Comprobar si es necesario volver a conseguir los objects etc
    public static void CheckToRegenerate()
    {
        _dailyQuestManagerObject = GameObject.Find("Daily Quest Manager");
        _dailyQuestsManager = _dailyQuestManagerObject.GetComponent<DailyQuestsManager>();
        _weeklyQuestsManager = _dailyQuestManagerObject.GetComponent<WeeklyQuestsManager>();

        _gameObject = GameObject.Find("UIManager/Safe Zone/Shop Panel/Wrapper/Quests/Quests");
        _questsList = _gameObject.GetComponent<QuestsList>().lastScrollListData;

        int dailyQuestsCount = 0;
        int weeklyQuestsCount = 0;
        foreach (var quest in _questsList)
        {
            var questTypeName = quest.GetIl2CppType().FullName;

            if (questTypeName == "WeeklyQuest")
                weeklyQuestsCount++;
            else if (questTypeName == "DailyQuest")
                dailyQuestsCount++;
        }

        if (dailyQuestsCount == 0)
            _dailyQuestsManager.RegenerateDailys();

        if (weeklyQuestsCount == 0)
            _weeklyQuestsManager.RegenerateWeeklies();

    }

    [HarmonyPatch(typeof(Button), "Press")]
    public class Patch_PrestigeButton
    {
        static void Postfix(Button __instance)
        {
            if (__instance.name != "Button") return;
            CheckToRegenerate();
        }
    }


}
