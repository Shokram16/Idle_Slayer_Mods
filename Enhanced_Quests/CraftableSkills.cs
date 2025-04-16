using Il2Cpp;
using UnityEngine;

namespace Enhanced_Quests;

public class CraftableSkills : MonoBehaviour
{
    public static CraftableSkills Instance { get; private set; }

    private CraftableItems _craftableItems;
    public static PermanentCraftableItem weeklyQuests;
    public static PermanentCraftableItem dailyQuests;

    private void Awake()
    {
        Instance = this;
        _craftableItems = CraftableItems.list;
        weeklyQuests = _craftableItems.WeeklyQuests;
        dailyQuests = _craftableItems.DailyQuests;
    }
    
}