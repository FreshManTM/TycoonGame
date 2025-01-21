using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public int RentDuration;

    public Customer(int rentDuration)
    {
        RentDuration = rentDuration;
    }
}
