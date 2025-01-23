using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _spawnInterval = 5f;

    ObjectPool _pool;
    
    private void Start()
    {
        _pool = ObjectPool.Instance;
        InvokeRepeating(nameof(SpawnCustomer), 1, _spawnInterval);
    }

    void SpawnCustomer()
    {
        var customerInstance = _pool.Spawn(_customerPrefab, _spawnPoint.position, Quaternion.identity, transform);

        var customerAI = customerInstance.GetComponent<CustomerAI>();
        if (customerAI != null)
        {
            int rentDuration = Random.Range(1, 5);
            var customer = new Customer(rentDuration);

            var buildingSpot = BuildingSpotManager.Instance.GetRandomSpot();
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
