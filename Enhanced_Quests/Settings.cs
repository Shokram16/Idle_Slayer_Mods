﻿using IdleSlayerMods.Common.Config;
using MelonLoader;

namespace Enhanced_Quests;

internal sealed class Settings(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> ResetDailies;
    internal MelonPreferences_Entry<bool> ResetWeeklies;
    internal MelonPreferences_Entry<bool> ResetPortal;
    internal MelonPreferences_Entry<bool> ResetReroll;
    internal MelonPreferences_Entry<bool> DisableAutoClaim;

    protected override void SetBindings()
    {
        ResetDailies = Bind("ResetDailies", true,
            "Toggle Reset Dailies");
        ResetWeeklies = Bind("ResetWeeklies", true,
            "Toggle Reset Weeklies");
        ResetPortal = Bind("ResetPortal", false,
            "Toggle Reset Portal");
        ResetReroll = Bind("ResetReroll", false,
                "Toggle Reset Quest Reroll");
        DisableAutoClaim = Bind("DisableAutoClaim", false,
                "Disable auto-claiming of quest rewards");
    }
}
