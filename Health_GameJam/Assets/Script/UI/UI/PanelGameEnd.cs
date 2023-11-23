using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGameEnd : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _panelGameOver;

    private void Update()
    {
        if (_gameManager.GameIsOver)
        {
            _panelGameOver.SetActive(true);
        }
    }
}
