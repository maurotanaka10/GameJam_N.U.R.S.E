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
            SelectedPatientCheck.GetComponent<PacientScript>().ActivateCheckIndicator();
        }
    }

    public void DestroyCheckPatientChallenge()
    {
        if (SelectedPatientCheck != null)
        {
            SelectedPatientCheck.GetComponent<PacientScript>().DeactivateCheckIndicator();
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
    
        SelectedPatientFood.GetComponent<PacientScript>().ActivateFoodIndicator();
    }

    public void DestroyFoodChallenge()
    {
        if (SelectedPatientFood != null)
        {
            SelectedPatientFood.GetComponent<PacientScript>().DeactivateFoodIndicator();
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
        if (_clearPrefabInstance != null)
        {
            _clearPrefabInstance.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _challengeManager.OnChallengeActivated -= HandlerSpawnChallenge;
    }
}