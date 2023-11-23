using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIPoints : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerPoints;

    private void Update()
    {
        _playerPoints.text = _gameManager.Points.ToString();
    }
}
