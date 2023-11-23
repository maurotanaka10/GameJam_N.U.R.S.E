using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class AnimationComponent : MonoBehaviour
{
    #region Components

    private Animator _animator;
    private CharacterController _characterControllerRef;

    #endregion

    #region Animator Parameters
    
    private int _inActionHash;

    #endregion

    #region Variables
    
    public bool PlayerInAction;

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
        PlayerInAction = PlayerManager.InActionRef.Invoke();
    }
    
    private void HandlerInteractionAnimation(bool isInteracting)
    {
        if (isInteracting && PlayerInAction)
        {
            _animator.SetBool(_inActionHash, true);
        }
        else if(isInteracting && !PlayerInAction)
        {
            _animator.SetBool(_inActionHash, false);
        }
        else
        {
            _animator.SetBool(_inActionHash, false);
        }
    }
    
    private void GetAnimatorParameters()
    {
        _inActionHash = Animator.StringToHash("inAction");
    }

    private void OnDisable()
    {
        PlayerManager.OnInteractionHandle -= HandlerInteractionAnimation;
    }
}