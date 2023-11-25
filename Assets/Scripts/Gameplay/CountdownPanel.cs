using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text countdownTxt = null;

    private int countdownSeconds = 3;

    public Action onGameStarted = null;
    public void Init(Action onGameStarted)
    {
        Time.timeScale = 0;
        this.onGameStarted = onGameStarted;
    }

    public void DecreaseCountdown()
    {
        countdownSeconds--;
        if (countdownSeconds >= 0)
        {
            countdownTxt.text = countdownSeconds.ToString();
        }
        else
        {
            onGameStarted.Invoke();
            Time.timeScale = 1;
        }
    }
}
