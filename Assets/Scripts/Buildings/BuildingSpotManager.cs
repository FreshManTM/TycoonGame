using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpotManager : MonoBehaviour
{
    public static BuildingSpotManager Instance;
    public List<BuildingSpot> GetAllBuildingSpots() => _buildingSpots;

    [SerializeField] GameObject _buildingSpotPrefab;
    [SerializeField] List<BuildingSpot> _buildingSpots = new List<BuildingSpot>();

    private void Awake()
    {
        Instance = this;
    }


    public BuildingSpot GetRandomSpot()
    {
        return _buildingSpots[Random.Range(0, _buildingSpots.Count)];
    }

    public BuildingSpot FindSpotForCar()
    {
        foreach (var spot in _buildingSpots)
        {
            if (spot.HasSpace)
            {
                return spot;
            }
        }
        return null;
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
