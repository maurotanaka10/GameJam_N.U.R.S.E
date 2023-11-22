using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorManager : MonoBehaviour
{
    [SerializeField] private DoctorController _doctorController;

    public static Action<EDoctorStates> OnIdleReceived;
    public static Action<EDoctorStates> OnWalkInsideReceived;
    public static Action<EDoctorStates> OnWalkOutsideReceived;

    private void Awake()
    {
        _doctorController.OnIdle += HandleIdle;
        _doctorController.OnWalkInside += HandleWalkInside;
        _doctorController.OnWalkOutside += HandleWalkOutside;
    }

    private void HandleWalkOutside(EDoctorStates currentState)
    {
        OnWalkOutsideReceived?.Invoke(currentState);
    }

    private void HandleWalkInside(EDoctorStates currentState)
    {
        OnWalkInsideReceived?.Invoke(currentState);
    }

    private void HandleIdle(EDoctorStates currentState)
    {
        OnIdleReceived?.Invoke(currentState);
    }

    private void OnDisable()
    {
        _doctorController.OnIdle -= HandleIdle;
        _doctorController.OnWalkInside -= HandleWalkInside;
        _doctorController.OnWalkOutside -= HandleWalkOutside;
    }
}
