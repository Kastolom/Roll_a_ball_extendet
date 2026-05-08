using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // статический экземпляр класса GameManager
    public const int coinForWin = 5; // количество очков для победы
    public int coinCount {get; private set; } // текущий счет игры
    public event Action<int> OnCoinCountChanged; // событие изменения счета
    public event Action OnGameWin; // событие победы
    
    public void AddCoin()
    {
        if (coinCount >= coinForWin)
        {
            OnGameWin?.Invoke();
            return;
        } 
        coinCount++;
        OnCoinCountChanged?.Invoke(coinCount);
    }
}
