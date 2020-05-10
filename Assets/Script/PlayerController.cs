using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 6f; 

    [SerializeField]
    private float _lookSensibilityX = 3f;

    [SerializeField]
    private float _lookSensibilityY = 3f;

    [SerializeField]
    private float _thrusterForce = 1000f;

    [SerializeField] private float _thrusterFuelBurnSpeed = 1f;

    [SerializeField] private float _thrusterFuelRegenSpeed = 0.3f;

    public float ThrusterFuelAmount { get; private set; }= 1f;


    [SerializeField] private LayerMask _environnementMask;

    [Header("Spring Settings")] 
    
    [SerializeField]
    private float _jointSpring = 20f;
    [SerializeField]
    private float _jointMaxForce = 40f;

    private PlayerMotor _motor;

    private ConfigurableJoint _joint;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _motor = GetComponent<PlayerMotor>();
        _joint = GetComponent<ConfigurableJoint>();
        _animator = GetComponent<Animator>();
        SetJointSettings(_jointSpring);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            _motor.Move(Vector3.zero);
            _motor.Rotation(Vector3.zero);
            _motor.RotationCamera(0);
            _motor.ApplyThruster(Vector3.zero);

            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, _environnementMask))
        {
            _joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
        }
        else
        {
            _joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _speed;
        _animator.SetFloat("ForwardVelocity", zMov);
        _motor.Move(velocity);

        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * _lookSensibilityX;

        _motor.Rotation(rotation);

        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * _lookSensibilityY;

        _motor.RotationCamera(cameraRotationX);

        Vector3 thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && ThrusterFuelAmount > 0f)
        {
            ThrusterFuelAmount -= _thrusterFuelBurnSpeed * Time.deltaTime;

            if (ThrusterFuelAmount >= 0.01f)
            {
                thrusterForce = Vector3.up * _thrusterForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            ThrusterFuelAmount += _thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(_jointSpring);
        }

        ThrusterFuelAmount = Mathf.Clamp(ThrusterFuelAmount, 0, 1);

        _motor.ApplyThruster(thrusterForce);
    }

    private void SetJointSettings(float jointSpring)
    {
        _joint.yDrive = new JointDrive
        {
            maximumForce = _jointMaxForce,
            positionSpring = jointSpring
        };
    }
}
