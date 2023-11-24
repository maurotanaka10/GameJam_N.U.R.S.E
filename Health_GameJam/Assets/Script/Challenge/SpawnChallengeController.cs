using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnChallengeController : MonoBehaviour
{
    [SerializeField] private ChallengeManager _challengeManager;

    [SerializeField] private GameObject[] _patients;

    #region Clear Challenge Variables

    [SerializeField] private Transform[] _spawnClearPoints;
    [SerializeField] private GameObject _clearPrefab;
    private GameObject _clearPrefabInstance;

    #endregion

    public GameObject SelectedPatientCheck;
    public GameObject SelectedPatientFood;


    #region UI Challenge Variables

    [SerializeField] private GameObject[] _timerChallengePrefab;
    [SerializeField] private RectTransform[] _patientTransform;
    [SerializeField] private RectTransform[] _dirtTransform;
    private GameObject _timerFoodChallengeInstance;
    private GameObject _timerClearChallengeInstance;
    private GameObject _timerCheckChallengeInstance;

    #endregion
    
    private void Awake()
    {
        _challengeManager.OnChallengeActivated += HandlerSpawnChallenge;
    }

    private void HandlerSpawnChallenge(EChallenges challenge)
    {
        if (challenge == EChallenges.Clear)
        {
            SpawnRandomClearChallengeLocation();
        }
        else if (challenge == EChallenges.MakeFood)
        {
            StartMakeFoodChallenge();
        }
        else if (challenge == EChallenges.CheckPacient)
        {
            StartCheckPacientChallenge();
        }
    }

    private void StartCheckPacientChallenge()
    {
        if (_patients.Length > 0)
        {
            int randomIndex = Random.Range(0, _patients.Length);
            SelectedPatientCheck = _patients[randomIndex];

            while (SelectedPatientCheck == SelectedPatientFood)
            {
                randomIndex = Random.Range(0, _patients.Length);
                SelectedPatientCheck = _patients[randomIndex];
            }

            if (SelectedPatientCheck == _patients[0])
            {
                _timerCheckChallengeInstance = Instantiate(_timerChallengePrefab[1], _patientTransform[0]);
            }
            else
            {
                _timerCheckChallengeInstance = Instantiate(_timerChallengePrefab[1], _patientTransform[1]);
            }
        }
    }

    public void DestroyCheckPatientChallenge()
    {
        if (_timerCheckChallengeInstance != null)
        {
            Destroy(_timerCheckChallengeInstance);
            SelectedPatientCheck = null;
        }
    }

    private void StartMakeFoodChallenge()
    {
        int randomIndex = Random.Range(0, _patients.Length);
        SelectedPatientFood = _patients[randomIndex];
        
        while (SelectedPatientFood == SelectedPatientCheck)
        {
            randomIndex = Random.Range(0, _patients.Length);
            SelectedPatientFood = _patients[randomIndex];
        }
        
        if (SelectedPatientFood == _patients[0])
        {
            _timerFoodChallengeInstance = Instantiate(_timerChallengePrefab[2], _patientTransform[0]);
        }
        else
        {
            _timerFoodChallengeInstance = Instantiate(_timerChallengePrefab[2], _patientTransform[1]);
        }
    }

    public void DestroyFoodChallenge()
    {
        if (_timerFoodChallengeInstance != null)
        {
            Destroy(_timerFoodChallengeInstance);
            SelectedPatientFood = null;
        }
    }

    private void SpawnRandomClearChallengeLocation()
    {
        if (_spawnClearPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, _spawnClearPoints.Length);
            Transform randomSpawnClearPoint = _spawnClearPoints[randomIndex];

            InstantiateClearChallengeIndicator(randomSpawnClearPoint.position);

            if (randomIndex == 0)
            {
                _timerClearChallengeInstance = Instantiate(_timerChallengePrefab[0], _dirtTransform[0]);
            }
            else if (randomIndex == 1)
            {
                _timerClearChallengeInstance = Instantiate(_timerChallengePrefab[0], _dirtTransform[1]);
            }
            else if (randomIndex == 2)
            {
                _timerClearChallengeInstance = Instantiate(_timerChallengePrefab[0], _dirtTransform[2]);
            }
            else if (randomIndex == 3)
            {
                _timerClearChallengeInstance = Instantiate(_timerChallengePrefab[0], _dirtTransform[3]);
            }
            else
            {
                _timerClearChallengeInstance = Instantiate(_timerChallengePrefab[0], _dirtTransform[4]);
            }
        }
    }

    private void InstantiateClearChallengeIndicator(Vector3 spawnPosition)
    {
        if (_clearPrefabInstance == null)
        {
            _clearPrefabInstance = Instantiate(_clearPrefab, spawnPosition, _clearPrefab.transform.rotation);
        }
        else
        {
            _clearPrefabInstance.SetActive(true);
            _clearPrefabInstance.transform.position = spawnPosition;
        }
    }

    public void DestroyClearChallenge()
    {
        if (_timerClearChallengeInstance != null)
        {
            Destroy(_timerClearChallengeInstance);
            _clearPrefabInstance.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _challengeManager.OnChallengeActivated -= HandlerSpawnChallenge;
    }
}