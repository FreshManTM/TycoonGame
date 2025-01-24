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
    [SerializeField] ParticleSystem _confettiParticle;
    [SerializeField] AudioSource _purchaseSound;

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
            _purchaseSound.Play();
            PlayParticle(_currentSpot.transform.position);

            _unlockButton.SetActive(false);
            _upgradeButton.SetActive(true);
            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade {_currentSpot.UpgradeCost}$";
            _addCarButton.SetActive(true);
        }
    }

    public void UpgradeButton()
    {
        if (_currentSpot.Upgrade())
        {
            _purchaseSound.Play();
            PlayParticle(_currentSpot.transform.position);

            _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade {_currentSpot.UpgradeCost}$";
        }
    }

    public void CraftCarButton()
    {
        if (_currentSpot.CraftCar())
        {
            _purchaseSound.Play();
            PlayParticle(_currentSpot.transform.position);
        }
        
    }

    void PlayParticle(Vector3 pos)
    {
        _confettiParticle.gameObject.transform.position = pos;
        _confettiParticle.gameObject.SetActive(true);
        _confettiParticle.Play();
    }
}
