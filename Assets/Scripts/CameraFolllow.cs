using UnityEngine;

public class CameraFolllow : MonoBehaviour
{
    private Transform target; // цель, за которой следует камера
    [SerializeField] private float smoothSpeed; // скорость сглаживания движения камеры
    private Vector3 offset; // смещение камеры относительно цели

    private Vector3 velocity = Vector3.zero; // текущая скорость движения камеры

    private void Start()
    {
        target = BallController.instance.transform;
        offset = transform.position - target.position; // вычисляем смещение камеры относительно цели
    }
    private void LateUpdate() // метод, вызывающийся после всех обновлений в кадре
    {
        Vector3 targetPosition  = target.position + offset; // вычисляем желаемую позицию камеры
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed); // сглаживаем движение камеры
        //transform.LookAt(target); // поворачиваем камеру, чтобы она смотрела на цель
    }
}
