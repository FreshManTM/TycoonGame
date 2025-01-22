using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class BuildingSpot : MonoBehaviour, IBuildingSpot
{
    [SerializeField] int _spotID;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] int _maxCars = 5; 
    [SerializeField] int _rentFeePerSecond = 1; 
    [SerializeField] Transform[] _carSpawnPoints;
    [SerializeField] Transform _rentPoint;
    [SerializeField] Transform _exitPoint;
    public bool HasSpace => _availableCars.Count + _rentedCars.Count < _maxCars;
    public bool HasAvailableCars => _availableCars.Count > 0;
    public int RentFeePerSecond => _rentFeePerSecond;
    public Transform RentalPoint => _rentPoint;
    public Transform ExitPoint => _exitPoint;

    Queue<GameObject> _availableCars = new Queue<GameObject>();
    List<GameObject> _rentedCars = new List<GameObject>();

    ObjectPool _pool;
    Saver _saver;

    private void Start()
    {
        _pool = ObjectPool.Instance;
        _saver = Saver.Instance;
        if (!LoadBuildingSpotData())
        {
            AddCar();
            SaveBuildingSpotData();
        }
    }

    public void AddCar()
    {
        if (HasSpace)
        {
            if (_carSpawnPoints.Length > _availableCars.Count)
            {
                int spawnIndex = _availableCars.Count;

                var car = _pool.Spawn(_carPrefab, _carSpawnPoints[spawnIndex].position, _carPrefab.transform.rotation);

                _availableCars.Enqueue(car);
            }
            else
            {
                Debug.LogWarning("Not enough spawn points defined for this building spot!");
            }
        }
        else
        {
            Debug.LogWarning("No space available in this building spot!");
        }
    }

    public GameObject RentCar()
    {
        if (HasAvailableCars)
        {
            var car = _availableCars.Dequeue();
            car.SetActive(false);
            _rentedCars.Add(car);
            return car;
        }

        Debug.LogWarning("No available cars to rent!");
        return null;
    }

    public void ReturnCar(GameObject car)
    {
        if (_rentedCars.Contains(car))
        {
            _rentedCars.Remove(car);
            AddCar();
        }
    }
    bool LoadBuildingSpotData()
    {
        foreach(var spotData in _saver.LoadInfo().BuildingSpotsData)
        {
            if(spotData.SpotID == _spotID)
            {
                _rentFeePerSecond = spotData.RentFeePerSecond;
                if(spotData.AvailableCars > _availableCars.Count)
                {
                    var carsToAdd = spotData.AvailableCars - _availableCars.Count;
                    for (int i = 0; i < carsToAdd; i++)
                    {
                        AddCar();
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void SaveBuildingSpotData()
    {
        var playerData = _saver.LoadInfo();
        var curData = playerData.BuildingSpotsData.Find(i => i.SpotID == _spotID);
        if (curData != null)
        {
            playerData.BuildingSpotsData.Remove(curData);
        }

        var spotData = new BuildingSpotData
        {
            SpotID = _spotID,
            AvailableCars = _availableCars.Count + _rentedCars.Count,
            RentFeePerSecond = _rentFeePerSecond
        };
        playerData.BuildingSpotsData.Add(spotData);
        _saver.SaveInfo(playerData);

    }
}
