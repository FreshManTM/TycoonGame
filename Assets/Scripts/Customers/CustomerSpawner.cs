using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _rentalPoint;
    [SerializeField] Transform _exitPoint;
    [SerializeField] float _spawnInterval = 5f;
    [SerializeField] int _rentFeePerSecond = 2;

    Queue<Customer> _customers = new Queue<Customer>();
    ObjectPool _pool;
    
    private void Start()
    {
        _pool = ObjectPool.Instance;
        InvokeRepeating(nameof(SpawnCustomer), _spawnInterval, _spawnInterval);
    }

    private void SpawnCustomer()
    {
        // Create a new customer instance
        var customerInstance = _pool.Spawn(_customerPrefab, _spawnPoint.position, Quaternion.identity);

        // Setup the CustomerAI component on the prefab
        var customerAI = customerInstance.GetComponent<CustomerAI>();
        if (customerAI != null)
        {
            int rentDuration = Random.Range(10, 30);
            var customer = new Customer(rentDuration, _rentFeePerSecond * rentDuration);
            _customers.Enqueue(customer);

            customerAI.Initialize(customer, _rentalPoint, _exitPoint, OnCarRented);
        }
    }

    private void OnCarRented(Customer customer)
    {
        // Handle the rental process (e.g., deduct car, add money)
        Debug.Log($"Customer rented a car for {customer.RentDuration} seconds, earning ${customer.RentFee}.");
        CurrencyManager.Instance.AddCurrency(customer.RentFee);
    }
}
