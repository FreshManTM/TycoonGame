using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingSpot
{
    public bool HasSpace { get; }
    public bool HasAvailableCars { get; }
    public int RentFeePerSecond { get; }
    public Transform RentalPoint { get; }
    public Transform ExitPoint { get; }

    public void AddCar();

    public GameObject RentCar();


    public void ReturnCar(GameObject car);

}
