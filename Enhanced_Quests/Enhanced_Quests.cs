using HarmonyLib;
using Il2Cpp;
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
    private static PortalButton _portalButton;
    private static DailyQuestReroll _dailyQuestReroll;
    private static WeeklyQuestReroll _weeklyQuestReroll;

    public void Awake()
    {
        CheckToRegenerate();
        _portalButton = PortalButton.instance;
        _dailyQuestReroll = DailyQuestReroll.instance;
        _weeklyQuestReroll = WeeklyQuestReroll.instance;

        CheckToRerollQuests();
    }

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

        if (dailyQuestsCount == 0 && CraftableSkills.dailyQuests && Plugin.Settings.ResetDailies.Value)
            _dailyQuestsManager.RegenerateDailys();

        if (weeklyQuestsCount == 0 && CraftableSkills.weeklyQuests && Plugin.Settings.ResetWeeklies.Value)
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

    [HarmonyPatch(typeof(Button), "Press")]
    public class Patch_PortalButton
    {
        static void Postfix(Button __instance)
        {
            if (__instance.name != "Portal Button" && __instance.name != "Portal Button(Clone)") return;
            CheckToResetPortal();
        }
    }

    [HarmonyPatch(typeof(Button), "Press")]
    public class Patch_RerollButton
    {
        static void Postfix(Button __instance)
        {
            if (__instance.name != "Confirm Button" && __instance.name != "Open Shop") return;

            CheckToRerollQuests();
        }
    }

    public static void CheckToResetPortal()
    {
        if (_portalButton.currentCd > 0 && Plugin.Settings.ResetPortal.Value)
            _portalButton.currentCd = 0;
    }

    public static void CheckToRerollQuests()
    {
        if (Plugin.Settings.ResetReroll.Value)
        {
            _dailyQuestReroll.rerollEnabled = true;
            _weeklyQuestReroll.rerollEnabled = true;
        }
    }


}
