using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // статический экземпляр класса GameManager
    public const int coinForWin = 5; // количество очков для победы
    public int coinCount {get; private set; } // текущий счет игры
     public GameState CurrentState { get; private set; }
    public event Action OnCoinCountChanged; // событие изменения счета
    public event Action OnGameWin; // событие победы
    public event Action OnGameLose;
    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    private void Update()
    {
        if (CurrentState == GameState.Win ||
            CurrentState == GameState.Lose)
        {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        }
    }

    public void AddCoin(int value)
    {
        if (CurrentState != GameState.Playing)
            return;

        coinCount += value;

        OnCoinCountChanged?.Invoke();

        if (coinCount >= coinForWin)
        {
            WinGame();
        }
    }

     public void WinGame()
    {
        ChangeState(GameState.Win);

        Time.timeScale = 0f;

        OnGameWin?.Invoke();
    }

    public void LoseGame()
    {
        ChangeState(GameState.Lose);

        Time.timeScale = 0f;

        OnGameLose?.Invoke();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
