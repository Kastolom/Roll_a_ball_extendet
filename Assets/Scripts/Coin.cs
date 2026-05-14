using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int coinValue = 1;
    private Vector3 originalScale; // добавляем переменную для хранения начального размера монеты
    private float rotateSpeed; // случайная скорость вращения для каждой монеты своя
    private float curentRotatSpeed; // текущая скорость вращения
    private void Start() //Запускаем 1 раз при запуске игры до обновления 1-го кадра
    {
        StartCoroutine(RotateY()); // запускаем корутину вращения монеты
        originalScale = transform.localScale; // сохраняем начальный размер монеты
        rotateSpeed = Random.Range(10f, 30f); // задаем случайную скорость вращения
    }
    private void Update()
    {
        //transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); //монеты вращались не вокруг нужной оси
        Vector3 _ballPosition = BallController.instance.transform.position; //получаем позицию игрока
        float _distance = Vector3.Distance(transform.position, _ballPosition); //получаем дистанцию между игроком и монетой

        curentRotatSpeed = rotateSpeed * _distance;
        Debug.Log(_distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Предлагаю немного поменять логику проверки столкновения без return
        // Мы хотим проверить что монета столкнулась именно с игроком?
        // 1. Добавляем тег Player на объект игрока
        // 2. Проверяем что это игрок
        // 3. Если это игрок, то добавляем очки и уничтожаем монету

        if (other.CompareTag("Player"))
        {
            // Добавляем очки
            GameManager.instance.AddCoin(coinValue);

            // Уничтожаем монету
            Destroy(gameObject);
        }
    }

    // Корутина вращения монеты вокруг оси Y
    IEnumerator RotateY()
    {
        while (true)
        {
            transform.Rotate(0, 0, curentRotatSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
