using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string ItemName;
    public int Quantity;

    public InventoryItem(string itemName, int quantity)
    {
        ItemName = itemName;
        Quantity = quantity;
    }
}
