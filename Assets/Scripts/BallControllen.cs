using UnityEngine;

public class BallControllen : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // скорость движения шара
    [SerializeField] private Vector3 movementInput; // вектор направления движения

    private Rigidbody rb; // компонент Rigidbody шара

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // получаем Rigidbody при старте
    }

    void Update()
    {
        // считываем ввод с клавиатуры/геймпада по осям
        movementInput.x = Input.GetAxis("Horizontal"); // лево-право (A/D, стрелки)
        movementInput.z = Input.GetAxis("Vertical");   // вперед-назад (W/S, стрелки)
    }

    void FixedUpdate()
    {
        // применяем силу к Rigidbody для физического движения
        rb.AddForce(movementInput * moveSpeed);
    }
}
