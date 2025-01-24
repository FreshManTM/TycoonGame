using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCraftSystem : MonoBehaviour
{
    public static InventoryCraftSystem Instance;

    [SerializeField] InventoryItemUI[] _inventoryItemsUI;
    [SerializeField] RectTransform _inventoryIcon;

    Saver _saver;
    List<InventoryItem> _inventory = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;
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

        UpdateUI(itemName);
        StartCoroutine(ShakeIcon());
        SaveInventory();
    }

    public void AddRandomItem()
    {
        string[] allItems = new string[3] { "Engine", "Body", "Wheel"};
        string item = allItems[Random.Range(0, allItems.Length)];
        int quantity = 1;
        if(item == "Wheel")
            quantity = Random.Range(2, 5);
        AddItem(item, quantity);
    }

    public bool RemoveItem(string itemName, int quantity)
    {
        var item = _inventory.Find(i => i.ItemName == itemName);
        if (item != null && item.Quantity >= quantity)
        {
            item.Quantity -= quantity;

            UpdateUI(itemName);

            SaveInventory();

            return true;
        }

        Debug.Log($"Not enough {itemName} in inventory or item not found.");
        return false;
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
            if (GetItemQuantity(part.Key) < part.Value)
            {
                Debug.Log($"Not enough {part.Key} to craft a car.");
                return false;
            }
        }

        foreach (var part in requiredParts)
        {
            RemoveItem(part.Key, part.Value);
        }
        Debug.Log("Car crafted successfully!");
        return true;
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
    IEnumerator ShakeIcon()
    {
        float shakeDuration = 1f;
        float shakeAngle = 10f;

        Quaternion originalRotation = Quaternion.Euler(Vector3.zero);
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float dampenedAngle = shakeAngle * (1f - (elapsedTime / shakeDuration));
            float zRotation = Random.Range(-dampenedAngle, dampenedAngle);

            _inventoryIcon.localRotation = Quaternion.Euler(0, 0, zRotation);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _inventoryIcon.localRotation = originalRotation;
    }
    void SaveInventory()
    {
        PlayerData data = _saver.LoadInfo();
        data.InventoryItems = _inventory;
        _saver.SaveInfo(data);
    }
}
