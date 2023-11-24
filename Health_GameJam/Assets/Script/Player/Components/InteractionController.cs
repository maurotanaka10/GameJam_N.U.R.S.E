using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private ChallengeManager _challengeManager;
    [SerializeField] private SpawnChallengeController _spawnChallengeController;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EnviromentSounds _enviromentSounds;
    [SerializeField] private PlayerSounds _playerSounds;

    [SerializeField] private Transform _localHand;
    private GameObject _objectToPickup;
    private bool _isCarryingObject = false;
    private bool _challengeCompleted = false;
    private bool _challengective = false;
    private float _interactionTimer;
    private GameObject _currentInteractedPatient;
    private GameObject _currentFoodPatient;

    public event Action OnErrorTask;

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
                _playerManager.Interact();

                _isDoingClearChallenge = true;
                _playerSounds.SoundClearChallenge();
            }
            else if (_canDoPacienteCheckChallenge)
            {
                _playerManager.Interact();

                _isDoingPacienteChallenge = true;
                _playerSounds.SoundCheckChallenge();
            }
        }
        else
        {
            _playerManager.DisableInteraction();
            _playerSounds.StopAllSounds();
            _isDoingClearChallenge = false;
            _isDoingPacienteChallenge = false;

            _challengective = false;
            _challengeCompleted = false;
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
            _currentFoodPatient = collider.gameObject;
            if (_currentFoodPatient == _spawnChallengeController.SelectedPatientFood)
            {
                ConclusionFoodChallenge();
                ResetPositionObject();
            }
        }

        if (collider.gameObject.CompareTag("Dirt"))
        {
            _canDoClearChallenge = true;
        }

        if (collider.gameObject.CompareTag("Patient"))
        {
            _canDoPacienteCheckChallenge = true;
            _currentInteractedPatient = collider.gameObject;
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
        }
        else if (collider.gameObject.CompareTag("Patient"))
        {
            _canDoPacienteCheckChallenge = false;
            _currentInteractedPatient = null;
            ResetInteractionTimer();
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

                _playerManager.DisableInteraction();
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
            _challengeManager.CompleteChallenge(EChallenges.CheckPacient);
            _spawnChallengeController.DestroyCheckPatientChallenge();

            _playerManager.DisableInteraction();
            _enviromentSounds.SoundTaskComplete();
            _playerSounds.StopAllSounds();
            _challengeCompleted = true;
        }
        else
        {
            _challengective = true;
            
            if (_challengective && !_challengeCompleted)
            {
                OnErrorTask?.Invoke();
                _playerManager.DisableInteraction();
            }

        }
    }

    private void ConclusionClearChallenge()
    {
        _challengeManager.CompleteChallenge(EChallenges.Clear);
        _spawnChallengeController.DestroyClearChallenge();
        _enviromentSounds.SoundTaskComplete();
        _playerSounds.StopAllSounds();
    }

    private void ConclusionFoodChallenge()
    {
        _challengeManager.CompleteChallenge(EChallenges.MakeFood);
        _spawnChallengeController.DestroyFoodChallenge();
        _enviromentSounds.SoundTaskComplete();
    }

    private void PickUpObject()
    {
        _objectToPickup.transform.SetParent(transform);
        _objectToPickup.transform.position = _localHand.transform.position;
        _objectToPickup.GetComponent<Rigidbody>().isKinematic = true;
        _isCarryingObject = true;

        _playerSounds.SoundFoodChallenge();
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

    private void OnDisable()
    {
        PlayerManager.OnInteractionHandle -= HandlerInteraction;
    }
}