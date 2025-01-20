using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InventorySystem inventorySystem;

    private void Start()
    {
    }

    public void AddInventoryItem(string itemName)
    {
        inventorySystem.AddItem(itemName, 1);
    }

    public void RemoveInventoryItem(string itemName)
    {
        inventorySystem.RemoveItem(itemName, 1);

    }
    public void CraftCar()
    {
        inventorySystem.gameObject.GetComponent<CraftingSystem>().CraftCar();
    }
}
