using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _spawnInterval = 5f;

    Queue<Customer> _customers = new Queue<Customer>();
    ObjectPool _pool;
    
    private void Start()
    {
        _pool = ObjectPool.Instance;
        InvokeRepeating(nameof(SpawnCustomer), 0, _spawnInterval);
    }

    void SpawnCustomer()
    {
        // Create a new customer instance
        var customerInstance = _pool.Spawn(_customerPrefab, _spawnPoint.position, Quaternion.identity, transform);
        print(customerInstance.transform.position);
        // Setup the CustomerAI component on the prefab
        var customerAI = customerInstance.GetComponent<CustomerAI>();
        if (customerAI != null)
        {
            int rentDuration = Random.Range(1, 5);
            var customer = new Customer(rentDuration);
            _customers.Enqueue(customer);

            var buildingSpot = BuildingSpotManager.Instance.GetAvailableSpot();
            if (buildingSpot != null)
            {
                customerAI.Initialize(customer, buildingSpot.RentalPoint, buildingSpot.ExitPoint, buildingSpot);
            }
            else
            {
                Debug.LogWarning("No available building spots for customers!");
                _pool.Despawn(customerInstance); // Return customer to the pool if no spot available
            }
        }
    }

}
