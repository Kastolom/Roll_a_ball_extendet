using System;
using UnityEngine;

public class CameraFolllow : MonoBehaviour
{
    private Transform target; // цель, за которой следует камера
    [SerializeField] private float smoothSpeed; // скорость сглаживания движения камеры
    private Vector3 offset; // смещение камеры относительно цели
    private Vector3 velocity = Vector3.zero; // текущая скорость движения камеры
    [SerializeField] private float zoomSpeed; // скорость приближения/удаления камеры
    [SerializeField] private float minZoom; // минимальное приближение камеры
    [SerializeField] private float maxZoom; // максимальное приближение камеры++
    private Camera cam;
    private float targetZoom;
    private void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.fieldOfView;
        target = BallController.instance.transform;
        offset = transform.position - target.position; // вычисляем смещение камеры относительно цели
    }
    private void LateUpdate() // метод, вызывающийся после всех обновлений в кадре
    {
        Vector3 targetPosition = target.position + offset; // вычисляем желаемую позицию камеры
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed); // сглаживаем движение камеры
        //transform.LookAt(target); // поворачиваем камеру, чтобы она смотрела на цель

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scroll * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);
    }
}
