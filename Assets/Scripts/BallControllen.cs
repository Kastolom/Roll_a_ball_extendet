using UnityEngine;

public class BallControllen : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    [SerializeField] private Vector3 movementInput; // вектор направления движения
    [SerializeField] private float jumpForce = 5f; // сила прыжка

    // НОВЫЕ ПЕРЕМЕННЫЕ
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f; // Сила рывка
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Клавиша для рывка

    private Rigidbody rb; // компонент Rigidbody шара
    private bool isGrounded; // флаг, находится ли шар на земле

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // получаем Rigidbody при старте
    }

    void Update()
    {
        // считываем ввод с клавиатуры/геймпада по осям
        movementInput.x = Input.GetAxis("Horizontal"); // лево-право (A/D, стрелки)
        movementInput.z = Input.GetAxis("Vertical");   // вперед-назад (W/S, стрелки)

        // прыжок по нажатию клавиши пробел
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        // ЛОГИКА РЫВКА
        // Проверяем нажатие клавиши рывка
        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }
    }

    private void Dash()
    {
        // Вычисляем направление рывка. 
        // Если игрок никуда не жмет, рывок пойдет вперед (по оси Z).
        // Если жмет кнопки движения — в сторону движения.
        Vector3 dashDirection = movementInput.normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward; 
        }

        // Прикладываем мгновенную силу
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
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

        // добавил просто комментарий
    }
}
