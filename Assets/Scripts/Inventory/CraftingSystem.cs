using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] BuildingSpotManager _buildingSpotManager;

    InventoryCraftSystem _inventorySystem;
    private void Start()
    {
        _inventorySystem = GetComponent<InventoryCraftSystem>();
    }


}
