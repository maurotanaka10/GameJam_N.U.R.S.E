using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private ChallengeManager _challengeManager;
    [SerializeField] private SpawnChallengeController _spawnChallengeController;
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Transform _localHand;
    private GameObject _objectToPickup;
    private bool _isCarryingObject = false;
    private bool _inAction = false;
    private float _interactionTimer;
    private GameObject _currentInteractedPatient;
    private GameObject _currentFoodPatient;

    #region Food Challenge Variables

    [SerializeField] private Transform _foodOriginalPosition;

    #endregion

    #region Clean Challenge Variables

    private bool _canDoClearChallenge;
    private bool _isDoingClearChallenge;
    [SerializeField] private float _interactionClearDuration;

    #endregion

    #region Paciente Check Challenge Variables

    private bool _canDoPacienteCheckChallenge;
    private bool _isDoingPacienteChallenge;
    [SerializeField] private float _interactionPacienteDuration;

    #endregion

    private void Awake()
    {
        PlayerManager.OnInteractionHandle += HandlerInteraction;
        PlayerManager.InActionRef = GetInAction;
    }

    private void HandlerInteraction(bool isInteracting)
    {
        if (isInteracting)
        {
            if (_isCarryingObject)
            {
                DropObject();
            }
            else if (_objectToPickup != null)
            {
                PickUpObject();
            }
            else if (_canDoClearChallenge)
            {
                _isDoingClearChallenge = true;
            }
            else if (_canDoPacienteCheckChallenge)
            {
                _isDoingPacienteChallenge = true;
            }
        }
        else
        {
            _isDoingClearChallenge = false;
            _isDoingPacienteChallenge = false;
        }
    }

    private void Update()
    {
        StartInteractionTimer();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Food"))
        {
            _objectToPickup = collider.gameObject;
        }

        if (_isCarryingObject && collider.gameObject.CompareTag("Patient"))
        {
            ResetPositionObject();
            _currentFoodPatient = collider.gameObject;
            if (_currentFoodPatient == _spawnChallengeController.SelectedPatientFood)
            {
                ConclusionFoodChallenge();
            }
        }

        if (collider.gameObject.CompareTag("Dirt"))
        {
            _canDoClearChallenge = true;
            _inAction = true;
        }

        if (collider.gameObject.CompareTag("Patient"))
        {
            _canDoPacienteCheckChallenge = true;
            _currentInteractedPatient = collider.gameObject;
            _inAction = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == _objectToPickup)
        {
            _objectToPickup = null;
        }
        else if (collider.gameObject.CompareTag("Dirt"))
        {
            _canDoClearChallenge = false;
            ResetInteractionTimer();
            _inAction = false;
        }
        else if (collider.gameObject.CompareTag("Patient"))
        {
            _canDoPacienteCheckChallenge = false;
            _currentInteractedPatient = null;
            ResetInteractionTimer();
            _inAction = false;
        }
    }

    private void StartInteractionTimer()
    {
        if (_isDoingClearChallenge)
        {
            _interactionTimer += Time.deltaTime;
            if (_interactionTimer >= _interactionClearDuration)
            {
                ConclusionClearChallenge();
                _canDoClearChallenge = false;
                _inAction = false;
            }
        }
        else if (_isDoingPacienteChallenge)
        {
            _interactionTimer += Time.deltaTime;
            if (_interactionTimer >= _interactionPacienteDuration)
            {
                ConclusionCheckPacientChallenge(_currentInteractedPatient);
            }
        }
        else
        {
            ResetInteractionTimer();
        }
    }

    private void ResetInteractionTimer()
    {
        _interactionTimer = 0f;
    }

    private void ConclusionCheckPacientChallenge(GameObject patientInteracted)
    {
        if (patientInteracted == _spawnChallengeController.SelectedPatientCheck)
        {
            Debug.Log($"concluiu o teste de checcar o paciente correto");
            _challengeManager.CompleteChallenge(EChallenges.CheckPacient);
            _spawnChallengeController.DestroyCheckPatientChallenge();
            _inAction = false;
        }
    }

    private void ConclusionClearChallenge()
    {
        Debug.Log($"concluiu o teste de limpeza");
        _challengeManager.CompleteChallenge(EChallenges.Clear);
        _spawnChallengeController.DestroyClearChallenge();
    }

    private void ConclusionFoodChallenge()
    {
        _challengeManager.CompleteChallenge(EChallenges.MakeFood);
        _spawnChallengeController.DestroyFoodChallenge();
    }

    private void PickUpObject()
    {
        _objectToPickup.transform.SetParent(transform);
        _objectToPickup.transform.position = _localHand.transform.position;
        _objectToPickup.GetComponent<Rigidbody>().isKinematic = true;
        _isCarryingObject = true;
    }

    private void DropObject()
    {
        _objectToPickup.transform.SetParent(null);
        _objectToPickup.GetComponent<Rigidbody>().isKinematic = false;
        _isCarryingObject = false;
    }

    private void ResetPositionObject()
    {
        _objectToPickup.transform.position = _foodOriginalPosition.transform.position;
        _objectToPickup.transform.rotation = _foodOriginalPosition.transform.rotation;
        _objectToPickup.transform.SetParent(null);
        _objectToPickup.GetComponent<Rigidbody>().isKinematic = false;
        _isCarryingObject = false;
    }

    private bool GetInAction()
    {
        return _inAction;
    }

    private void OnDisable()
    {
        PlayerManager.OnInteractionHandle -= HandlerInteraction;
    }
}