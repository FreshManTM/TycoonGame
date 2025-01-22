using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] InventoryItemUI[] _inventoryItemsUI;

    Saver _saver;
    private List<InventoryItem> _inventory = new List<InventoryItem>();

    private void Awake()
    {
        _saver = Saver.Instance;
    }

    private void Start()
    {
        _inventory = _saver.LoadInfo().InventoryItems;
        if(_inventory.Count == 0)
        {
            foreach (var item in _inventoryItemsUI)
            {
                AddItem(item.GetName(), 0);
            }
        }
        //AddItem("Engine", 1);
        //AddItem("Body", 2);
        //AddItem("Wheel", 3);
        //RemoveItem("Wheel", 2);
        foreach (var item in _inventoryItemsUI)
        {
            item.SetQuantityUI(_inventory.Find(i => i.ItemName == item.GetName()).Quantity);
        }
    }



    public void AddItem(string itemName, int quantity)
    {
        var item = _inventory.Find(i => i.ItemName == itemName);
        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            _inventory.Add(new InventoryItem(itemName, quantity));
        }
        Debug.Log($"{quantity} {itemName}(s) added.");

        UpdateUI(itemName);

        SaveInventory();
    }


    public bool RemoveItem(string itemName, int quantity)
    {
        var item = _inventory.Find(i => i.ItemName == itemName);
        if (item != null && item.Quantity >= quantity)
        {
            item.Quantity -= quantity;

            Debug.Log($"{quantity} {itemName}(s) removed.");

            UpdateUI(itemName);

            SaveInventory();

            return true;
        }

        Debug.Log($"Not enough {itemName} in inventory or item not found.");
        return false;
    }


    public int GetItemQuantity(string itemName)
    {
        var item = _inventory.Find(i => i.ItemName == itemName);
        return item != null ? item.Quantity : 0;
    }

    void UpdateUI(string itemName)
    {
        var item = _inventory.Find(i => i.ItemName == itemName);

        foreach (var itemUI in _inventoryItemsUI)
        {
            if (itemUI.GetName() == item.ItemName)
                itemUI.SetQuantityUI(_inventory.Find(i => i.ItemName == itemUI.GetName()).Quantity);
        }
    }

    void SaveInventory()
    {
        PlayerData data = _saver.LoadInfo();
        data.InventoryItems = _inventory;
        _saver.SaveInfo(data);
    }
}
