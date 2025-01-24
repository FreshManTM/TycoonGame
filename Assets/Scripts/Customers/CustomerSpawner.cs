using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _baseSpawnInterval = 3f;

    ObjectPool _pool;
    BuildingSpotManager _buildingSpotManager;
    
    private void Start()
    {
        _pool = ObjectPool.Instance;
        _buildingSpotManager = BuildingSpotManager.Instance;
        StartCoroutine(ISpawnCustomer());
    }

    IEnumerator ISpawnCustomer()
    {

        int buildingSpotsAmount = _buildingSpotManager.BuildingSpotsAmount;
        float adjustedInterval = Mathf.Lerp(_baseSpawnInterval, .5f, (float)buildingSpotsAmount / 6f);
        yield return new WaitForSeconds(2);
        SpawnCustomer();
        StartCoroutine(ISpawnCustomer());
    }

    void SpawnCustomer()
    {
        var customerInstance = _pool.Spawn(_customerPrefab, _spawnPoint.position, Quaternion.identity, transform);

        var customerAI = customerInstance.GetComponent<CustomerAI>();
        if (customerAI != null)
        {
            int rentDuration = Random.Range(3, 11);
            var customer = new Customer(rentDuration);

            var buildingSpot = _buildingSpotManager.GetRandomSpot();
            if (buildingSpot != null)
            {
                customerAI.Initialize(customer, buildingSpot);
            }
            else
            {
                Debug.LogWarning("No available building spots for customers!");
                _pool.Despawn(customerInstance);
            }
        }
    }

}
