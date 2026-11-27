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
    public GameObject WinScreen;

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

    public void WinSequence(int coins, int lives)
    {
        WinScreen.SetActive(true);
    }
}
