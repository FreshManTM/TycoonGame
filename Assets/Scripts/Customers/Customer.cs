using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public float RentDuration;
    public int RentFee;

    public Customer(float rentDuration, int rentFee)
    {
        RentDuration = rentDuration;
        RentFee = rentFee;
    }
}
