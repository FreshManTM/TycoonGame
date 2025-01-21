using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private Customer _customerData;
    private Transform _rentalPoint;
    private Transform _exitPoint;
    private System.Action<Customer> _onCarRentedCallback;

    private enum CustomerState { MovingToRental, Renting, Leaving }
    private CustomerState _currentState = CustomerState.MovingToRental;

    [SerializeField] private NavMeshAgent _navMeshAgent; // NavMeshAgent component for movement

    public void Initialize(Customer customerData, Transform rentalPoint, Transform exitPoint, System.Action<Customer> onCarRentedCallback)
    {
        _customerData = customerData;
        _rentalPoint = rentalPoint;
        _exitPoint = exitPoint;
        _onCarRentedCallback = onCarRentedCallback;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        MoveTo(_rentalPoint.position);
    }

    private void Update()
    {
        if (_currentState == CustomerState.Renting && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            BeginRental();
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

    private void BeginRental()
    {
        _currentState = CustomerState.Renting;
        Invoke(nameof(EndRental), _customerData.RentDuration);
    }

    private void EndRental()
    {
        _onCarRentedCallback?.Invoke(_customerData);
        _currentState = CustomerState.Leaving;
        MoveTo(_exitPoint.position);
    }
}
