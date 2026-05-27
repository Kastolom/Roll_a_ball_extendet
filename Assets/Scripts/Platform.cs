using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    [Header("Для движущейся платформы")]
    [SerializeField] private bool isMoving; // перемещается ли платформа
    [SerializeField] private float moveDuration; // продолжительность перемещения платформы в одне сторону
    [SerializeField] private Vector3 moveDirection; // направление движения платформы и амплитуда

    [Header("Для вращающейся платформы")]
    [SerializeField] bool isRotating; // вращается ли платформа
    [SerializeField] private float rotateSpeed; // cкорость вращения платформы
    [SerializeField] private Vector3 rotateDirection; // направление вращения платформы
    
    [Header("Для качающейся платформы")]
    [SerializeField] bool isSwinging; // качается ли платформа
    [SerializeField] private float swingSpeed; // скорость качания платформы
    [SerializeField] private Vector3 swingDirection; // направление качания платформы и амплитуда

    [Header("Для падающей платформы")]
    [SerializeField] bool isFalling; // падает ли платформа
    
    void Start()
    {
        if(isMoving) StartCoroutine(MovePlatformAnima());
    }

    IEnumerator MovePlatformAnima()
    {
        //float smoothSpeed = 0.3f;
        //Vector3 velocity = Vector3.zero;
        Vector3 startPosition = transform.position;
        Vector3 newPosition = startPosition + moveDirection;
        while(true)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, Mathf.PingPong(Time.time, moveDuration) / moveDuration);
            //transform.position = Vector3.SmoothDamp(startPosition, newPosition, ref velocity, smoothSpeed);
            yield return null;
        }
    }
}
