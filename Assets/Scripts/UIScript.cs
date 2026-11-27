using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIScript : MonoBehaviour
{
    public TMP_Text lifesText;
    public TMP_Text moneyText;
    public GameObject EnergyIndicator;
    [Header("WinScreen")]
    public GameObject WinScreen;
    public TMP_Text livesScore;
    public TMP_Text coinScore;
    public TMP_Text enemiesScore;
    public TMP_Text totalText;
    public TMP_Text RankText;

    public void Lives(int value)
    {
        lifesText.text = "Lives: " + value;
    }

    public void Money(int value)
    {
        moneyText.text = "$" + value;
    }

    public void Energy(int value)
    {
        if(value <= 0)
        {
            EnergyIndicator.SetActive(false);
        }
        else
        {
            EnergyIndicator.SetActive(true);
        }
    }

    public void WinSequence(int coins, int lives, int enemies)
    {
        WinScreen.SetActive(true);
        int livesPoints = lives * 3;
        livesScore.text = "Lives: " + lives + " = " + livesPoints;
        coinScore.text = "Coins: " + coins + " = " + coins;
        int enemiesPoints = enemies * 2;
        enemiesScore.text = "Enemies Defeated: " + enemies + " = " + enemiesPoints;
        int totalPoints = livesPoints + coins + enemiesPoints;
        int rank;
        if (totalPoints > 30)
            rank = 1;
        else if (totalPoints > 20)
            rank = 2;
        else
            rank = 3;

        totalText.text = "Total: " + totalPoints;

        switch (rank)
        {
            case 1:
                RankText.text = "Rank A";
                break;
            case 2:
                RankText.text = "Rank B";
                break;
            case 3:
                RankText.text = "Rank C";
                break;
            default:
                RankText.text = "Rank Impossible";
                break;
        }

    }
}
