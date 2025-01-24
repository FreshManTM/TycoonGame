using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currency_Text;
    [SerializeField] GameObject _tutorialPanel;

    CurrencyManager _currencyManager;

    private void Start()
    {
        _currencyManager = CurrencyManager.Instance;
        if (!PlayerPrefs.HasKey("FirstEntering"))
        {
            ChangeTimeScale(0);
            _tutorialPanel.SetActive(true);
            PlayerPrefs.SetInt("FirstEntering", 1);
        }
    }

    private void Update()
    {
        _currency_Text.text = _currencyManager.Currency.ToString();
    }

    public void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
