using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static event Action<bool> OnInteractionUI; 
    
    private void Awake()
    {
        GameManager.OnInteractionReceived += OnInteractionHandler;
    }

    private void OnInteractionHandler(bool isInteracting)
    {
        OnInteractionUI?.Invoke(isInteracting);
    }

    private void OnDisable()
    {
        GameManager.OnInteractionReceived -= OnInteractionHandler;
    }
}
