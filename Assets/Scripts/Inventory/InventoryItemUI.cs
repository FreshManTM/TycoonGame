using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemUI: MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] TextMeshProUGUI _quantity_Text;


    public void SetQuantity(int quantity)
    {
        _quantity_Text.text = quantity.ToString();
    }
    public string GetName()
    {
        return _name;
    }
}
