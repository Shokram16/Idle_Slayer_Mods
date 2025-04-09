using IdleSlayerMods.Common.Config;
using MelonLoader;

namespace Enhanced_Drop_Rate;

internal sealed class Settings(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> UncapMaterials;
    internal MelonPreferences_Entry<bool> MaxDropRate;
    internal MelonPreferences_Entry<bool> MaxArmoryDropRate;
    internal MelonPreferences_Entry<int> IncreaseChestInAChestChance;

    protected override void SetBindings()
    {
        UncapMaterials = Bind("UncapMaterials", true,
            "Toggle Material uncap");
        MaxDropRate = Bind("MaxDropRate", false,
            "Toggle Max Drop Rate");
        MaxArmoryDropRate = Bind("MaxArmoryDropRate", false,
            "Toggle Max Drop Rate");
        IncreaseChestInAChestChance = Bind("IncreaseChestInAChestChance", 0,
            "Increase Chest in a Chest chance. With '4' you will probably get a chest in every chest hunt game. Its not recommended more than 4");
    }
}