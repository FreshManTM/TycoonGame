using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpot : MonoBehaviour, IBuildingSpot
{
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


    private void Start()
    {
        _pool = ObjectPool.Instance;
        AddCar();
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
}
