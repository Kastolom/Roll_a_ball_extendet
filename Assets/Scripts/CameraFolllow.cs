using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Rigidbody targetRb;

    [Header("Настройки следования")]
    // ЕЩЁ ДАЛЬШЕ: Y=25 (очень высоко), Z=-20 (очень далеко сзади)
    [SerializeField] private Vector3 offset = new Vector3(0, 25, -20);

    [SerializeField] private float positionSmoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [Header("Настройки автоматического поворота")]
    [SerializeField] private float rotationSmoothTime = 0.2f;
    [SerializeField] private float cameraPitch = 25f;  // УМЕНЬШЕНО: камера смотрит более горизонтально
    [SerializeField] private float moveThreshold = 0.5f;

    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private float rotationVelocity = 0f;

    [Header("Настройки зума")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 60f;

    [Header("Настройки шейкера (тряски)")]
    [SerializeField] private float defaultShakeDuration = 0.3f;
    [SerializeField] private float defaultShakeMagnitude = 0.1f;

    private Camera cam;
    private float targetZoom;
    private GameManager gameManager;
    private BallController ballController;
    private Vector3 currentShakeOffset = Vector3.zero;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        ballController = FindAnyObjectByType<BallController>();

        cam = GetComponent<Camera>();
        targetZoom = cam.fieldOfView;
        target = ballController.transform;
        targetRb = target.GetComponent<Rigidbody>();

        currentAngle = transform.eulerAngles.y;
        targetAngle = currentAngle;
    }

    private void LateUpdate()
    {
        if (target == null || targetRb == null) return;

        Vector3 moveDir = new Vector3(targetRb.linearVelocity.x, 0, targetRb.linearVelocity.z);

        if (moveDir.sqrMagnitude > moveThreshold * moveThreshold)
        {
            targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        }

        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, rotationSmoothTime);

        Quaternion horizontalRotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 desiredPosition = target.position + (horizontalRotation * offset);
        Quaternion desiredRotation = Quaternion.Euler(cameraPitch, currentAngle, 0f);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionSmoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 5f);

        transform.position += currentShakeOffset;
        if (currentShakeOffset.magnitude > 0)
        {
            currentShakeOffset = Vector3.Lerp(currentShakeOffset, Vector3.zero, Time.deltaTime * 10f);
            if (currentShakeOffset.magnitude < 0.001f) currentShakeOffset = Vector3.zero;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && gameManager != null) gameManager.ZoomChange(scroll);

        targetZoom -= scroll * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        StartCoroutine(ShakeCoroutine(magnitude, duration));
    }

    public void ShakeCamera()
    {
        ShakeCamera(defaultShakeMagnitude, defaultShakeDuration);
    }
    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            currentShakeOffset = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentShakeOffset = Vector3.zero;
    }
}