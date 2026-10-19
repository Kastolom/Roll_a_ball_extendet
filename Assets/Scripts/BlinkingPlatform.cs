using UnityEngine;
using System.Collections;
public class BlinkingPlatform : MonoBehaviour
{
    [Header("Для мигающей платформы")]
    [SerializeField] private float visibleTime = 3f;        // сколько секунд платформа видна
    [SerializeField] private float hiddenTime = 2f;         // сколько секунд платформа невидима
    [SerializeField] private float fadeDuration = 1f;       // время плавного потускнения перед исчезновением
    [Header("Настройка прозрачности")]
    [SerializeField] private float minAlpha = 0.2f;         // минимальная прозрачность при потускнении (0 = полностью невидима)
    private Renderer _renderer;
    private Collider _collider;
    private Material _material;
    private Color _originalColor;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        if (_renderer != null)
        {
            _material = _renderer.material;
            _originalColor = _material.color;
            // Устанавливаем полную видимость при старте
            SetAlpha(1f);
        }
        StartCoroutine(BlinkAnima());
    }
    IEnumerator BlinkAnima()
    {
        while (true)
        {
            // 1. Платформа полностью видна и тверда
            SetPlatformState(true);
            SetAlpha(1f);
            yield return new WaitForSeconds(visibleTime - fadeDuration);
            // 2. Плавное потускнение (платформа еще твердая!)
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeDuration; // от 0 до 1
                float currentAlpha = Mathf.Lerp(1f, minAlpha, progress);
                SetAlpha(currentAlpha);
                yield return null;
            }
            SetAlpha(minAlpha);
            // 3. Платформа исчезает полностью и становится не твердой
            SetPlatformState(false);
            yield return new WaitForSeconds(hiddenTime);
        }
    }
    void SetPlatformState(bool isVisible)
    {
        if (_renderer != null)
            _renderer.enabled = isVisible;

        if (_collider != null)
            _collider.enabled = isVisible;
    }
    void SetAlpha(float alpha)
    {
        if (_material != null)
        {
            Color newColor = _originalColor;
            newColor.a = alpha;
            _material.color = newColor;
        }
    }
}