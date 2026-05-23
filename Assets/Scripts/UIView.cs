using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject yourWinText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject restartText;
    [SerializeField] private GameObject progressBarJump;
    [SerializeField] private GameObject progressBarDash;

    void Start()
    {
        GameManager.instance.OnCoinCountChanged += OnAddCoin;
        GameManager.instance.OnGameWin += OnGameWin;
        GameManager.instance.OnGameLose += OnGameLose;
        GameManager.instance.OnJumpBarChange += JumpBarActivate;
        GameManager.instance.OnDashBarChange += DashBarActivate;
    }

    private void OnAddCoin()
    {
        score.text = GameManager.instance.coinCount.ToString();
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
}
