using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private RectTransform _thrusterFuelFill;

    [SerializeField] private RectTransform _healthBarFill;

    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private Text _textAmmo;

    private PlayerController _controller;

    private Player _player;

    private WeaponManager _weaponManager;

    public void SetPlayer(Player player)
    {
        _player = player;
        _controller = player.GetComponent<PlayerController>();
        _weaponManager = player.GetComponent<WeaponManager>();
    }
    
    private void Start()
    {
        PauseMenu.isOn = false;
    }

    private void Update()
    {
        SetFuelAmount(_controller.ThrusterFuelAmount);
        SetHealthAmount(_player.GetHealthPourcentage());
        SetAmmoAmount(_weaponManager.CurrentWeapon.Bullets);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TooglePauseMenu();
        }
    }

    public void TooglePauseMenu()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        PauseMenu.isOn = _pauseMenu.activeSelf;
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1f,amount,1f);
    }

    private void SetHealthAmount(float amount)
    {
        _healthBarFill.localScale = new Vector3(1f, amount, 1f);
    }

    private void SetAmmoAmount(int amount)
    {
        _textAmmo.text = Convert.ToString(amount);
    }
}
