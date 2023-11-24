using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private EnviromentSounds _enviromentSounds;

    public static event Action<InputAction.CallbackContext> OnMoveReceived;
    public static event Action<bool> OnInteractionReceived;
    public static event Action<bool> OnRunReceived;
    public event Action<bool> OnGameIsOver;

    public int Points;
    [SerializeField] private int _takePoints;
    public float TimeToEnd;
    public bool GameIsOver;
    private bool _canTakePoints = true;

    public float TimerGame;

    private void Awake()
    {
        _inputManager.OnMove += HandleMovement;
        _inputManager.OnInteraction += HandleInteraction;
        _inputManager.OnRun += HandleRun;

        PlayerManager.OnSoundErrorReceived += HandlePoints;

        TimerGame = TimeToEnd;
        GameIsOver = false;
    }

    private void HandlePoints()
    {
        StartCoroutine(DelayToTakePoints());
    }

    private void Update()
    {
        HandlerGameIsOver();
        
        if (Points <= 0)
        {
            Points = 0;
        }
    }

    private void HandlerGameIsOver()
    {
        TimerGame -= Time.deltaTime;
        
        if (TimerGame <= 0)
        {
            TimerGame = 0f;
            GameIsOver = true;
            OnGameIsOver?.Invoke(GameIsOver);
            _enviromentSounds.SoundGameComplete();
        }
    }

    private void HandleRun(bool isRunning)
    {
        OnRunReceived?.Invoke(isRunning);
    }

    private void HandleInteraction(bool isInteracting)
    {
        OnInteractionReceived?.Invoke(isInteracting);
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        OnMoveReceived?.Invoke(context);
    }

    private IEnumerator DelayToTakePoints()
    {
        if (_canTakePoints)
        {
            _canTakePoints = false;

            Points -= _takePoints;
            yield return new WaitForSeconds(5f);
            _canTakePoints = true;
        }
    }

    private void OnDisable()
    {
        _inputManager.OnMove -= HandleMovement;
        _inputManager.OnInteraction -= HandleInteraction;
        _inputManager.OnRun -= HandleRun;
    }
}
