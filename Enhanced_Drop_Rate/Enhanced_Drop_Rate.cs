using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace Enhanced_Drop_Rate;

public class Enhanced_Drop_Rate : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    public static Il2CppReferenceArray<DropObject> _dropClones;

    private MapController _mapController;
    private BonusMapController _bonusController;
    private bool _isInBonusGame;
    private double _playerDecreaseRandomBoxCoinsChances;

    public void Awake()
    {
        _playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();

        // Bonus Stage 
        _mapController = GameObject.Find("Map").GetComponent<MapController>();
        _bonusController = GameObject.Find("Bonus Map Controller").GetComponent<BonusMapController>();
        _playerDecreaseRandomBoxCoinsChances = _playerInventory.decreaseRandomBoxCoinsChances;
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
        _isInBonusGame = _mapController.selectedMap.name.Contains("bonus");

        // only do logic in bonus stages
        if ((_isInBonusGame || _bonusController.showCurrentTime )&& Plugin.Settings.DecreaseBonusStageCoinsChance.Value)
            _playerInventory.decreaseRandomBoxCoinsChances = 100;
        else
            _playerInventory.decreaseRandomBoxCoinsChances = _playerDecreaseRandomBoxCoinsChances;

    }
}


