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

    private void OnEnable()
    {
        PlayerManager.OnInteract += HandlerInteractionSlider;
        PlayerManager.OnDisableInteract += HandlerDisableInteractionSlider;
    }

    private void Update()
    {
        _interactionGameObject.transform.position = new Vector3(_playerTransform.position.x,
            _interactionGameObject.transform.position.y, _playerTransform.transform.position.z);
        
        ValueSliderController();
    }

    private void ValueSliderController()
    {
        if (_isInteracting)
        {
            _interactionGameObject.GetComponent<Slider>().value += Time.deltaTime;
        }
        else
        {
            _interactionGameObject.GetComponent<Slider>().value = 0f;
        }
    }

    private void HandlerInteractionSlider()
    {
        _interactionGameObject.SetActive(true);

        _isInteracting = true;
    }

    private void HandlerDisableInteractionSlider()
    {
        _interactionGameObject.SetActive(false);

        _isInteracting = false;
    }
    

    private void OnDisable()
    {
        PlayerManager.OnInteract -= HandlerInteractionSlider;
        PlayerManager.OnDisableInteract -= HandlerDisableInteractionSlider;
    }
}