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
        if (Input.GetButton("Jump"))
        {
            thrusterForce = Vector3.up * _thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(_jointSpring);
        }

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
