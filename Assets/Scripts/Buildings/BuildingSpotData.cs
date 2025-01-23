using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingSpotData 
{
    public int SpotID; 
    public int AvailableCars;
    public int UpgradeCost;
    public int RentFeePerSecond;
    public bool IsUnlocked;
}
