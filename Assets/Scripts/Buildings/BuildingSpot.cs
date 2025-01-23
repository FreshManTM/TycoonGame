using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSpot : MonoBehaviour
{
    #region PrivateProperties
    [SerializeField] int _spotID;
    [SerializeField] GameObject _carPrefab;
    [SerializeField] int _maxCars = 5; 
    [SerializeField] int _rentFeePerSecond = 1; 
    [SerializeField] Transform[] _carSpawnPoints;
    [SerializeField] Transform _rentPoint;
    [SerializeField] GameObject _lockCover;
    [SerializeField] int _unlockCost = 1000;
    [SerializeField] int _upgradeCost = 1000; 
    [SerializeField] bool _isUnlocked = false;
    #endregion

    #region PublicProperties
    public bool IsUnlocked => _isUnlocked;
    public Transform RentalPoint => _rentPoint;
    public bool HasSpace => _availableCars.Count + _rentedCars.Count < _maxCars;
    public bool HasAvailableCars => _availableCars.Count > 0;
    public Transform ExitPoint { get; private set; }
    public int RentFeePerSecond => _rentFeePerSecond;
    public int UnlockCost => _unlockCost;
    public int UpgradeCost => _upgradeCost;
    #endregion

    Queue<GameObject> _availableCars = new Queue<GameObject>();
    List<GameObject> _rentedCars = new List<GameObject>();

    ObjectPool _pool;
    Saver _saver;

    private void Start()
    {
        _pool = ObjectPool.Instance;
        _saver = Saver.Instance;
        if (!LoadBuildingSpotData() && _spotID == 0)
        {
            _isUnlocked = true;
            _lockCover.SetActive(false);
            BuildingSpotManager.Instance.AddNewBuildingSpot(this);
            AddCar();
            SaveBuildingSpotData();
        }
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            print("clicked");
            BuildingSpotManager.Instance.ShowUI(this);
        }
    }

    public void SetExitPoint(Transform point)
    {
        ExitPoint = point;
    }

    public bool Unlock()
    {
        if (!_isUnlocked && CurrencyManager.Instance.Currency >= _unlockCost)
        {
            CurrencyManager.Instance.SpendCurrency(_unlockCost);
            _isUnlocked = true;
            _lockCover.SetActive(false);
            BuildingSpotManager.Instance.AddNewBuildingSpot(this);

            AddCar();
            SaveBuildingSpotData() ;
            Debug.Log($"Building spot {_spotID} unlocked!");

            return true;
        }
        else
        {
            Debug.LogWarning("Not enough currency or spot already unlocked!");
            return false;
        }
    }

    public bool Upgrade()
    {
        if(CurrencyManager.Instance.Currency >= _upgradeCost)
        {
            CurrencyManager.Instance.SpendCurrency(_upgradeCost);
            _rentFeePerSecond += 1;
            _upgradeCost += 500;
            SaveBuildingSpotData();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough currency to upgrade");
            return false;
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
        foreach (var spotData in _saver.LoadInfo().BuildingSpotsData)
        {
            if (spotData.SpotID == _spotID)
            {
                _upgradeCost = spotData.UpgradeCost;
                _rentFeePerSecond = spotData.RentFeePerSecond;
                _isUnlocked = spotData.IsUnlocked;

                if (_isUnlocked)
                {
                    _lockCover.SetActive(false);
                    BuildingSpotManager.Instance.AddNewBuildingSpot(this);
                }

                if (spotData.AvailableCars > _availableCars.Count)
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
            UpgradeCost = _upgradeCost,
            RentFeePerSecond = _rentFeePerSecond,
            IsUnlocked = _isUnlocked
        };
        playerData.BuildingSpotsData.Add(spotData);
        _saver.SaveInfo(playerData);

    }
}
