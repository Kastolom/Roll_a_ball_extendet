using System;
using TMPro;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject yourWinText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject restartText;

    void Start()
    {
        GameManager.instance.OnCoinCountChanged += OnAddCoin;
        GameManager.instance.OnGameWin += OnGameWin;
        GameManager.instance.OnGameLose += OnGameLose;
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

}
