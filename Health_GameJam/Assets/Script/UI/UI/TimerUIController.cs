using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIController : MonoBehaviour
{
    [SerializeField] private GameObject _uiGameObject;
    [SerializeField] private Image _uiFill;

    [SerializeField] private int _duration;
    private float remaingDuration;

    private void Awake()
    {
        remaingDuration = 0f;
    }

    private void Update()
    {
        Being();
    }

    private void Being()
    {
        remaingDuration += Time.deltaTime;

        _uiFill.fillAmount = Mathf.InverseLerp(0, _duration, remaingDuration);

        if(remaingDuration >= _duration)
            Destroy(_uiGameObject);
    }
}