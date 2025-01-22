using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _model;
    Customer _customerData;
    BuildingSpot _assignedSpot;
    GameObject _rentedCar;
    Transform _rentalPoint;
    Transform _exitPoint;

    enum CustomerState { MovingToRental, Renting, Leaving }
    CustomerState _currentState = CustomerState.MovingToRental;
    float _pointTriggerDelay = .5f;

    public void Initialize(Customer customerData, BuildingSpot assignedSpot)
    {
        _customerData = customerData;
        _assignedSpot = assignedSpot;
        _rentalPoint = _assignedSpot.RentalPoint;
        _exitPoint = _assignedSpot.ExitPoint;
        _currentState = CustomerState.MovingToRental;

        MoveTo(_rentalPoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Point")
        {
            if (_currentState == CustomerState.MovingToRental)
            {
                _animator.SetBool("Walk", false);
                Invoke(nameof(RentCar), _pointTriggerDelay);
            }
            else if (_currentState == CustomerState.Leaving)
            {
                _animator.SetBool("Walk", false);
                Invoke(nameof(DespawnCustomer), _pointTriggerDelay);
            }
        }
    }

    void MoveTo(Vector3 destination)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(destination);
            _animator.SetBool("Walk", true);
        }
    }

    void RentCar()
    {
        if (_assignedSpot != null)
        {
            _rentedCar = _assignedSpot.RentCar();
            if (_rentedCar != null)
            {
                CurrencyManager.Instance.AddCurrency(_customerData.RentDuration * _assignedSpot.RentFeePerSecond);

                _currentState = CustomerState.Renting;
                _navMeshAgent.enabled = false;
                _model.SetActive(false);
                Invoke(nameof(ReturnCar), _customerData.RentDuration);
            }
            else
            {
                Debug.Log("No cars available at the assigned building spot!");
                MoveTo(_exitPoint.position);
                _currentState = CustomerState.Leaving;
            }
        }
        else
        {
            Debug.LogWarning("Assigned building spot is null!");
            MoveTo(_exitPoint.position);
            _currentState = CustomerState.Leaving;
        }
    }

    void ReturnCar()
    {
        if (_assignedSpot != null && _rentedCar != null)
        {
            _assignedSpot.ReturnCar(_rentedCar);
        }

        _navMeshAgent.enabled = true;
        _model.SetActive(true);
        _rentedCar = null;

        _currentState = CustomerState.Leaving;
        MoveTo(_exitPoint.position);
    }


    void DespawnCustomer()
    {
        ObjectPool.Instance.Despawn(gameObject);

    }
}
