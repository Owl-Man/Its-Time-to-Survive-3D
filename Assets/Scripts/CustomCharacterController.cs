using UnityEngine;


public class CustomCharacterController : MonoBehaviour
{
    [SerializeField] private Animator _animation;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private FixedJoystick _joystick;

    [SerializeField] private bool PC_Mode;

    public float jumpForce = 3.5f;
    public float walkingSpeed = 2f;
    public float runningSpeed = 6f;
    public float currentSpeed;
    private float animationInterpolation = 1f;


    private void Start()
    {
        // ����������� ������ � �������� ������
        Cursor.lockState = CursorLockMode.Locked;
        // � ������ ��� ���������
        Cursor.visible = false;
    }

    private void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);

        if (PC_Mode)
        {
            _animation.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
            _animation.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
        }
        else
        {
            _animation.SetFloat("x", _joystick.Horizontal * animationInterpolation);
            _animation.SetFloat("y", _joystick.Vertical * animationInterpolation);
        }

        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }

    private void Walk()
    {
        // Mathf.Lerp - ������� �� ��, ����� ������ ���� ����� animationInterpolation(� ������ ������) ������������ � ����� 1 �� ��������� Time.deltaTime * 3.
        // Time.deltaTime - ��� ����� ����� ���� ������ � ���������� ������. ��� ��������� ������ ���������� � ������ ����� �� ������� ���������� �� ������ � ������� (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);

        if (PC_Mode) 
        {
            _animation.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
            _animation.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
        }
        else 
        {
            _animation.SetFloat("x", _joystick.Horizontal * animationInterpolation);
            _animation.SetFloat("y", _joystick.Vertical * animationInterpolation);
        }

        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
    }

    private void Update()
    {
        // ������������� ������� ��������� ����� ������ �������������� 
        if (PC_Mode) 
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _cameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else 
        {
            if  (_joystick.Horizontal != 0 || _joystick.Vertical != 0) 
            {
                //transform.rotation = Quaternion.LookRotation(new Vector3(_joystick.Horizontal * currentSpeed, _rigidbody.velocity.y, _joystick.Vertical * currentSpeed));
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _cameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }

        // ������ �� ������ W � Shift?
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            // ������ �� ��� ������ A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // ���� ��, �� �� ���� ������
                Walk();
            }
            // ���� ���, �� ����� �����!
            else
            {
                Run();
            }
        }
        // ���� W & Shift �� ������, �� �� ������ ���� ������
        else
        {
            Walk();
        }
        //���� ����� ������, �� � ��������� ���������� ��������� �������, ������� ���������� �������� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animation.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        // ����� �� ������ �������� ��������� � ����������� �� ����������� � ������� ������� ������
        // ��������� ����������� ������ � ������ �� ������ 
        Vector3 camF = _cameraTransform.forward;
        Vector3 camR = _cameraTransform.right;
        // ����� ����������� ������ � ������ �� �������� �� ���� ������� �� ������ ����� ��� ����, ����� ����� �� ������� ������, �������� ����� ���� ������� ��� ����� ������� ����� ��� ����
        // ������ ���� ��������� ��� ����� ����� camF.y = 0 � camR.y = 0 :)
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        // ��� �� �������� ���� ������� �� ������ W & S �� ����������� ������ ������ � ���������� � �������� �� ������ A & D � �������� �� ����������� ������ ������
        
        if (PC_Mode) 
        {
            movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
        }
        else 
        {
            movingVector = Vector3.ClampMagnitude(camF.normalized * _joystick.Vertical * currentSpeed + camR.normalized * _joystick.Horizontal * currentSpeed, currentSpeed);
        }

        // Magnitude - ��� ������ �������. � ���� ������ �� currentSpeed ��� ��� �� �������� ���� ������ �� currentSpeed �� 86 ������. � ���� �������� ����� �������� 1.
        _animation.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
        // ����� �� ������� ���������! ������������� �������� ������ �� x & z ������ ��� �� �� ����� ����� ��� �������� ������� � ������
        _rigidbody.velocity = new Vector3(movingVector.x, _rigidbody.velocity.y, movingVector.z);
        // � ���� ��� ���, ��� �������� �������� �� ����� � ��� �������� � ������� ���� ������
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    public void OnJumpButtonClick() => _animation.SetTrigger("Jump");

    public void Jump()
    {
        // ��������� ������ �� ������� ��������.
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}