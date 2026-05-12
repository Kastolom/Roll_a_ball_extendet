using System;
using TMPro;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject yourWinText;

    void Start()
    {
        GameManager.instance.OnCoinCountChanged += OnAddCoin;
        GameManager.instance.OnGameWin += OnGameWin;
    }

    private void OnAddCoin()
    {
        score.text = GameManager.instance.coinCount.ToString();
    }

    private void OnGameWin() // метод, вызывающийся при победе
    {
        yourWinText.SetActive(true);
    }

<<<<<<< HEAD
     private void OnGameLoose() // метод, вызывающийся при поражении
    {
        // заглушка метода OnGameLoose
    }

=======
>>>>>>> c5308fb5384a8e00cfa56a737992ca189703f361
}
