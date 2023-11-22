using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    #region Components

    private CharacterController _characterController;

    #endregion

    #region Variables

    private Vector2 _characterMovementInput;
    private Vector3 _characterMovement;
    private Vector3 _positionToLookAt;
    private const float _gravity = -9.81f;
    private float _gravityVelocity;
    private float _playerVelocity;
    private bool _isMoving;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _rotationVelocity;

    #endregion

    #region Delegate

    public delegate float PlayerVelocityMovement();

    public static PlayerVelocityMovement PlayerVelocityMove;

    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        PlayerManager.OnMovementHandle += SetMoveInfo;
        PlayerManager.OnRunHandle += HandleRunning;
        PlayerManager.CharacterControllerRef = GetCharacterController;

        _playerVelocity = 3f;
    }
    
    private void FixedUpdate()
    {
        SetMovement(_characterMovementInput);
        RotationHandler();
        GravityHandler();
    }

    private void HandleRunning(bool isRunning, float _playerRunVelocity)
    {
        if (isRunning)
            _playerVelocity = _playerRunVelocity;
        else
        {
            _playerVelocity = PlayerVelocityMove.Invoke();
        }
    }

    private void SetMoveInfo(InputAction.CallbackContext context)
    {
        _characterMovementInput = context.ReadValue<Vector2>();
        _isMoving = _characterMovementInput.x != 0 || _characterMovementInput.y != 0;
    }

    private void SetMovement(Vector2 _characterMovementInput)
    {
        _characterMovement = new Vector3(_characterMovementInput.x, _gravityVelocity, _characterMovementInput.y);
        _characterController.Move(_characterMovement * (_playerVelocity * Time.deltaTime));
    }

    private void RotationHandler()
    {
        _positionToLookAt = new Vector3(_characterMovement.x, 0, _characterMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (_isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, _rotationVelocity * Time.deltaTime);
        }
    }

    private void GravityHandler()
    {
        if (_characterController.isGrounded && _gravityVelocity < 0f)
        {
            _gravityVelocity = -1f;
        }
        else
        {
            _gravityVelocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _characterMovement.y = _gravityVelocity;
    }

    private CharacterController GetCharacterController()
    {
        return _characterController;
    }

    private void OnDisable()
    {
        PlayerManager.OnMovementHandle -= SetMoveInfo;
    }
}