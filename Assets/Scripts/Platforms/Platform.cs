using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    [Header("Для движущейся платформы")]
    [SerializeField] private bool isMoving; // перемещается ли платформа
    [SerializeField] private float moveSpeed; // скорость движения платформы
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
    private Renderer _renderer;
    private Material _material;
    
    void Start()
    {
        _renderer = GetComponent<Renderer>();
         _material = _renderer.material;
        if(isMoving) StartCoroutine(MovePlatformAnima());
        if(isRotating) StartCoroutine(RotatePlatformAnima());
        if(isSwinging) StartCoroutine(SwingPlatformAnima());
    }

    IEnumerator MovePlatformAnima()
    {   
        Vector3 startPosition = transform.position;
        Vector3 newPosition = startPosition + moveDirection;
        while(true)
        {
            float wave = Mathf.Sin(Time.time * moveSpeed); // -1 до 1
            float pingPongValue = (wave + 1f) / 2f; // 0 до 1

            transform.position = Vector3.Lerp(startPosition, newPosition, pingPongValue);
            yield return null;
        }
    }

    IEnumerator RotatePlatformAnima()
    {
        while(true) 
        {
            transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);
            yield return null;
        }        
    }

    IEnumerator SwingPlatformAnima()
    {
        Vector3 startRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = startRotation + swingDirection;
        while(true)
        {
            float wave = Mathf.Sin(Time.time * swingSpeed); // -1 до 1
            float pingPongValue = (wave + 1f) / 2f; // 0 до 1

            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, newRotation, pingPongValue));
            yield return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isFalling) StartCoroutine(FallPlatformAnima());
    }
    IEnumerator FallPlatformAnima()
    {
        Color _colorFall = Color.yellow;
        Color _color = _material.color; // устанавливаем начальный цвет
        _material.color = _colorFall; // меняем цвет на красный
        yield return new WaitForSeconds(0.1f);
        _material.color = _color; // возвращаем начальный цвет
        yield return new WaitForSeconds(0.9f);
        _material.color = _colorFall; // меняем цвет на красный
        yield return new WaitForSeconds(0.1f);
        _material.color = _color; // возвращаем начальный цвет
        yield return new WaitForSeconds(0.6f);
         _material.color = _colorFall; // меняем цвет на красный
        yield return new WaitForSeconds(0.1f);
        _material.color = _color; // возвращаем начальный цвет
        yield return new WaitForSeconds(0.4f);
         _material.color = _colorFall; // меняем цвет на красный
        yield return new WaitForSeconds(0.1f);
        _material.color = _color; // возвращаем начальный цвет
        yield return new WaitForSeconds(0.2f);
         _material.color = _colorFall; // меняем цвет на красный
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject.GetComponent<Platform>());
        Rigidbody _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;    
    }
}
