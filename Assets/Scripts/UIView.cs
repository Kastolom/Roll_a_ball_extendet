using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject yourWinText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject restartText;
    [SerializeField] private GameObject progressBarJump;
    [SerializeField] private GameObject progressBarDash;
    [SerializeField] private GameObject zoomAdd;
    [SerializeField] private GameObject zoomSub;

    void Start()
    {
        GameManager.instance.OnCoinCountChanged += OnAddCoin;
        GameManager.instance.OnGameWin += OnGameWin;
        GameManager.instance.OnGameLose += OnGameLose;
        GameManager.instance.OnJumpBarChange += JumpBarActivate;
        GameManager.instance.OnDashBarChange += DashBarActivate;
        GameManager.instance.OnZoomChange += ShowZoomInfo;
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

    private void ShowZoomInfo(float _zoom)
    {
        StartCoroutine(ZoomInfoAnima(_zoom));
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
