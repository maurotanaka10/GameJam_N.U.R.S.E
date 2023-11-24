using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioCheckChallengeClip;
    [SerializeField] private AudioClip _audioClearChallengeClip;
    [SerializeField] private AudioClip _audioFoodChallengeClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = _volume / 100;
    }

    public void SoundCheckChallenge()
    {
        _audioSource.clip = _audioCheckChallengeClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    public void SoundClearChallenge()
    {
        _audioSource.clip = _audioClearChallengeClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    public void SoundFoodChallenge()
    {
        _audioSource.clip = _audioFoodChallengeClip;
        _audioSource.Play();
    }

    public void StopAllSounds()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
    }
}
