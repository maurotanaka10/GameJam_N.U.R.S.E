using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioTaskCompleteClip;
    [SerializeField] private AudioClip _audioGameCompleteClip;
    [SerializeField, Range(0f, 100f)] private float _volume;

    public bool _soundIsPlaying = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = _volume / 100;
        _soundIsPlaying = true;
    }

    public void SoundTaskComplete()
    {
        _audioSource.clip = _audioTaskCompleteClip;
        _audioSource.Play();
    }

    public void SoundGameComplete()
    {
        if (_soundIsPlaying)
        {
            _audioSource.clip = _audioGameCompleteClip;
            _audioSource.Play();
        }
    }
}
