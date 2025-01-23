using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InventoryCraftSystem inventorySystem;
    [SerializeField] TextMeshProUGUI _currency_Text;

    CurrencyManager _currencyManager;
    BuildingSpot _selectedBuildSpot;
    private void Start()
    {
        _currencyManager = CurrencyManager.Instance;
    }
    private void Update()
    {
        _currency_Text.text = _currencyManager.Currency.ToString();
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
        inventorySystem.CraftCar();
    }

    public void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void AddCar()
    {
        _selectedBuildSpot.CraftCar();
    }
}
