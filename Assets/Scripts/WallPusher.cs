using UnityEngine;
using System.Collections;
public class WallPusher : MonoBehaviour
{
    [Header("Настройки поршня")]
    [SerializeField] private bool isActive = true;          // активен ли механизм
    [SerializeField] private float pushDistance = 3f;       // дистанция выдвижения куба
    [SerializeField] private float pushSpeed = 3f;          // скорость выдвижения
    [SerializeField] private float retractSpeed = 3f;       // скорость возврата
    [SerializeField] private float delayBeforePush = 2f;    // задержка перед выдвижением
    [SerializeField] private float delayBeforeRetract = 1f; // задержка перед возвратом
    [Header("Настройка направления")]
    [SerializeField] private Vector3 pushDirection = Vector3.forward; // направление выдвижения
    [Header("Настройка игрока")]
    [SerializeField] private string playerTag = "Player";   // тег шарика
    [SerializeField] private float knockbackForce = 10f;    // сила выталкивания (доп. импульс)
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isPushing = false;
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + pushDirection * pushDistance;

        if (isActive)
        {
            StartCoroutine(PushCycle());
        }
    }
    IEnumerator PushCycle()
    {
        while (true)
        {
            // Ждём перед выдвижением
            yield return new WaitForSeconds(delayBeforePush);
            // Выдвигаем куб
            isPushing = true;
            while (Vector3.Distance(transform.position, endPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, pushSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = endPosition;
            // Ждём перед возвратом
            yield return new WaitForSeconds(delayBeforeRetract);
            // Возвращаем куб
            isPushing = false;
            while (Vector3.Distance(transform.position, startPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, retractSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = startPosition;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && isPushing)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(pushDirection.normalized * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}