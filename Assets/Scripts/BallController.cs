using UnityEngine;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    private Vector3 movementInput; // вектор направления движения
    [SerializeField] private float jumpForce = 5f; // максимальная сила прыжка
    private float jumpForceCurent; // текущее значение силы прыжка
    [SerializeField] private float jumpBarSpeed = 2f; // скорость заполнения полосы прыжка]
    private float jumpBarCurent; // текущее значение полосы прыжка
    [SerializeField] private float dashForce = 10f; // Сила рывка
    private float dashForceCurent; // текущее значение силы рывка
    [SerializeField] private float dashBarSpeed = 2f; // скорость заполнения полосы рывка
    private float dashBarCurent; // текущее значение полосы рывка
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Клавиша для рывка
    [SerializeField] private float loseYLevel = -5f; // уровень Y, при достижении которого игра проиграна
    private List<Collision> curentColiders = new List<Collision>();
    private Rigidbody rb; // компонент Rigidbody шара
    private bool isGrounded; // флаг, находится ли шар на земле
    private GameManager gameManager;

    void OnEnable()
    {
        GameManager.OnGameLose += StopMoving;
    }
    void OnDisable()
    {
        GameManager.OnGameLose -= StopMoving;
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>(); // получаем GameManager из всех объектов в сцене
        rb = GetComponent<Rigidbody>(); // получаем Rigidbody при старте
    }

    void Update()
    {
        if (gameManager.CurrentState != GameState.Playing && gameManager.CurrentState != GameState.Start)
            return;
        // считываем ввод с клавиатуры/геймпада по осям
        movementInput.x = Input.GetAxis("Horizontal"); // лево-право (A/D, стрелки)
        movementInput.z = Input.GetAxis("Vertical");   // вперед-назад (W/S, стрелки)

        if (Input.GetKey(KeyCode.Space) && isGrounded) // если нажата клавиша пробела и шар на земле
        {
            if (jumpBarCurent <= 1) // если полоса прыжка не заполнена
            {
                jumpBarCurent += jumpBarSpeed * Time.deltaTime;
                jumpForceCurent = jumpForce * jumpBarCurent;
                gameManager.JumpBarChange(jumpBarCurent);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded) // если отпущена клавиша пробела и шар на земле
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForceCurent, rb.linearVelocity.z);
            jumpBarCurent = 0;
            gameManager.JumpBarChange(jumpBarCurent);
        }

        if (Input.GetKey(dashKey)) // удерживание клавиши для рывка
        {
            if (dashBarCurent <= 1)
            {
                dashBarCurent += dashBarSpeed * Time.deltaTime;
                dashForceCurent = dashForce * dashBarCurent;
                gameManager.DashBarChange(dashBarCurent);
            }
            
        }

        if (Input.GetKeyUp(dashKey)) // Отпускание клавиши рывка
        {

            // Вычисляем направление рывка. 
            // Если игрок никуда не жмет, то и рывка не будет.
            // Если жмет кнопки движения — в сторону движения.
            Vector3 dashDirection = movementInput.normalized;
            // Прикладываем мгновенную силу
            rb.AddForce(dashDirection * dashForceCurent, ForceMode.Impulse);
            dashBarCurent = 0;
            gameManager.DashBarChange(dashBarCurent);
        }

        // ПРОИГРЫШ
        if (transform.position.y < loseYLevel)
        {
            gameManager.LoseGame();
            // StopMoving();
            // gameManager.LoseGame();
        }
    }

    private void StopMoving() // останавливаем шар
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate() 
    {
        if (gameManager.CurrentState != GameState.Playing && gameManager.CurrentState != GameState.Start)
            return;
        // применяем силу к Rigidbody для физического движения
        rb.AddForce(movementInput * moveSpeed);
    }


    void OnCollisionEnter(Collision collision) // метод, вызываемый при столкновении с другим объектом
    {
        // проверяем, что шар коснулся земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            curentColiders.Add(collision);
            Debug.Log("Входим в контакт с землей количество контактов: " + curentColiders.Count);
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision) // метод, вызываемый при выходе из столкновения с другим объектом
    {
        // проверяем, что шар оторвался от земли
        if (collision.gameObject.CompareTag("Ground") &&
        curentColiders.Contains(collision))
        {
            curentColiders.Remove(collision);
            Debug.Log("Выходим из контакта с землей количество контактов: " + curentColiders.Count);
            if (curentColiders.Count == 0)
                isGrounded = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Start"))
        {
            gameManager.StartGame();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            gameManager.CheckWinGame();
        }
        
    }
}
