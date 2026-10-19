using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Добавлено для возможности перезагрузки сцены

public class Mill : MonoBehaviour
{
    [Header("Для вращающейся мельницы")]
    [SerializeField] private bool isRotating; // вращается ли мельница
    [SerializeField] private float rotateSpeed; // скорость вращения мельницы
    [SerializeField] private Vector3 rotateDirection; // направление вращения мельницы (ось)

    [Header("Для настройки проигрыша")]
    [SerializeField] private string playerTag = "Player"; // Тег шарика-игрока
    [SerializeField] private bool restartSceneOnLose; // перезагружать ли сцену при проигрыше

    void Start()
    {
        if (isRotating) StartCoroutine(RotateMillAnima());
    }

    IEnumerator RotateMillAnima()
    {
        while (true)
        {
            transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что столкновение произошло именно с игроком (шариком)
        if (collision.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(PlayerLoseAnima(collision.gameObject));
        }
    }

    IEnumerator PlayerLoseAnima(GameObject player)
    {
        Debug.Log("Игрок проиграл! Касание лопастей мельницы.");

        // Визуальный эффект проигрыша 
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            Color originalColor = playerRenderer.material.color;
            playerRenderer.material.color = Color.red;

            yield return new WaitForSeconds(0.3f);

            playerRenderer.material.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Основное действие при проигрыше
        if (restartSceneOnLose)
        {
            // Перезагружаем текущую сцену для рестарта
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Альтернативный вариант 1: просто уничтожить шарик
            // Destroy(player);

        }
    }
}