using UnityEngine;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    private Vector3 movementInput; // вектор направления движения
    [SerializeField] private float jumpForce = 5f; // максимальная сила прыжка
    private float jumpForceCurent; // текущее значение силы прыжка
    [SerializeField] private float jumpBarSpeed = 2f; // скорость заполнения полосы прыжка
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

    // === НОВОЕ: Ссылка на скрипт камеры для вызова тряски ===
    private CameraFollow cameraFollow;

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

        // === НОВОЕ: Получаем компонент камеры ===
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (gameManager.CurrentState != GameState.Playing && gameManager.CurrentState != GameState.Start)
            return;

        // считываем ввод с клавиатуры/геймпада по осям
        movementInput.x = Input.GetAxis("Horizontal"); // лево-право (A/D, стрелки)
        movementInput.z = Input.GetAxis("Vertical");   // вперед-назад (W/S, стрелки)

        // --- ПРЫЖОК ---
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

            // === НОВОЕ: Легкая тряска камеры при прыжке ===
            if (cameraFollow != null) cameraFollow.ShakeCamera(0.15f, 0.2f);
        }

        // --- РЫВОК ---
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
            // === НОВОЕ: Направление рывка теперь тоже относительно камеры ===
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f; // Игнорируем наклон камеры по вертикали
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 dashDirection = (camForward * movementInput.z) + (camRight * movementInput.x);

            // Если игрок куда-то движется, делаем рывок в этом направлении
            if (dashDirection.magnitude > 0.1f)
            {
                rb.AddForce(dashDirection.normalized * dashForceCurent, ForceMode.Impulse);
            }

            dashBarCurent = 0;
            gameManager.DashBarChange(dashBarCurent);

            // === НОВОЕ: Тряска камеры при рывке (чуть сильнее) ===
            if (cameraFollow != null) cameraFollow.ShakeCamera(0.25f, 0.3f);
        }

        // ПРОИГРЫШ
        if (transform.position.y < loseYLevel)
        {
            gameManager.LoseGame();
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

        // === НОВОЕ: Движение относительно направления камеры ===
        if (movementInput.magnitude > 0.1f)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // Обнуляем ось Y, чтобы шарик не пытался лететь вверх или врываться в землю
            camForward.y = 0f;
            camRight.y = 0f;

            // Нормализуем векторы
            camForward.Normalize();
            camRight.Normalize();

            // Собираем итоговое направление движения на основе ввода и направления камеры
            Vector3 targetDirection = (camForward * movementInput.z) + (camRight * movementInput.x);

            // Применяем силу к Rigidbody для физического движения
            rb.AddForce(targetDirection * moveSpeed);
        }
    }

    void OnCollisionEnter(Collision collision) // метод, вызываемый при столкновении с другим объектом
    {
        // проверяем, что шар коснулся земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            curentColiders.Add(collision);
            // Debug.Log("Входим в контакт с землей количество контактов: " + curentColiders.Count);
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision) // метод, вызываемый при выходе из столкновения с другим объектом
    {
        // проверяем, что шар оторвался от земли
        if (collision.gameObject.CompareTag("Ground") && curentColiders.Contains(collision))
        {
            curentColiders.Remove(collision);
            // Debug.Log("Выходим из контакта с землей количество контактов: " + curentColiders.Count);
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