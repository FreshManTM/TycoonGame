using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] GameObject _model;


    Customer _customerData;
    BuildingSpot _assignedSpot;
    GameObject _rentedCar;
    Transform _rentalPoint;
    Transform _exitPoint;

    enum CustomerState { MovingToRental, Renting, Leaving }
    CustomerState _currentState = CustomerState.MovingToRental;


    public void Initialize(Customer customerData, Transform rentalPoint, Transform exitPoint, BuildingSpot assignedSpot)
    {
        _customerData = customerData;
        _assignedSpot = assignedSpot;
        _rentalPoint = rentalPoint;
        _exitPoint = exitPoint;
        _currentState = CustomerState.MovingToRental;

        MoveTo(_rentalPoint.position);
    }

    private void Update()
    {
        if (_currentState == CustomerState.MovingToRental && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            RentCar();
        }
        else if (_currentState == CustomerState.Leaving && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            ObjectPool.Instance.Despawn(gameObject);
        }
    }

    private void MoveTo(Vector3 destination)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(destination);
        }
    }

    void RentCar()
    {
        var availableSpot = BuildingSpotManager.Instance.GetAvailableSpot();
        if (availableSpot != null)
        {
            print("renting + " + availableSpot);
            _rentedCar = availableSpot.RentCar();
            if (_rentedCar != null)
            {
                CurrencyManager.Instance.AddCurrency(_customerData.RentDuration * availableSpot.RentFeePerSecond); // Add rent fee to player's balance

                _currentState = CustomerState.Renting;
                _navMeshAgent.enabled = false;
                _model.SetActive(false);
                Invoke(nameof(ReturnCar), _customerData.RentDuration); 
            }
        }
        else
        {
            Debug.Log("No parking spots with available cars!");
            _currentState = CustomerState.Leaving;
            MoveTo(_exitPoint.position);
        }
    }

    void ReturnCar()
    {
        _assignedSpot.ReturnCar(_rentedCar);
        _navMeshAgent.enabled = true;
        _model.SetActive(true);
        _rentedCar = null;
        _currentState = CustomerState.Leaving;
        MoveTo(_exitPoint.position);
    }

}
