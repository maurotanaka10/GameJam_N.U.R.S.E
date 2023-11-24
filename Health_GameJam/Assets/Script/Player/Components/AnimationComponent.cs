using System;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetAnimatorParameters();
        
        PlayerManager.OnInteract += () => SetActionAnimation(true);
        PlayerManager.OnDisableInteract += () => SetActionAnimation(false);
    }

    public void SetActionAnimation(bool isOn)
    {
        _animator.SetBool(_inActionHash, isOn);
    }
    
    private void GetAnimatorParameters()
    {
        _inActionHash = Animator.StringToHash("inAction");
    }
}