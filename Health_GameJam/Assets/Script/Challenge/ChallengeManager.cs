using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeManager : MonoBehaviour
{
    private EChallenges _eChallenges;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SpawnChallengeController _spawnChallengeController;

    public event Action<EChallenges> OnChallengeActivated;
    
    public List<ActiveChallenge> _activeChallenges = new List<ActiveChallenge>();
    public List<EChallenges> ChallengesList = new List<EChallenges>();

    private float _timerForAddChallenge = 0f;
    private bool _completedChallenge = false;
    private bool _gameIsOver = false;
    [SerializeField] private float _timeToAddChallenge;
    [SerializeField] private float _taskDuration;
    [SerializeField] private int _pointsEarnedPerChallenge;
    [SerializeField] private int _pointsLostedPerChallenge;

    private void Awake()
    {
        ActivedRandomChallenge(_taskDuration);

        _gameManager.OnGameIsOver += StopTasksController;
    }

    private void StopTasksController(bool gameIsOver)
    {
        _gameIsOver = gameIsOver;
    }

    private void Update()
    {
        if (!_gameIsOver)
        {
            TimerToAddSecondChallenge();

            _completedChallenge = false;

            for (int i = 0; i < _activeChallenges.Count; i++)
            {
                _activeChallenges[i].UpdateTimer(Time.deltaTime);
            }
        }
    }

    private void TimerToAddSecondChallenge()
    {
        _timerForAddChallenge += Time.deltaTime;
        if (_completedChallenge)
        {
            _timerForAddChallenge = 0f;
        }

        if (_timerForAddChallenge >= _timeToAddChallenge)
        {
            _timerForAddChallenge = 0f;
            ActivedRandomChallenge(_taskDuration);
        }
    }

    public void ActivedRandomChallenge(float challengeDuration)
    {
        if (ChallengesList.Count > 0)
        {
            int randomIndex = Random.Range(0, ChallengesList.Count);
            EChallenges newChallenge = ChallengesList[randomIndex];

            if (!_activeChallenges.Exists(challenge => challenge.ChallengeType == newChallenge))
            {
                ActiveChallenge _activeChallenge = new ActiveChallenge(newChallenge, challengeDuration);
                _activeChallenge.OnChallengeTimeout += HandleChallengeTimeout;
                _activeChallenges.Add(_activeChallenge);
                
                Debug.Log($"Novo desafio ativado:{newChallenge}");
                OnChallengeActivated?.Invoke(newChallenge);
            }
        }
    }

    private void HandleChallengeTimeout(EChallenges challengeType)
    {
        Debug.Log($"Desafio expirado:{challengeType}");
        RemoveChallenge(challengeType);

        if (challengeType == EChallenges.CheckPacient)
        {
            _spawnChallengeController.DestroyCheckPatientChallenge();
        }
        else if (challengeType == EChallenges.Clear)
        {
            _spawnChallengeController.DestroyClearChallenge();
        }
        else
        {
            _spawnChallengeController.DestroyFoodChallenge();
        }
    }
    
    public void CompleteChallenge(EChallenges completedChallenge)
    {
        ActiveChallenge challenge = _activeChallenges.Find(active => active.ChallengeType == completedChallenge);
        if (challenge != null)
        {
            _activeChallenges.Remove(challenge);
            Debug.Log($"Desafio concluído: {completedChallenge}");
            _gameManager.Points += _pointsEarnedPerChallenge;
        }
    }

    public void RemoveChallenge(EChallenges challengesToRemove)
    {
        if (!_gameIsOver)
        {
            ActiveChallenge challenge = _activeChallenges.Find(active => active.ChallengeType == challengesToRemove);
            if (challenge != null)
            {
                _activeChallenges.Remove(challenge);
                Debug.Log($"Desafio removido: {challengesToRemove}");
                _gameManager.Points -= _pointsLostedPerChallenge;
            }
        }
    }

    private void OnDisable()
    {
        _gameManager.OnGameIsOver -= StopTasksController;
    }
}

public enum EChallenges
{
    Clear,
    CheckPacient,
    MakeFood
}
