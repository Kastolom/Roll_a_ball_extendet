using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameManager gameManager;
    // Вызывается перед первым кадром
    void Start()
    {
        gameManager = GameManager.instance;
        StartCoroutine(RotateY());
    }

    // Вызывается при попадании коллайдера в триггер
    void OnTriggerEnter(Collider other)
    {
        gameManager.AddCoin();
        Destroy(gameObject);
    }

    // Корутина вращения монеты вокруг оси Y
    IEnumerator RotateY()
    {
        while (true)
        {
            transform.Rotate(0, 0, 90 * Time.deltaTime);
            yield return null;
        }
    }
}