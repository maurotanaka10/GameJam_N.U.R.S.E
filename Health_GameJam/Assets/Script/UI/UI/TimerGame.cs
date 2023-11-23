using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _timerText;

    private void Update()
    {
        _timerText.text = _gameManager.TimerGame.ToString("F2");
    }
}
