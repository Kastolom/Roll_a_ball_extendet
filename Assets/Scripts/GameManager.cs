using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance; // статический экземпляр класса GameManager
    [SerializeField] public Timer timerScript; //скрипт таймера
    public const int coinForWin = 15; // количество очков для победы
    public int curentCoin { get; private set; } // текущий счет игры
    public GameState CurrentState { get; private set; } // текущее состояние игры
    public static event Action<int, int> OnCoinCountChanged; // событие изменения счета
    public static event Action OnGameWin; // событие победы
    public static event Action OnGameLose; // событие поражения
    public static event Action<float> OnJumpBarChange; // событие активации прыжка
    public static event Action<float> OnDashBarChange; // событие активации рывка
    //public event Action<GameState> OnGameStateChanged; // событие изменения состояния игры
    public static event Action<float> OnZoomChange; // событие изменения зума

    void OnEnable()
    {
        Timer.OnTimeIsUp += LoseGame;
    }
    void OnDisable()
    {
        Timer.OnTimeIsUp -= LoseGame;
    }
    private void Start() // метод, вызывающийся при старте игры
    {
        Application.targetFrameRate = 120; // устанавливаем частоту кадров
        ChangeState(GameState.Start);
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

        curentCoin += value;

        OnCoinCountChanged?.Invoke(curentCoin, coinForWin); 

        //CheckWinGame();
    }

    public void StartGame()
    {
        if (CurrentState == GameState.Start)
        {
            ChangeState(GameState.Playing);
            timerScript.StartTimer(30);
            OnCoinCountChanged?.Invoke(curentCoin, coinForWin);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckWinGame()
    {
        if (curentCoin >= coinForWin && timerScript.IsRunning)
        {
            ChangeState(GameState.Win);
            OnGameWin?.Invoke();
        }
    }

    public void LoseGame()
    {
        ChangeState(GameState.Lose);
        OnGameLose?.Invoke();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    public void JumpBarChange(float _jumpBarCurent)
    {
        OnJumpBarChange?.Invoke(_jumpBarCurent);
    }

    public void DashBarChange(float _dashBarCurent)
    {
        OnDashBarChange?.Invoke(_dashBarCurent);
    }

    public void ZoomChange(float _zoom)
    {
        OnZoomChange?.Invoke(_zoom);
    }
}
