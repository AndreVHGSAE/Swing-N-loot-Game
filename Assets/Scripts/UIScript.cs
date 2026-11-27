using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIScript : MonoBehaviour
{
    public TMP_Text lifesText;

    public void Lives(int value)
    {
        lifesText.text = "Lives: " + value;
    }
}
