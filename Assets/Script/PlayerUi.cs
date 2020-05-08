using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private RectTransform _thrusterFuelFill;

    private PlayerController _controller;

    public void SetController(PlayerController controller)
    {
        _controller = controller;
    }

    private void Update()
    {
        SetFuelAmount(_controller.ThrusterFuelAmount);
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1f,amount,1f);
    }
}
