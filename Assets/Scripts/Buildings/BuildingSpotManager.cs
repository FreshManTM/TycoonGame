using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpotManager : MonoBehaviour
{
    public static BuildingSpotManager Instance;

    [SerializeField] GameObject _buildingSpotPrefab;
    [SerializeField] List<BuildingSpot> _activeBuildingSpots;
    [SerializeField] BuildingSpotUI _buildingSpotUI;
    [SerializeField] Transform _exitPoint;
    private void Awake()
    {
        Instance = this;
    }


    public BuildingSpot GetRandomSpot()
    {
        return _activeBuildingSpots[Random.Range(0, _activeBuildingSpots.Count)];
    }

    public BuildingSpot FindSpotForCar()
    {
        foreach (var spot in _activeBuildingSpots)
        {
            if (spot.HasSpace)
            {
                return spot;
            }
        }
        return null;
    }

    public void AddNewBuildingSpot(BuildingSpot spot)
    {
        spot.SetExitPoint(_exitPoint);
        if (!_activeBuildingSpots.Contains(spot))
        {
            _activeBuildingSpots.Add(spot);
        }
    }
    public void ShowUI(BuildingSpot buildingSpot)
    {
        _buildingSpotUI.Initialize(buildingSpot);
    }
}
