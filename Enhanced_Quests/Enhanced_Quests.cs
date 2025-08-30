using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MelonLoader;
using System.Linq;

namespace Enhanced_Quests;

public class Enhanced_Quests : MonoBehaviour
{
    public static Enhanced_Quests Instance { get; private set; }

    private Il2CppSystem.Collections.Generic.List<Quest> _questsList;
    private DailyQuestsManager _dailyQuestsManager;
    private WeeklyQuestsManager _weeklyQuestsManager;
    private GameObject _gameObject;
    private PortalButton _portalButton;
    private DailyQuestReroll _dailyQuestReroll;
    private WeeklyQuestReroll _weeklyQuestReroll;
    private bool questsChecking = false;

    public bool claimingQuest = false;

    public void Awake()
    {
        Instance = this;

        _portalButton = PortalButton.instance;
        _dailyQuestReroll = DailyQuestReroll.instance;
        _weeklyQuestReroll = WeeklyQuestReroll.instance;

        _dailyQuestsManager = DailyQuestsManager.instance;
        _weeklyQuestsManager = WeeklyQuestsManager.instance;
    }

    public void Start()
    { 
        CheckToRegenerate();
        CheckToRerollQuests();    
    }

    public void RefreshQuestList() 
    { 
        _gameObject = GameObject.Find("UIManager/Safe Zone/Shop Panel/Wrapper/Quests/Quests");
        _questsList = _gameObject.GetComponent<QuestsList>().lastScrollListData;    
    }
    public void Update()
    {
        if (!questsChecking)
        {
            questsChecking = true;
            CheckQuests();
        }
    }

    private void CheckQuests()
    {
        if (Plugin.Settings.DisableAutoClaim.Value)
        {
            questsChecking = false;
            return;
        }

        RefreshQuestList();
        if (_questsList != null && _questsList.Count > 0)
        {
            foreach (Quest quest in _questsList)
            {
                if (quest.CanBeClaimed())
                {
                    quest.Claim();
                    CheckToRegenerate();
                }
            }
        }
        questsChecking = false;
    }

    public void CheckToRegenerate()
    {
        RefreshQuestList();

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

        if (dailyQuestsCount == 0 && CraftableSkills.dailyQuests.IsActive() && Plugin.Settings.ResetDailies.Value)
            _dailyQuestsManager.RegenerateDailys();

        if (weeklyQuestsCount == 0 && CraftableSkills.weeklyQuests.IsActive() && Plugin.Settings.ResetWeeklies.Value)
            _weeklyQuestsManager.RegenerateWeeklies();
        
        RefreshQuestList();
    }

    [HarmonyPatch(typeof(DailyQuest), "Claim")]
    public static class Patch_DailyQuestClaim
    {
        static void Postfix(Quest __instance)
        {
            if (__instance == null) return;
            Enhanced_Quests.Instance?.Invoke(
                nameof(Enhanced_Quests.CheckToRegenerate), 2f);
        }
    }

    [HarmonyPatch(typeof(WeeklyQuest), "Claim")]
    public class Patch_WeeklyQuestClaim
    {
        static void Postfix(Quest __instance)
        {
            if (__instance == null) return;
            Enhanced_Quests.Instance?.Invoke(
                nameof(Enhanced_Quests.CheckToRegenerate),2f);
        }
    }

    [HarmonyPatch(typeof(DailyQuestReroll), "RewardForShowing")]
    public class Patch_DailyQuestRerollReward
    {
        static void Postfix()
        {
            Enhanced_Quests.Instance?.CheckToRerollQuests();
        }
    }

    [HarmonyPatch(typeof(WeeklyQuestReroll), "RewardForShowing")]
    public class Patch_WeeklyQuestRerollReward
    {
        static void Postfix()
        {
            Enhanced_Quests.Instance?.CheckToRerollQuests();
        }
    }

    [HarmonyPatch(typeof(Button), "Press")]
    public class Patch_PortalButton
    {
        static void Postfix(Button __instance)
        {
            if (__instance.name != "Portal Button" && __instance.name != "Portal Button(Clone)") return;
            Enhanced_Quests.Instance?.CheckToResetPortal();
        }
    }

    public void CheckToResetPortal()
    {
        if (_portalButton.currentCd > 0 && Plugin.Settings.ResetPortal.Value)
            _portalButton.currentCd = 0;
    }

    public void CheckToRerollQuests()
    {
        if (Plugin.Settings.ResetReroll.Value)
        {
            _dailyQuestReroll.rerollEnabled = true;
            _weeklyQuestReroll.rerollEnabled = true;
        }
    }


}
