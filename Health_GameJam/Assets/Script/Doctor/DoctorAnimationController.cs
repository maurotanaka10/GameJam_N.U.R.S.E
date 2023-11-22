using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorAnimationController : MonoBehaviour
{
    private Animator _animator;

    private int _isIdleHash;
    private int _isWalkingHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetAnimatorParameters();

        DoctorManager.OnIdleReceived += IdleAnimationHandle;
        DoctorManager.OnWalkInsideReceived += WalkInsideAnimationHandler;
        DoctorManager.OnWalkOutsideReceived += WalkOutsideAnimationHandler;
    }

    private void WalkOutsideAnimationHandler(EDoctorStates currentState)
    {
        if(currentState == EDoctorStates.WalkOutside)
            _animator.SetTrigger(_isWalkingHash);
    }

    private void WalkInsideAnimationHandler(EDoctorStates currentState)
    {
        if(currentState == EDoctorStates.WalkInside)
            _animator.SetTrigger(_isWalkingHash);
    }

    private void IdleAnimationHandle(EDoctorStates currentState)
    {
        if(currentState == EDoctorStates.Idle)
            _animator.SetTrigger(_isIdleHash);
    }

    private void GetAnimatorParameters()
    {
        _isIdleHash = Animator.StringToHash("isIdle");
        _isWalkingHash = Animator.StringToHash("isWalking");
    }

    private void OnDisable()
    {
        DoctorManager.OnIdleReceived -= IdleAnimationHandle;
        DoctorManager.OnWalkInsideReceived -= WalkInsideAnimationHandler;
        DoctorManager.OnWalkOutsideReceived -= WalkOutsideAnimationHandler;
    }
}
