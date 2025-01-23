using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpotUI : MonoBehaviour
{
    [SerializeField] GameObject _bottomBar;
    [SerializeField] GameObject _unlockButton;
    [SerializeField] GameObject _upgradeButton;
    [SerializeField] GameObject _addCarButton;
    [SerializeField] GameObject _selectedLight;
    BuildingSpot _currentSpot;
    public void Initialize(BuildingSpot buildingSpot)
    {
        CloseButton();
        _currentSpot = buildingSpot;
        _selectedLight.transform.position = buildingSpot.transform.position;
        _selectedLight.SetActive(true);
        if (!_currentSpot.IsUnlocked)
        {
            _unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Unlock {_currentSpot.UnlockCost}$";
            _unlockButton.SetActive(true);
        }
        else
        {
            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade {_currentSpot.UpgradeCost}$";
            _upgradeButton.SetActive(true);
            _addCarButton.SetActive(true);
        }
        _bottomBar.SetActive(true);

    }

    public void CloseButton()
    {
        _currentSpot = null;
        _selectedLight.SetActive(false);
        _bottomBar.SetActive(false);

        _unlockButton.SetActive(false);
        _upgradeButton.SetActive(false);
        _addCarButton.SetActive(false);
    }

    public void UnlockButton()
    {
        if (_currentSpot.Unlock())
        {
            _unlockButton.SetActive(false);
            _upgradeButton.SetActive(true);
            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade {_currentSpot.UpgradeCost}$";
            _addCarButton.SetActive(true);
        }
    }

    public void UpgradeButton()
    {
        _currentSpot.Upgrade();
        _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade {_currentSpot.UpgradeCost}$";
    }

}
