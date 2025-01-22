using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int Currency { get; private set; }
    Saver _saver;

    private void Awake()
    {
        Instance = this;
             
    }
    private void Start()
    {
        _saver = Saver.Instance;
        Currency = _saver.LoadInfo().Currency;
       
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
        PlayerData playerData = _saver.LoadInfo();
        playerData.Currency = Currency;
        _saver.SaveInfo(playerData);
    }
}
