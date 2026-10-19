 using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Событие обновления времени для UI (передает оставшиеся секунды)
    public static event Action<float> OnTimerUpdated;
    // Событие окончания времени для GameManager
    public static event Action OnTimeIsUp;
    private float timeRemaining;
    public bool IsRunning { get; private set; } = false;

    void OnEnable()
    {
        GameManager.OnGameWin += StopTimer;
        GameManager.OnGameLose += StopTimer;
    }
    void OnDisable()
    {
        GameManager.OnGameWin -= StopTimer;
        GameManager.OnGameLose -= StopTimer;   
    }
    void Update()
    {
        if (!IsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            OnTimerUpdated?.Invoke(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            IsRunning = false;
            OnTimerUpdated?.Invoke(timeRemaining);
            OnTimeIsUp?.Invoke(); // Оповещаем, что время вышло
        }
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        IsRunning = true;
        OnTimerUpdated?.Invoke(timeRemaining);
    }

    public void StopTimer()
    {
        IsRunning = false;
    }
}