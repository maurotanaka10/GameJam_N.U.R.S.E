using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Components

    private PlayerInputSystem _playerInputSystem;

    #endregion

    #region Actions

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<bool> OnInteraction;
    public event Action<bool> OnRun;

    #endregion

    private void Awake()
    {
        _playerInputSystem = new PlayerInputSystem();

        _playerInputSystem.Player.Movement.started += HandleMovementInput;
        _playerInputSystem.Player.Movement.canceled += HandleMovementInput;
        _playerInputSystem.Player.Movement.performed += HandleMovementInput;

        _playerInputSystem.Player.Interaction.started += HandleInteractionInput;
        _playerInputSystem.Player.Interaction.canceled += HandleInteractionInput;

        _playerInputSystem.Player.Run.started += HandleRunInput;
        _playerInputSystem.Player.Run.canceled += HandleRunInput;
    }

    private void HandleRunInput(InputAction.CallbackContext context)
    {
        OnRun?.Invoke(context.ReadValueAsButton());
    }

    private void HandleInteractionInput(InputAction.CallbackContext context)
    {
        OnInteraction?.Invoke(context.ReadValueAsButton());
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    private void OnEnable()
    {
        _playerInputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputSystem.Player.Disable();
        
        _playerInputSystem.Player.Movement.started -= HandleMovementInput;
        _playerInputSystem.Player.Movement.canceled -= HandleMovementInput;
        _playerInputSystem.Player.Movement.performed -= HandleMovementInput;
        
        _playerInputSystem.Player.Interaction.started -= HandleInteractionInput;
        _playerInputSystem.Player.Interaction.canceled -= HandleInteractionInput;

        _playerInputSystem.Player.Run.started -= HandleRunInput;
        _playerInputSystem.Player.Run.canceled -= HandleRunInput;
    }
}
