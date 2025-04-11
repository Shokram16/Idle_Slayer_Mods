using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace Enhanced_Drop_Rate;

public class Enhanced_Drop_Rate : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    public static Il2CppReferenceArray<DropObject> _dropClones;

    public void Awake()
    {
        _playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
    }

    public void Start()
    {
        if (!Plugin.Settings.UncapMaterials.Value) return;

        _dropClones = (Il2CppReferenceArray<DropObject>)Resources.FindObjectsOfTypeAll<DropObject>();
        foreach (var drop in _dropClones)
        {
            if (drop.name == "Drop(Clone)" && drop.gameObject.activeInHierarchy && drop.drop.maxAmount > 0)
            {
                drop.drop.maxAmount = 0;
                drop.drop.amount -= 1;
                drop.drop.AddDrop(1);

                drop.buyMoreButton.image.enabled = false;
            }
        }
    }

    public void Update()
    {
        if ((_playerInventory.dropChance < 100 || _playerInventory.giantsDropChance < 100) && Plugin.Settings.MaxDropRate.Value)
        {
            _playerInventory.dropChance = 100;
            _playerInventory.giantsDropChance = 100;
        }

        if ((_playerInventory.armoryExcellentModifier < 100 ||
            _playerInventory.armoryOptionsModifier < 100 ||
            _playerInventory.armoryItemAscendingHeightsChance < 100 ||
            _playerInventory.increaseBonusStageArmoryChestsChance < 100) &&
            Plugin.Settings.MaxArmoryDropRate.Value)
        {
            _playerInventory.armoryExcellentModifier = 100;
            _playerInventory.armoryOptionsModifier = 100;
            _playerInventory.armoryItemAscendingHeightsChance = 100;
            _playerInventory.increaseBonusStageArmoryChestsChance = 100;
        }

        if (_playerInventory.increaseChestHuntArmoryChestsChance < 100 && Plugin.Settings.IncreaseChestInAChestChance.Value > 0)
        {
            _playerInventory.increaseChestHuntArmoryChestsChance = Plugin.Settings.IncreaseChestInAChestChance.Value;
        }
    }
}


