using IdleSlayerMods.Common.Config;
using MelonLoader;

namespace Enhanced_Drop_Rate;

internal sealed class Settings(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> UncapMaterials;
    internal MelonPreferences_Entry<bool> MaxDropRate;
    internal MelonPreferences_Entry<bool> MaxArmoryDropRate;

    protected override void SetBindings()
    {
        UncapMaterials = Bind("UncapMaterials", true,
            "Toggle Material uncap");
        MaxDropRate = Bind("MaxDropRate", false,
            "Toggle Max Drop Rate");
        MaxArmoryDropRate = Bind("MaxArmoryDropRate", false,
            "Toggle Max Drop Rate");
    }
}