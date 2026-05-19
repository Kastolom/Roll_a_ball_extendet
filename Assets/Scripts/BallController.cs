using UnityEngine;

public class BallController : MonoBehaviour
{
    // Сделаем экземпляр класса шара статическим, потому что он будет в единственном экземпляре и нам будет удобно к нему обращаться из других классов
    public static BallController instance; // статический экземпляр класса BallController
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    [SerializeField] private Vector3 movementInput; // вектор направления движения
    [SerializeField] private float jumpForce = 5f; // сила прыжка

    // НОВЫЕ ПЕРЕМЕННЫЕ
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f; // Сила рывка
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Клавиша для рывка

    [Header("Lose Settings")]
    [SerializeField] private float loseYLevel = -5f;

    private Rigidbody rb; // компонент Rigidbody шара
    private bool isGrounded; // флаг, находится ли шар на земле

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Application.targetFrameRate = 120; // устанавливаем частоту кадров
        rb = GetComponent<Rigidbody>(); // получаем Rigidbody при старте
    }

    void Update()
    {
        if (GameManager.instance.CurrentState != GameState.Playing)
            return;
        // считываем ввод с клавиатуры/геймпада по осям
        movementInput.x = Input.GetAxis("Horizontal"); // лево-право (A/D, стрелки)
        movementInput.z = Input.GetAxis("Vertical");   // вперед-назад (W/S, стрелки)

        // прыжок по нажатию клавиши пробел
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        // Проверяем нажатие клавиши рывка
        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }

         // ПРОИГРЫШ
        if (transform.position.y < loseYLevel)
        {
            GameManager.instance.LoseGame();
        }
    }

    private void Dash()
    {
        // Вычисляем направление рывка. 
        // Если игрок никуда не жмет, то и рывка не будет.
        // Если жмет кнопки движения — в сторону движения.
        Vector3 dashDirection = movementInput.normalized;

        // Прикладываем мгновенную силу
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (GameManager.instance.CurrentState != GameState.Playing)
            return;
        // применяем силу к Rigidbody для физического движения
        rb.AddForce(movementInput * moveSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        // проверяем, что шар коснулся земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // проверяем, что шар оторвался от земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        
    }
}
