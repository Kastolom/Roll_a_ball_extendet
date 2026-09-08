using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI targetScore;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject yourWinText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject restartText;
    [SerializeField] private GameObject progressBarJump;
    [SerializeField] private GameObject progressBarDash;
    [SerializeField] private GameObject zoomAdd;
    [SerializeField] private GameObject zoomSub;
    [SerializeField] private TextMeshProUGUI timerText;
    private GameManager gameManager;

    void OnEnable()
    {
        GameManager.OnCoinCountChanged += OnCoinsChanged;
        GameManager.OnGameWin += OnGameWin;
        GameManager.OnGameLose += OnGameLose;
        GameManager.OnJumpBarChange += JumpBarActivate;
        GameManager.OnDashBarChange += DashBarActivate;
        GameManager.OnZoomChange += ShowZoomInfo;
        Timer.OnTimerUpdated += UpdateTimer;      
    }

        void OnDisable ()
    {
        GameManager.OnCoinCountChanged -= OnCoinsChanged;
        GameManager.OnGameWin -= OnGameWin;
        GameManager.OnGameLose -= OnGameLose;
        GameManager.OnJumpBarChange -= JumpBarActivate;
        GameManager.OnDashBarChange -= DashBarActivate;
        GameManager.OnZoomChange -= ShowZoomInfo;
        Timer.OnTimerUpdated -= UpdateTimer;      
    }

    private void OnCoinsChanged(int _curentCoin, int _coinForWin)
    {
        score.text = _curentCoin.ToString();
        targetScore.text = _coinForWin.ToString();
    }

    private void OnGameWin() // метод, вызывающийся при победе
    {
        infoPanel.SetActive(true);
        yourWinText.SetActive(true);
    }

    private void OnGameLose()
    {
        infoPanel.SetActive(true);
        loseText.SetActive(true);
    }

    private void JumpBarActivate(float _jumpBarCurent)
    {
        if (progressBarJump.activeSelf == false) progressBarJump.SetActive(true);
        progressBarJump.GetComponent<Image>().fillAmount = _jumpBarCurent;
    }

    private void DashBarActivate(float _dashBarCurent)
    {
        if (progressBarDash.activeSelf == false) progressBarDash.SetActive(true);
        progressBarDash.GetComponent<Image>().fillAmount = _dashBarCurent;
    }

    private void ShowZoomInfo(float _zoom)
    {
        StartCoroutine(ZoomInfoAnima(_zoom));
    }

    private void UpdateTimer(float _lastTickTime)
    {
        int totalSeconds = Mathf.CeilToInt(_lastTickTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    IEnumerator ZoomInfoAnima(float _zoom)
    {
        if (_zoom > 0)
        {
            zoomAdd.SetActive(true);
            yield return new WaitForSeconds(1f);
            zoomAdd.SetActive(false);
        }
        else if (_zoom < 0)
        {
            zoomSub.SetActive(true);
            yield return new WaitForSeconds(1f);
            zoomSub.SetActive(false);
        }
    }
}
