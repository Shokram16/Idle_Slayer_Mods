using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace Enhanced_Drop_Rate;

public class Enhanced_Drop_Rate : MonoBehaviour
{
    private Il2CppReferenceArray<DropObject> _dropClones;
    private PlayerInventory _playerInventory;

    public void Awake()
    {
        _playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
    }

    public void Start()
    {
        _dropClones = (Il2CppReferenceArray<DropObject>)Resources.FindObjectsOfTypeAll<DropObject>();
        foreach (var drop in _dropClones)
        {
            if (drop.name == "Drop(Clone)" && drop.gameObject.activeInHierarchy && drop.drop.maxAmount > 0)
            {
                drop.drop.maxAmount = 0;
                Plugin.Logger.Msg($"Modificado {drop.name}");
                drop.drop.amount -= 2;
                drop.drop.AddDrop(1);
            }
        }
    }

    public void Update()
    {
        if (_playerInventory.dropChance < 100)
            _playerInventory.dropChance = 100;

    }
}


