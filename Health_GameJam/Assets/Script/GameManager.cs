using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    public static event Action<InputAction.CallbackContext> OnMoveReceived;
    public static event Action<bool> OnInteractionReceived;
    public static event Action<bool> OnRunReceived;

    public int Points;

    private void Awake()
    {
        _inputManager.OnMove += HandleMovement;
        _inputManager.OnInteraction += HandleInteraction;
        _inputManager.OnRun += HandleRun;
    }

    private void HandleRun(bool isRunning)
    {
        OnRunReceived?.Invoke(isRunning);
    }

    private void HandleInteraction(bool isInteracting)
    {
        OnInteractionReceived?.Invoke(isInteracting);
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        OnMoveReceived?.Invoke(context);
    }

    private void OnDisable()
    {
        _inputManager.OnMove -= HandleMovement;
        _inputManager.OnInteraction -= HandleInteraction;
        _inputManager.OnRun -= HandleRun;
    }
}
