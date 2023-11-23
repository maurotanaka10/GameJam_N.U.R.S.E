using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private AnimationComponent _animationComponent;

    [SerializeField] private GameObject _interactionGameObject;
    [SerializeField] private Transform _playerTransform;

    private bool _isInteracting = false;

    private void Awake()
    {
        UIManager.OnInteractionUI += HandlerInteractionUI;
    }

    private void Update()
    {
        _interactionGameObject.transform.position = new Vector3(_playerTransform.position.x,
            _interactionGameObject.transform.position.y, _playerTransform.transform.position.z);
        
        HandlerInteractionSlider();
    }

    private void HandlerInteractionSlider()
    {
        if (_isInteracting && _animationComponent.PlayerInAction)
        {
            if(_interactionGameObject != null)
            {
                _interactionGameObject.SetActive(true);
                _interactionGameObject.GetComponent<Slider>().value += Time.deltaTime;
            }
        }
        else
        {
            if (_interactionGameObject != null)
            {
                _interactionGameObject.SetActive(false);
                _interactionGameObject.GetComponent<Slider>().value = 0f;
            }
        }
    }
    
    private void HandlerInteractionUI(bool isInteracting)
    {
        _isInteracting = isInteracting;
    }

    private void OnDisable()
    {
        UIManager.OnInteractionUI -= HandlerInteractionUI;
    }
}