using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // статический экземпляр класса GameManager
    public const int coinForWin = 15; // количество очков для победы
    public int coinCount { get; private set; } // текущий счет игры
    public GameState CurrentState { get; private set; } // текущее состояние игры
    public event Action OnCoinCountChanged; // событие изменения счета
    public event Action OnGameWin; // событие победы
    public event Action OnGameLose; // событие поражения
    public event Action<float> OnJumpBarChange; // событие активации прыжка
    public event Action<float> OnDashBarChange; // событие активации рывка
    public event Action<GameState> OnGameStateChanged; // событие изменения состояния игры

    void Awake() // метод, вызывающийся при загрузке объекта
    {
        instance = this;
    }

    private void Start() // метод, вызывающийся при старте игры
    {
        Application.targetFrameRate = 120; // устанавливаем частоту кадров
        ChangeState(GameState.Playing);
    }

    private void Update() // метод, вызывающийся каждый кадр
    {
        if (CurrentState == GameState.Win ||
            CurrentState == GameState.Lose)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
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
        //Time.timeScale = 0f;
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
    
    public void JumpBarChange( float _jumpBarCurent)
    {
        OnJumpBarChange?.Invoke(_jumpBarCurent);
    }

    public void DashBarChange(float _dashBarCurent)
    {
        OnDashBarChange?.Invoke(_dashBarCurent);
    }
}
