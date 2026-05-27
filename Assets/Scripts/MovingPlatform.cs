using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveType
    {
        Vertical,
        Horizontal
    }

    [Header("Тип движения")]
    [SerializeField] private MoveType moveType;

    [Header("Настройки движения")]
    [SerializeField] private float moveDistance = 3f; // расстояние движения
    [SerializeField] private float moveSpeed = 2f; // скорость движения

    private Vector3 startPosition; 

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        Debug.Log(offset);
        
        Vector3 newPosition = startPosition;

        switch (moveType)
        {
            case MoveType.Vertical:
                newPosition.y += offset;
                break;

            case MoveType.Horizontal:
                newPosition.x += offset;
                break;
        }

        transform.position = newPosition;
    }
}