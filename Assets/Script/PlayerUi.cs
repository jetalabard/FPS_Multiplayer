using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private RectTransform _thrusterFuelFill;

    [SerializeField] private GameObject _pauseMenu;

    private PlayerController _controller;

    public void SetController(PlayerController controller)
    {
        _controller = controller;
    }

    private void Start()
    {
        PauseMenu.isOn = false;
    }

    private void Update()
    {
        SetFuelAmount(_controller.ThrusterFuelAmount);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TooglePauseMenu();
        }
    }

    private void TooglePauseMenu()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        PauseMenu.isOn = _pauseMenu.activeSelf;
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1f,amount,1f);
    }
}
