using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationComponent : MonoBehaviour
{
    #region Components

    private Animator _animator;
    private CharacterController _characterControllerRef;

    #endregion

    #region Animator Parameters

    //private int _velocityHash;
    private int _inActionHash;

    #endregion

    #region Variables

    //private float _playerVelocity;
    private bool _playerInAction;

    #endregion

    #region Delegate

    public delegate float PlayerVelocityReference();

    public static PlayerVelocityReference PlayerVelocityRef;

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetAnimatorParameters();

        PlayerManager.OnInteractionHandle += HandlerInteractionAnimation;
    }


    private void Update()
    {
        //_characterControllerRef = PlayerManager.CharacterControllerRef?.Invoke();
        _playerInAction = PlayerManager.InActionRef.Invoke();

        //HandlerMovementAnimation();
    }
    
    private void HandlerInteractionAnimation(bool isInteracting)
    {
        Debug.Log($"esta interagindo");
        if (isInteracting && _playerInAction)
        {
            _animator.SetBool(_inActionHash, true);
        }
        else
        {
            _animator.SetBool(_inActionHash, false);
        }
    }

    /*private void HandlerMovementAnimation()
    {
        _playerVelocity = _characterControllerRef.velocity.magnitude;
        _animator.SetFloat(_velocityHash, _playerVelocity);
    }*/

    private void GetAnimatorParameters()
    {
        //_velocityHash = Animator.StringToHash("velocity");
        _inActionHash = Animator.StringToHash("inAction");
    }

    private void OnDisable()
    {
        PlayerManager.OnInteractionHandle -= HandlerInteractionAnimation;
    }
}