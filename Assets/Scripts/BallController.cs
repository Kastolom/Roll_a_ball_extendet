using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    [SerializeField] private Vector3 movementInput; // вектор направления движения
    [SerializeField] private float jumpForce = 5f; // сила прыжка

    private Rigidbody rb; // компонент Rigidbody шара
    private bool isGrounded; // флаг, находится ли шар на земле

    void Start()
    {
        Application.targetFrameRate = 120; // устанавливаем частоту кадров
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
    }
}
