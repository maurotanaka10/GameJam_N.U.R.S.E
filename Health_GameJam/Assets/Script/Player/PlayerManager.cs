using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InteractionController _interactionController;
    
    #region Actions

    public static event Action<InputAction.CallbackContext> OnMovementHandle;
    public static event Action<bool> OnInteractionHandle;
    public static event Action<bool, float> OnRunHandle;

    public static event Action OnSoundErrorReceived;

    public static event Action OnInteract;
    public static event Action OnDisableInteract;

    #endregion
    #region Player Variables

    [SerializeField] private float _playerVelocity;
    [SerializeField] private float _playerRunVelocity;

    #endregion


    private void Awake()
    {
        GameManager.OnMoveReceived += HandlePlayerMovement;
        GameManager.OnInteractionReceived += HandlePlayerInteraction;
        GameManager.OnRunReceived += HandlePlayerRun;
        _interactionController.OnErrorTask += HandleSoundError;
        MovementComponent.PlayerVelocityMove = GetPlayerVelocity;
    }

    private void HandleSoundError()
    {
        OnSoundErrorReceived?.Invoke();
    }

    private void HandlePlayerRun(bool isRunning)
    {
        OnRunHandle?.Invoke(isRunning, _playerRunVelocity);
    }

    private void HandlePlayerInteraction(bool isPressing)
    {
        OnInteractionHandle?.Invoke(isPressing);
    }

    private void HandlePlayerMovement(InputAction.CallbackContext context)
    {
        OnMovementHandle?.Invoke(context);
    }

    public void Interact()
    {
        OnInteract?.Invoke();
    }
    public void DisableInteraction()
    {
        OnDisableInteract?.Invoke();
    }

    private float GetPlayerVelocity()
    {
        return _playerVelocity;
    }

    private void OnDisable()
    {
        GameManager.OnMoveReceived -= HandlePlayerMovement;
        GameManager.OnInteractionReceived -= HandlePlayerInteraction;
        GameManager.OnRunReceived -= HandlePlayerRun;
    }
}