using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int Currency { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        SaveCurrency();
    }

    public bool SpendCurrency(int amount)
    {
        if (Currency >= amount)
        {
            Currency -= amount;
            SaveCurrency();
            return true;
        }
        return false;
    }

    void SaveCurrency()
    {
        PlayerData playerData = Saver.Instance.LoadInfo();
        playerData.Currency = Currency;
        Saver.Instance.SaveInfo(playerData);
    }
}
