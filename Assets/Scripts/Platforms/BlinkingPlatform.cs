using UnityEngine;       // Подключаем сборку Unity Engine
using System.Collections; // Подключаем пространство имён для корутин

public class BlinkingPlatform : MonoBehaviour  // Класс наследуется от MonoBehaviour (компонент Unity)
{
    [Header("Для мигающей платформы")] // Открывает заголовок в редакторе Inspector
    [SerializeField] private float visibleTime = 3f;        // сколько секунд платформа видна
    [SerializeField] private float hiddenTime = 2f;         // сколько секунд платформа невидима
    [SerializeField] private float fadeDuration = 1f;       // время плавного потускнения перед исчезновением
    [Header("Настройка прозрачности")] // Открывает заголовок в редакторе Inspector
    [SerializeField] private float minAlpha = 0.2f;         // минимальная прозрачность при потускнении (0 = полностью невидима)
    private Renderer _renderer;       // Ссылка на компонент Renderer объекта
    private Collider _collider;      // Ссылка на компонент Collider объекта
    private Material _material;      // Ссылка на материал, привязанный к объекту
    private Color _originalColor;    // Сохраняет исходный цвет материала

    void Start()  // Вызывается один раз при инициализации объекта в сцене
    {
        _renderer = GetComponent<Renderer>();   // Получаем компонент Renderer из этого объекта
        _collider = GetComponent<Collider>();   // Получаем компонент Collider из этого объекта
        if (_renderer != null)                  // Проверяем, найден ли компонент Renderer
        {
            _material = _renderer.material;     // Берём материал, привязанный к объекту
            _originalColor = _material.color;   // Сохраняем исходный цвет материала
            // Устанавливаем полную видимость при старте
            SetAlpha(1f);                       // Делаем объект полностью непрозрачным сразу после старта
        }
        StartCoroutine(BlinkAnima());           // Запускаем корутину, которая управляет анимацией мигания
    }

    IEnumerator BlinkAnima()  // Корутина: бесконечный цикл мигания платформы
    {
        while (true)  // Бесконечный цикл — платформа будет мигать постоянно
        {
            // 1. Платформа полностью видна и тверда
            SetPlatformState(true);              // Включаем Renderer и Collider (объект виден и взаимодействует с физикой)
            SetAlpha(1f);                       // Устанавливаем полную непрозрачность
            yield return new WaitForSeconds(visibleTime - fadeDuration);  // Ждём: visibleTime минус fadeDuration, чтобы фаза полной видимости совпала с fadeDuration
            // 2. Плавное потускнение (платформа еще твердая!)
            float timer = 0f;                    // Переменная-таймер для отслеживания прогресса анимации
            while (timer < fadeDuration)         // Цикл длится ровно fadeDuration секунд
            {
                timer += Time.deltaTime;         // Добавляем прошедшее время с момента последнего кадра
                float progress = timer / fadeDuration; // Вычисляем процент завершения анимации (от 0 до 1)
                float currentAlpha = Mathf.Lerp(1f, minAlpha, progress);   // Плавно меняем прозрачность от 1.0 до minAlpha
                SetAlpha(currentAlpha);          // Применяем новую прозрачность к материалу
                yield return null;              // Ждём следующий кадр (небольшая пауза для анимации)
            }
            SetAlpha(minAlpha);                  // Устанавливаем минимальную прозрачность в конце анимации
            // 3. Платформа исчезает полностью и становится не твердой
            SetPlatformState(false);             // Отключаем Renderer и Collider (объект невидим и не взаимодействует с физикой)
            yield return new WaitForSeconds(hiddenTime);  // Ждём hiddenTime — платформа «прозрачна» в течение этого времени
        }
    }

    void SetPlatformState(bool isVisible)  // Метод для включения/выключения видимости и физического взаимодействия
    {
        if (_renderer != null)              // Если компонент Renderer найден
            _renderer.enabled = isVisible;   // Включаем или выключаем отображение объекта

        if (_collider != null)              // Если компонент Collider найден
            _collider.enabled = isVisible;   // Включаем или выключаем физическое взаимодействие
    }

    void SetAlpha(float alpha)  // Метод для установки прозрачности материала
    {
        if (_material != null)             // Проверяем, существует ли материал
        {
            Color newColor = _originalColor;  // Создаём копию исходного цвета
            newColor.a = alpha;              // Меняем альфа-канал (прозрачность)
            _material.color = newColor;      // Применяем изменённый цвет к материалу
        }
    }
}