using IdleSlayerMods.Common.Config;
using MelonLoader;

namespace Enhanced_Divinities;

internal sealed class Settings(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<int> MinionLevels;

    protected override void SetBindings()
    {
        MinionLevels = Bind("MinionLevels", 0,
            "Minion level on prestige, if '0' it will be minion max level");
    }
}