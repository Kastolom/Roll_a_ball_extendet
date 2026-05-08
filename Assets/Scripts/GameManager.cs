using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // статический экземпляр класса GameManager
    public const int coinForWin = 5; // количество очков для победы
    public int coinCount {get; private set; } // текущий счет игры
    public event Action OnCoinCountChanged; // событие изменения счета
    public event Action OnGameWin; // событие победы

    void Awake()
    {
        instance = this;
    }
    public void AddCoin(int _coinCount)
    {
        coinCount += _coinCount;
        if (coinCount >= coinForWin)
        {
            OnCoinCountChanged?.Invoke();
            OnGameWin?.Invoke();
            return;
        } 
        OnCoinCountChanged?.Invoke();
    }

}
