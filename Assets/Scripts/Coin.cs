using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Вызывается перед первым кадром
    void Start()
    {
        StartCoroutine(RotateY());
    }

    // Вызывается при попадании коллайдера в триггер
    void OnTriggerEnter(Collider other)
    {
        GameManager.instance.AddCoin(1);
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