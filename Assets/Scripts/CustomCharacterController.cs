using UnityEngine;


public class CustomCharacterController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Transform mainCamera;

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
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }

    private void Walk()
    {
        // Mathf.Lerp - ������� �� ��, ����� ������ ���� ����� animationInterpolation(� ������ ������) ������������ � ����� 1 �� ��������� Time.deltaTime * 3.
        // Time.deltaTime - ��� ����� ����� ���� ������ � ���������� ������. ��� ��������� ������ ���������� � ������ ����� �� ������� ���������� �� ������ � ������� (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
    }

    private void Update()
    {
        // ������������� ������� ��������� ����� ������ �������������� 
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

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
            anim.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        // ����� �� ������ �������� ��������� � ����������� �� ����������� � ������� ������� ������
        // ��������� ����������� ������ � ������ �� ������ 
        Vector3 camF = mainCamera.forward;
        Vector3 camR = mainCamera.right;
        // ����� ����������� ������ � ������ �� �������� �� ���� ������� �� ������ ����� ��� ����, ����� ����� �� ������� ������, �������� ����� ���� ������� ��� ����� ������� ����� ��� ����
        // ������ ���� ��������� ��� ����� ����� camF.y = 0 � camR.y = 0 :)
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        // ��� �� �������� ���� ������� �� ������ W & S �� ����������� ������ ������ � ���������� � �������� �� ������ A & D � �������� �� ����������� ������ ������
        movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
        // Magnitude - ��� ������ �������. � ���� ������ �� currentSpeed ��� ��� �� �������� ���� ������ �� currentSpeed �� 86 ������. � ���� �������� ����� �������� 1.
        anim.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
        Debug.Log(movingVector.magnitude / currentSpeed);
        // ����� �� ������� ���������! ������������� �������� ������ �� x & z ������ ��� �� �� ����� ����� ��� �������� ������� � ������
        rig.velocity = new Vector3(movingVector.x, rig.velocity.y, movingVector.z);
        // � ���� ��� ���, ��� �������� �������� �� ����� � ��� �������� � ������� ���� ������
        rig.angularVelocity = Vector3.zero;
    }

    public void Jump()
    {
        // ��������� ������ �� ������� ��������.
        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}