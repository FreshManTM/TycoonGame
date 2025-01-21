using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpotManager : MonoBehaviour
{
    public static BuildingSpotManager Instance;

    [SerializeField] GameObject _buildingSpotPrefab;
    [SerializeField] List<BuildingSpot> _buildingSpots = new List<BuildingSpot>();

    private void Awake()
    {
        Instance = this;
    }

    public BuildingSpot GetAvailableSpot()
    {
        foreach (var spot in _buildingSpots)
        {
            if (spot.HasAvailableCars)
            {
                return spot;
            }
        }
        return null; // No available spot with cars
    }

    public BuildingSpot FindSpotForCar(GameObject car)
    {
        foreach (var spot in _buildingSpots)
        {
            if (spot.HasSpace || spot.HasAvailableCars)
            {
                return spot;
            }
        }
        return null; // No spots found
    }

    public BuildingSpot AddNewBuildingSpot(Vector3 position)
    {
        var newSpot = Instantiate(_buildingSpotPrefab, position, Quaternion.identity);
        var spotComponent = newSpot.GetComponent<BuildingSpot>();
        if (spotComponent != null)
        {
            _buildingSpots.Add(spotComponent);
        }
        return spotComponent;
    }
}
