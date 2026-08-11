using System;
using UnityEngine;

public class Timer
{
    private float duration; // Длительность таймера в секундах
    public float RemainingTime { get; private set; } // Оставшееся время до завершения (секунды), доступно только для чтения
    public bool IsRunning { get; private set; } // Флаг активности таймера, доступен только для чтения снаружи
    public bool IsFinished => RemainingTime <= 0f; // Вычисляемое свойство: true, когда время вышло

    // События для подписки из внешних классов
    public event Action OnTick; // Событие тика (срабатывает каждую секунду)
    public event Action OnFinished; // Событие завершения таймера
    private float _accumulator; // Накопитель прошедшего времени (в секундах)
    private float _lastTickTime; // Значение времени последнего срабатывания тика

    // Публичный метод для запуска таймера с заданной длительностью
    public void StartTimer(float duration)
    {
        this.duration = Mathf.Max(duration, 0f); // Гарантируем неотрицательную длительность
        RemainingTime = this.duration; // Инициализируем оставшееся время полной длительностью
        _accumulator = 0f; // Сбрасываем накопитель времени
        _lastTickTime = this.duration; // Инициализируем последнее время тика
        IsRunning = true; // Устанавливаем флаг активности
    }

    // Публичный метод для полной остановки таймера
    public void StopTimer()
    {
        IsRunning = false; // Останавливаем таймер
        RemainingTime = 0f; // Обнуляем оставшееся время
        _accumulator = 0f; // Обнуляем накопитель
    }

    // Публичный метод для приостановки таймера (сохраняя текущее время)
    public void PauseTimer()
    {
        IsRunning = false; // Только останавливаем, не сбрасывая время
    }

    // Публичный метод для возобновления работы таймера
    public void ResumeTimer()
    {
        IsRunning = true; // Возобновляем отсчет
    }

    // Публичный метод для сброса таймера к начальному состоянию
    public void ResetTimer()
    {
        RemainingTime = duration; // Возвращаем оставшееся время к начальному значению
        _accumulator = 0f; // Сбрасываем накопитель
        _lastTickTime = duration; // Сбрасываем последний тик
        IsRunning = false; // Останавливаем таймер
    }

    // Публичный метод для изменения длительности во время остановки
    public void SetDuration(float duration)
    {
        this.duration = Mathf.Max(duration, 0f); // Гарантируем неотрицательное значение
        if (!IsRunning)
            RemainingTime = this.duration; // Обновляем оставшееся время только если таймер остановлен
    }

    private void Update()
    {
        if (!IsRunning)
            return; // Выходим из метода, если таймер не активен

        _accumulator += Time.deltaTime; // Увеличиваем накопитель на время, прошедшее с последнего кадра

        if (_accumulator >= RemainingTime) // Проверяем, истекло ли время таймера
        {
            RemainingTime = 0f; // Устанавливаем оставшееся время в ноль
            IsRunning = false; // Останавливаем таймер

            OnTick?.Invoke(); // Вызываем событие тика (оператор ?. проверяет наличие подписчиков)
            OnFinished?.Invoke(); // Вызываем событие завершения таймера

        }
        else // Если время еще не истекло
        {
            RemainingTime = duration - _accumulator; // Вычисляем оставшееся время

            if ((int)_lastTickTime != (int)RemainingTime) // Проверяем, прошла ли целая секунда (сравнение целых частей)
            {
                _lastTickTime = RemainingTime; // Обновляем последнее значение времени тика
                OnTick?.Invoke(); // Вызываем событие тика
            }
        }
    }
}
