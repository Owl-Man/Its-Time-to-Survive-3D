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
        // Прекрепляем курсор к середине экрана
        Cursor.lockState = CursorLockMode.Locked;
        // и делаем его невидимым
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
        // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
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
        // Устанавливаем поворот персонажа когда камера поворачивается 
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

        // Зажаты ли кнопки W и Shift?
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            // Зажаты ли еще кнопки A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // Если да, то мы идем пешком
                Walk();
            }
            // Если нет, то тогда бежим!
            else
            {
                Run();
            }
        }
        // Если W & Shift не зажаты, то мы просто идем пешком
        else
        {
            Walk();
        }
        //Если зажат пробел, то в аниматоре отправляем сообщение тригеру, который активирует анимацию прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animation.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        // Здесь мы задаем движение персонажа в зависимости от направления в которое смотрит камера
        // Сохраняем направление вперед и вправо от камеры 
        Vector3 camF = _cameraTransform.forward;
        Vector3 camR = _cameraTransform.right;
        // Чтобы направления вперед и вправо не зависили от того смотрит ли камера вверх или вниз, иначе когда мы смотрим вперед, персонаж будет идти быстрее чем когда смотрит вверх или вниз
        // Можете сами проверить что будет убрав camF.y = 0 и camR.y = 0 :)
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        // Тут мы умножаем наше нажатие на кнопки W & S на направление камеры вперед и прибавляем к нажатиям на кнопки A & D и умножаем на направление камеры вправо
        
        if (PC_Mode) 
        {
            movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
        }
        else 
        {
            movingVector = Vector3.ClampMagnitude(camF.normalized * _joystick.Vertical * currentSpeed + camR.normalized * _joystick.Horizontal * currentSpeed, currentSpeed);
        }

        // Magnitude - это длинна вектора. я делю длинну на currentSpeed так как мы умножаем этот вектор на currentSpeed на 86 строке. Я хочу получить число максимум 1.
        _animation.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
        // Здесь мы двигаем персонажа! Устанавливаем движение только по x & z потому что мы не хотим чтобы наш персонаж взлетал в воздух
        _rigidbody.velocity = new Vector3(movingVector.x, _rigidbody.velocity.y, movingVector.z);
        // У меня был баг, что персонаж крутился на месте и это исправил с помощью этой строки
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    public void OnJumpButtonClick() => _animation.SetTrigger("Jump");

    public void Jump()
    {
        // Выполняем прыжок по команде анимации.
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}