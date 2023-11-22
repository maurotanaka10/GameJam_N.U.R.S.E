using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DoctorController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private EDoctorStates _currentState;

    public event Action<EDoctorStates> OnIdle;
    public event Action<EDoctorStates> OnWalkInside;
    public event Action<EDoctorStates> OnWalkOutside;

    private bool _isIdleDelaying = false;
    private bool _isWalking = false;
    private bool _isInsideTheRoom;
    private float _delayTimerIdle;
    private Vector3 _movePosition;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _velocityWalk;
    [SerializeField] private Transform _inicialPosition;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = EDoctorStates.Idle;
        _isWalking = false;
        _isIdleDelaying = false;
    }

    private void Update()
    {
        SetDoctorCurrentState();
    }

    private void SetDoctorCurrentState()
    {
        switch (_currentState)
        {
            case EDoctorStates.Idle:
                IdleStateHandler();
                break;
            case EDoctorStates.WalkInside:
                WalkInsideHandler();
                break;
            case EDoctorStates.WalkOutside:
                WalkOutsideHandler();
                break;
        }
    }

    private void IdleStateHandler()
    {
        OnIdle?.Invoke(_currentState);
        _navMeshAgent.isStopped = true;

        if (_isIdleDelaying)
        {
            _isWalking = false;
            
            _delayTimerIdle += Time.deltaTime;
            if (_delayTimerIdle >= _idleTime)
            {
                _isIdleDelaying = false;
            }
        }

        if (!_isIdleDelaying)
        {
            _delayTimerIdle = 0f;

            if (_isInsideTheRoom)
            {
                _currentState = EDoctorStates.WalkOutside;
            }
            else
            {
                _currentState = EDoctorStates.WalkInside;
            }
        }
    }

    private void WalkInsideHandler()
    {
        OnWalkInside?.Invoke(_currentState);
        _navMeshAgent.speed = _velocityWalk;
        _navMeshAgent.isStopped = false;

        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(SetDestinationPosition());
            _isWalking = true;
        }
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentState = EDoctorStates.Idle;
            _navMeshAgent.isStopped = true;
            _isIdleDelaying = true;
            _isInsideTheRoom = true;
        }
    }

    private void WalkOutsideHandler()
    {
        OnWalkOutside?.Invoke(_currentState);
        _navMeshAgent.isStopped = false;

        if (!_isWalking)
        {
            _navMeshAgent.SetDestination(_inicialPosition.transform.position);
            _isWalking = true;
        }
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentState = EDoctorStates.Idle;
            _navMeshAgent.isStopped = true;
            _isIdleDelaying = true;
            _isInsideTheRoom = false;
        }
    }

    private Vector3 SetDestinationPosition()
    {
        _movePosition = new Vector3(Random.Range(_minPosition.x, _maxPosition.x), transform.position.y,
            Random.Range(_minPosition.y, _maxPosition.y));

        return _movePosition;
    }
}