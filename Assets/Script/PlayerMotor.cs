using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 _velocity;

    private Vector3 _rotation;

    private float _cameraRotationX = 0f;

    private float _currentCameraRotationX = 0f;

    private Vector3 _thrusterForce;

    [SerializeField]
    private float _cameraRotationLimit = 85f;

    [SerializeField]
    private Camera _camera;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformRotation()
    {
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));
        _currentCameraRotationX -= _cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);
        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX,0f,0f);
    }

    private void PerformMovement()
    {
        if (_velocity != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
        }

        if (_thrusterForce != Vector3.zero)
        {
            _rb.AddForce(_thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    internal void Move(Vector3 velocity)
    {
        _velocity = velocity;
    }

    internal void Rotation(Vector3 rotation)
    {
        _rotation = rotation;

    }

    public void RotationCamera(float cameraRotationX)
    {
        _cameraRotationX = cameraRotationX;
    }

    internal void ApplyThruster(Vector3 thrusterForce)
    {
        _thrusterForce = thrusterForce;
    }
}
