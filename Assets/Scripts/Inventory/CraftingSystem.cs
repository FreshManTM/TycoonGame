using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] BuildingSpotManager _buildingSpotManager;

    InventorySystem _inventorySystem;
    private void Start()
    {
        _inventorySystem = GetComponent<InventorySystem>();
    }

    public bool CraftCar()
    {
        var requiredParts = new Dictionary<string, int>
        {
            { "Engine", 1 },
            { "Body", 1 },
            { "Wheel", 4 }
        };

        foreach (var part in requiredParts)
        {
            if (_inventorySystem.GetItemQuantity(part.Key) < part.Value)
            {
                Debug.Log($"Not enough {part.Key} to craft a car.");
                return false;
            }
        }

        foreach (var part in requiredParts)
        {
            _inventorySystem.RemoveItem(part.Key, part.Value);
        }

        var spotForCar = _buildingSpotManager.FindSpotForCar();
        spotForCar.AddCar();
        spotForCar.SaveBuildingSpotData();
        Debug.Log("Car crafted successfully!");
        return true;
    }
}
