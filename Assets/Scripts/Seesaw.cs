using UnityEngine;
public class Seesaw : MonoBehaviour
{
    [Header("Настройки качелей")]
    [SerializeField] private float tiltSpeed = 2f;              // скорость наклона
    [SerializeField] private float maxTiltAngle = 25f;          // максимальный угол наклона
    [SerializeField] private float detectionRange = 5f;         // радиус обнаружения игрока
    [Header("Настройка оси вращения")]
    [SerializeField] private Vector3 tiltAxis = Vector3.right;  // ось наклона
    [Header("Настройка игрока")]
    [SerializeField] private string playerTag = "Player";       // тег игрока
    private Transform playerTransform;
    private float currentTilt = 0f;
    private float targetTilt = 0f;
    private Vector3 startRotation;
    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Игрок с тегом " + playerTag + " не найден!");
        }
    }
    void Update()
    {
        if (playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < detectionRange)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float side = Vector3.Dot(direction.normalized, tiltAxis.normalized);
            targetTilt = side * maxTiltAngle;
        }
        else
        {
            targetTilt = 0f;
        }
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(startRotation + tiltAxis * currentTilt);
    }
}