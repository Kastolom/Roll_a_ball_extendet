using System.Collections;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private AudioClip collectSound;
    private float rotateSpeed; // случайная скорость вращения для каждой монеты своя
    private float curentRotatSpeed; // текущая скорость вращения
    private Renderer _renderer; // ссылка на Renderer для изменения прозрачности монеты
    private Material _material; // ссылка на материал
    [SerializeField] private TextMeshPro textCoin; // ссылка на объект с текстом с количеством монет
    
    private void Start() //Запускаем 1 раз при запуске игры до обновления 1-го кадра
    {
        _renderer = GetComponent<Renderer>(); // получаем ссылку на Renderer
        _material = _renderer.material; // получаем материал
        rotateSpeed = Random.Range(10f, 30f); // задаем случайную скорость вращения

        StartCoroutine(RotateY()); // запускаем корутину вращения монеты
    }
    private void Update()
    {
        //transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); //монеты вращались не вокруг нужной оси
        Vector3 _ballPosition = BallController.instance.transform.position; //получаем позицию игрока
        float _distance = Vector3.Distance(transform.position, _ballPosition); //получаем дистанцию между игроком и монетой

        curentRotatSpeed = rotateSpeed * _distance;
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
            Destroy(GetComponent<Collider>()); // удаляем коллайдер чтобы не было повторного сбора
            // звук
            AudioSource.PlayClipAtPoint(
            collectSound,
            transform.position, 2f);
            GameManager.instance.AddCoin(coinValue);
            textCoin.gameObject.SetActive(true); // включаем текст с количеством монет
            StartCoroutine(CoinDestroyAnima()); // запускаем анимацию уничтожения монеты
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

    IEnumerator CoinDestroyAnima() // анимация уничтожения монеты
    {
        rotateSpeed *= 50; // увеличиваем скорость вращения при уничтожении

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 2f;

        float duration = 0.5f; // длительность анимации
        
        Color startColor = _material.color; // сохраняем начальный цвет монетки
        Color targetColor = startColor; // устанавливаем конечный цвет монетки
        Color startTextColor = textCoin.color; // сохраняем начальный цвет текста
        Color targetTextColor = startTextColor; // устанавливаем конечный цвет текста
        targetTextColor.a = 0f; // делаем текст прозрачным
        targetColor.a = 0f;
        
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, t / duration);
            _material.color = Color.Lerp(startColor, targetColor, t / duration);
            textCoin.color = Color.Lerp(startTextColor, targetTextColor, t / duration);
            yield return null;
        }
        
        Destroy(gameObject);
        Destroy(textCoin);
    }
}
