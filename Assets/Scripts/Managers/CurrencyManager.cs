using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int Currency { get; private set; }

    [SerializeField] Transform _coinCounter;
    [SerializeField] AudioSource _coinSound;
    [SerializeField] float _scaleDuration = 0.3f;
    [SerializeField] Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1);

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
        CurrencyAddAnimation();
        _coinSound.Play();
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

    void CurrencyAddAnimation()
    {
        StartCoroutine(ScaleUpAndDown());
    }

    IEnumerator ScaleUpAndDown()
    {
        Vector3 originalScale = Vector3.one;

        // Scale up
        float timer = 0f;
        while (timer < _scaleDuration)
        {
            timer += Time.deltaTime;
            _coinCounter.localScale = Vector3.Lerp(originalScale, _targetScale, timer / _scaleDuration);
            yield return null;
        }

        // Scale down
        timer = 0f;
        while (timer < _scaleDuration)
        {
            timer += Time.deltaTime;
            _coinCounter.localScale = Vector3.Lerp(_targetScale, originalScale, timer / _scaleDuration);
            yield return null;
        }
    }
}
