using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Currency;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public List<BuildingSpotData> BuildingSpotsData = new List<BuildingSpotData>();

}
