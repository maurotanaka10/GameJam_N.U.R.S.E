using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region Actions

    public static event Action<InputAction.CallbackContext> OnMovementHandle;
    public static event Action<bool> OnInteractionHandle;
    public static event Action<bool, float> OnRunHandle;

    #endregion
    #region Player Variables

    [SerializeField] private float _playerVelocity;
    [SerializeField] private float _playerRunVelocity;

    #endregion

    #region Delegates

    public delegate CharacterController CharacterControllerReference();

    public delegate bool InActionReference();

    public static CharacterControllerReference CharacterControllerRef;
    public static InActionReference InActionRef;

    #endregion

    private void Awake()
    {
        GameManager.OnMoveReceived += HandlePlayerMovement;
        GameManager.OnInteractionReceived += HandlePlayerInteraction;
        GameManager.OnRunReceived += HandlePlayerRun;
        //AnimationComponent.PlayerVelocityRef += GetPlayerVelocity;
        MovementComponent.PlayerVelocityMove = GetPlayerVelocity;
    }

    private void HandlePlayerRun(bool isRunning)
    {
        OnRunHandle?.Invoke(isRunning, _playerRunVelocity);
    }

    private void HandlePlayerInteraction(bool isInteracting)
    {
        OnInteractionHandle?.Invoke(isInteracting);
    }

    private void HandlePlayerMovement(InputAction.CallbackContext context)
    {
        OnMovementHandle?.Invoke(context);
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