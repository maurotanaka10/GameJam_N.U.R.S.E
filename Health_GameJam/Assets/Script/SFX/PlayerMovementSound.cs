using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    [SerializeField] private AudioClip _startMovingSound;
    [SerializeField] private AudioClip _continuousMoveSound;
    [SerializeField, Range(0f, 100f)] private float _volume;

    public AudioSource _audioSource;

    private void Update()
    {
        _audioSource.volume = _volume / 100;
    }

    public void PlayStartMovingSound()
    {
        _audioSource.clip = _startMovingSound;
        _audioSource.Play();
    }

    public void PlayContinuousMoveSound()
    {
        _audioSource.clip = _continuousMoveSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void StopAllSounds()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
    }
}
