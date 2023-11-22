using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacientScript : MonoBehaviour
{
    [SerializeField] private GameObject _checkIndicator;
    [SerializeField] private GameObject _foodIndicator;

    public void ActivateCheckIndicator()
    {
        _checkIndicator.SetActive(true);
    }

    public void DeactivateCheckIndicator()
    {
        _checkIndicator.SetActive(false);
    }

    public void ActivateFoodIndicator()
    {
        _foodIndicator.SetActive(true);
    }

    public void DeactivateFoodIndicator()
    {
        _foodIndicator.SetActive(false);
    }
}
