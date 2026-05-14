using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int coinValue = 1;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 90f;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем что это игрок
        if (!other.CompareTag("Player"))
            return;

        // Добавляем очки
        GameManager.instance.AddCoin(coinValue);

        // Уничтожаем монету
        Destroy(gameObject);
    }
}
