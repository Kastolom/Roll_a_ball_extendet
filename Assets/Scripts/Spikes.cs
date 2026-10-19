using UnityEngine;
using System.Collections;
public class Spikes : MonoBehaviour
{
    [Header("Для выдвижных шипов")]
    [SerializeField] private bool isPopping = true;     // выдвигаются ли шипы
    [SerializeField] private float popSpeed = 3f;       // скорость выдвижения
    [SerializeField] private float popHeight = 1.5f;    // высота выдвижения
    [SerializeField] private float stayUpTime = 1.5f;   // сколько торчат
    [SerializeField] private float stayDownTime = 1.5f; // сколько спрятаны
    [Header("Для настройки проигрыша")]
    [SerializeField] private string playerTag = "Player";
    private Vector3 startPosition;
    private Vector3 upPosition;
    private GameManager gameManager;
    void Start()
    {
        startPosition = transform.localPosition;
        upPosition = startPosition + new Vector3(0, popHeight, 0);
        gameManager = FindAnyObjectByType<GameManager>();
        if (isPopping) StartCoroutine(PopAnima());
    }
    IEnumerator PopAnima()
    {
        while (true)
        {
            // 1. Выдвигаем вверх
            while (Vector3.Distance(transform.localPosition, upPosition) > 0.05f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, upPosition, popSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localPosition = upPosition;
            // 2. Ждем
            yield return new WaitForSeconds(stayUpTime);
            // 3. Прячем в землю
            while (Vector3.Distance(transform.localPosition, startPosition) > 0.05f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, popSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localPosition = startPosition;
            // 4. Ждем перед следующим циклом
            yield return new WaitForSeconds(stayDownTime);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            if (gameManager != null) gameManager.LoseGame();
        }
    }
}